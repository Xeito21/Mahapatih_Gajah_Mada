using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class RelikChest : MonoBehaviour, ISaveManager
{
    [System.Serializable]
    public class DropItem
    {
        public GameObject item;
        public float dropChanceRelik;
    }

    [SerializeField] private int ChestRelikID;
    [SerializeField] private TextMeshProUGUI interactPrompt;
    [SerializeField] private GameObject inputPrompt;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private AnimationCurve fadeCurve;
    [SerializeField] private float enemyCheckRadius = 5f;
    [SerializeField] private LayerMask enemyLayerMask;
    [SerializeField] private int gobogDrop;
    public List<DropItem> dropRelik = new List<DropItem>();
    private bool isPlayerInRange = false;
    private bool isEnemyNearby = false;
    private bool isOpen = false;
    private Animator chestAnimator;

    private void Start()
    {
        chestAnimator = GetComponentInChildren<Animator>();

        if (isOpen)
        {
            gameObject.SetActive(false);
        }
        else
        {
            if (interactPrompt != null)
            {
                interactPrompt.gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (isEnemyNearby)
            {
                AudioManager.instance.PlaySFX(35, null);
                User_Interfaces.instance.StartCoroutine(User_Interfaces.instance.DisplayPopupText("Harta karun masih terkunci!"));
            }
            else
            {
                TryInteract();
                SaveManager.instance.SaveGame();
            }
        }
    }

    private void TryInteract()
    {
        if (isOpen)
            return;

        List<GameObject> selectedItems = ChooseItemsToDrop();

        foreach (GameObject item in selectedItems)
        {
            Interact(item);
        }

        isOpen = true;


        
    }

    private void Interact(GameObject item)
    {
        if (isOpen)
            return;

        if (item != null)
        {
            AudioManager.instance.PlaySFX(38, null);
            PlayerManager.instance.currency += gobogDrop;
            inputPrompt.gameObject.SetActive(false);
            GameObject spawnedItem = Instantiate(item, transform.position, Quaternion.identity);

            Rigidbody2D itemRigidbody = spawnedItem.GetComponent<Rigidbody2D>();

            if (itemRigidbody == null)
            {
                itemRigidbody = spawnedItem.AddComponent<Rigidbody2D>();
            }
            float randomForce = Random.Range(1f, 6f);
            float randomDirection = Random.Range(0f, 1f);
            Vector3 forceDirection = (randomDirection < 0.5f) ? Vector3.left : Vector3.right;

            itemRigidbody.AddForce(Vector3.up * randomForce, ForceMode2D.Impulse);
            itemRigidbody.AddForce(forceDirection * randomForce, ForceMode2D.Impulse);

            chestAnimator.SetBool("isOpen", true); // Mengaktifkan animasi terbuka pada chest
            User_Interfaces.instance.StartCoroutine(User_Interfaces.instance.DisplayPopupText("Harta Karun Terbuka!"));
            StartCoroutine(FadeAndDestroy());
        }
    }

    private List<GameObject> ChooseItemsToDrop()
    {
        List<GameObject> selectedItems = new List<GameObject>();

        foreach (DropItem dropItem in dropRelik)
        {
            if (Random.value <= dropItem.dropChanceRelik)
            {
                selectedItems.Add(dropItem.item);
            }
        }

        return selectedItems;
    }

    private IEnumerator FadeAndDestroy()
    {
        float timer = 0f;
        Renderer renderer = GetComponentInChildren<Renderer>();

        Color initialColor = renderer.material.color;

        while (timer < fadeDuration)
        {
            float progress = timer / fadeDuration;

            float opacity = fadeCurve.Evaluate(progress);
            Color fadeColor = new Color(initialColor.r, initialColor.g, initialColor.b, opacity);
            renderer.material.color = fadeColor;

            yield return null;

            timer += Time.deltaTime;
        }

        Color transparentColor = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);
        renderer.material.color = transparentColor;

        Destroy(gameObject);
        isOpen = true;
    }

    private bool IsEnemyNearby()
    {
        Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(transform.position, enemyCheckRadius, enemyLayerMask);
        foreach (Collider2D collider in enemyColliders)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                return true;
            }
        }
        return false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            inputPrompt.gameObject.SetActive(true);
            isEnemyNearby = IsEnemyNearby();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            inputPrompt.gameObject.SetActive(false);
            isEnemyNearby = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Menggambar gizmo berbentuk lingkaran untuk deteksi musuh
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyCheckRadius);
    }

    public void LoadData(GameData _data)
    {
        if (_data.chestRelik.TryGetValue(ChestRelikID, out bool value))
        {
            isOpen = value;
            if (isOpen)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        if (_data.chestRelik.TryGetValue(ChestRelikID, out bool value))
        {
            _data.chestRelik.Remove(ChestRelikID);
            _data.chestRelik.Add(ChestRelikID, isOpen);
        }
        else
            _data.chestRelik.Add(ChestRelikID, isOpen);
    }
}

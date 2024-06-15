using System.Collections;
using TMPro;
using UnityEngine;

public class ChestDrop : MonoBehaviour, ISaveManager
{
    [SerializeField] private int chestID;
    [SerializeField] private int gobogDrop;
    [SerializeField] private TextMeshProUGUI interactPrompt;
    [SerializeField] private GameObject inputPrompt;
    [SerializeField] private float enemyCheckRadius = 5f;
    [SerializeField] private LayerMask enemyLayerMask;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private AnimationCurve fadeCurve;
    private Animator chestAnimator;
    private ItemDrop myDropSystem;
    public bool isOpen = false;
    private bool playerInRange = false;
    private bool isEnemyNearby = false;

    void Start()
    {
        if (interactPrompt != null)
        {
            interactPrompt.gameObject.SetActive(false);
        }
        chestAnimator = GetComponentInChildren<Animator>();
        myDropSystem = GetComponent<ItemDrop>();
    }

    void Update()
    {
        if (playerInRange && !isOpen && Input.GetKeyDown(KeyCode.E))
        {
            if (isEnemyNearby)
            {
                AudioManager.instance.PlaySFX(35, null);
                User_Interfaces.instance.StartCoroutine(User_Interfaces.instance.DisplayPopupText("Harta karun masih terkunci!"));
            }
            else
            {
                Interact();
            }
        }
    }

    private void OpenChest()
    {
        AudioManager.instance.PlaySFX(38, null);
        myDropSystem.GenerateDrop();
        chestAnimator.SetBool("isOpen", true);
        PlayerManager.instance.currency += gobogDrop;
        isOpen = true;
        StartCoroutine(FadeAndDestroy());
    }

    private void Interact()
    {
        if (isOpen)
            return;
        OpenChest();
        User_Interfaces.instance.StartCoroutine(User_Interfaces.instance.DisplayPopupText("Harta Karun Terbuka!"));
        SaveManager.instance.SaveGame();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true; // Set status pemain berada di area interaksi menjadi true
            inputPrompt.gameObject.SetActive(true);
            isEnemyNearby = IsEnemyNearby();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false; // Set status pemain berada di area interaksi menjadi false
            inputPrompt.gameObject.SetActive(false);
            isEnemyNearby = false;
        }
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

    private void OnDrawGizmosSelected()
    {
        // Menggambar gizmo berbentuk lingkaran untuk deteksi musuh
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyCheckRadius);
    }

    public void LoadData(GameData _data)
    {
        if (_data.chest.TryGetValue(chestID, out bool value))
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
        if (_data.chest.TryGetValue(chestID, out bool value))
        {
            _data.chest.Remove(chestID);
            _data.chest.Add(chestID, isOpen);
        }
        else
            _data.chest.Add(chestID, isOpen);
    }
}

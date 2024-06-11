using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class RelikChest : MonoBehaviour
{
    [System.Serializable]
    public class DropItem
    {
        public GameObject item;// Prefab atau GameObject yang akan dikeluarkan
        public float dropChance; // Peluang item muncul
    }

    [SerializeField] private TextMeshProUGUI interactPrompt; // UI Text untuk prompt interaksi
    [SerializeField] private GameObject inputPrompt;
    [SerializeField] private float fadeDuration = 1f; // Durasi fading
    [SerializeField] private AnimationCurve fadeCurve; // Kurva fading
    [SerializeField] private float enemyCheckRadius = 5f; // Radius untuk deteksi musuh
    [SerializeField] private LayerMask enemyLayerMask; // Layer mask untuk musuh
    public List<DropItem> dropItems = new List<DropItem>(); // List item yang mungkin di-drop
    private List<GameObject> itemsToSpawn = new List<GameObject>();
    private bool isPlayerInRange = false;
    private bool isEnemyNearby = false;
    private bool isOpened = false;
    private Animator chestAnimator;

    private void Start()
    {
        if (interactPrompt != null)
        {
            interactPrompt.gameObject.SetActive(false);
        }

        chestAnimator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (isEnemyNearby)
            {
                AudioManager.instance.PlaySFX(35, null);
                User_Interfaces.instance.StartCoroutine(User_Interfaces.instance.DisplayPopupText("Kalahkan Prajurit tersebut!"));
            }
            else
            {
                TryInteract();
            }
        }
    }

    private void TryInteract()
    {
        List<GameObject> selectedItems = ChooseItemsToDrop();

        foreach (GameObject item in selectedItems)
        {
            Interact(item);
        }

        isOpened = true;
    }

    /* private DropItem ChooseItemToDrop()
    {
        float totalChance = 0f;

        foreach (var item in dropItems)
        {
            totalChance += item.dropChance;
        }

        float randomValue = Random.Range(0f, totalChance);

        foreach (var item in dropItems)
        {
            if (randomValue <= item.dropChance)
            {
                return item;
            }

            randomValue -= item.dropChance;
        }

        return null;
    }
    */

    private void Interact(GameObject item)
    {
        if (isOpened == true)
            return;
        if (item != null)
        {
            AudioManager.instance.PlaySFX(38, null);
            inputPrompt.gameObject.SetActive(false);
            GameObject spawnedItem = Instantiate(item, transform.position, Quaternion.identity);
            Rigidbody2D itemRigidbody = spawnedItem.GetComponent<Rigidbody2D>();

            if (itemRigidbody == null)
            {
                // Tambahkan komponen Rigidbody jika belum ada
                itemRigidbody = spawnedItem.AddComponent<Rigidbody2D>();
            }

            // Buat nilai acak untuk gaya dari 1f hingga 6f
            float randomForce = Random.Range(1f, 6f);

            // Buat nilai acak untuk arah gaya
            float randomDirection = Random.Range(0f, 1f);
            Vector3 forceDirection = (randomDirection < 0.5f) ? Vector3.left : Vector3.right;

            // Tambahkan gaya ke item agar terlempar ke atas, kanan, atau kiri dengan gaya acak
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

        foreach (DropItem dropItem in dropItems)
        {
            if (Random.value <= dropItem.dropChance)
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

        // Mendapatkan opacity awal
        Color initialColor = renderer.material.color;

        while (timer < fadeDuration)
        {
            // Menghitung progress fading
            float progress = timer / fadeDuration;

            // Menerapkan fading menggunakan kurva yang dipilih
            float opacity = fadeCurve.Evaluate(progress);
            Color fadeColor = new Color(initialColor.r, initialColor.g, initialColor.b, opacity);
            renderer.material.color = fadeColor;

            // Menunggu frame selanjutnya
            yield return null;

            // Mengupdate timer
            timer += Time.deltaTime;
        }

        // Set opacity ke 0 (transparan)
        Color transparentColor = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);
        renderer.material.color = transparentColor;

        // Hapus chest setelah fading selesai
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
            isEnemyNearby = false;
            inputPrompt.gameObject.SetActive(false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Menggambar gizmo berbentuk lingkaran untuk deteksi musuh
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyCheckRadius);
    }
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class RelikChest : MonoBehaviour
{
    [System.Serializable]
    public class DropItem
    {
        public GameObject item;// Prefab atau GameObject yang akan dikeluarkan
        public float dropChance; // Peluang item muncul
    }

    public List<DropItem> dropItems = new List<DropItem>(); // List item yang mungkin di-drop
    private List<GameObject> itemsToSpawn = new List<GameObject>();
    [SerializeField] private TextMeshProUGUI interactPrompt; // UI Text untuk prompt interaksi
    private bool isPlayerInRange = false;
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
            TryInteract();
        }
    }

    private void TryInteract()
    {
        List<GameObject> selectedItems = ChooseItemsToDrop();

        foreach (GameObject item in selectedItems)
        {
            Interact(item);
        }
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
        if (item != null)
        {
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


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            if (interactPrompt != null)
            {
                interactPrompt.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (interactPrompt != null)
            {
                interactPrompt.gameObject.SetActive(false);
            }
        }
    }
}

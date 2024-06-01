using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteraction : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<GajahMada>() != null)
        {
            PlayerManager.instance.QuestOnProgress();
            Destroy(gameObject);
        }
    }
}

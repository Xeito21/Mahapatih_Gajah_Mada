using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keys : MonoBehaviour
{
    [SerializeField] private int keysAmount;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<GajahMada>() != null)
        {
            PlayerManager.instance.keysCurrency += keysAmount;     
            Destroy(gameObject);
        }
    }
}

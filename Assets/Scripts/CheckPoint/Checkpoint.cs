using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Animator anim;
    public string checkpointId;
    public bool checkPointStatus;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }


    [ContextMenu("Generate Checkpoint ID")]
    private void GenerateId()
    {
        checkpointId = System.Guid.NewGuid().ToString();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<GajahMada>() != null)
        {
            ActivateCheckPoint();
        }
    }

    public void ActivateCheckPoint()
    {
        if(checkPointStatus == false)
            AudioManager.instance.PlaySFX(6,null);

        checkPointStatus = true;
        anim.SetBool("active", true);
    }
}

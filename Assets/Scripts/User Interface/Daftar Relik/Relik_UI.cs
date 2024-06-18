using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Relik_UI : MonoBehaviour
{
    public Relik_Tooltip relikTooltip;
    void Start()
    {
        relikTooltip.gameObject.SetActive(false);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_UI : MonoBehaviour
{
    public Level_ToolTip levelTooltip;
    void Start()
    {
        levelTooltip.gameObject.SetActive(false);
    }

}

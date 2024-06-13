using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Options : MonoBehaviour
{
    public void SaveExit()
    {
        GameManager.instance.BackToMenuScene();
    }
}

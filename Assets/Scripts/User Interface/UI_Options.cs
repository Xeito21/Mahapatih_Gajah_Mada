using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Options : MonoBehaviour
{
    public void SaveExit()
    {
        GameManager.instance.BackToMenuScene();
    }
}

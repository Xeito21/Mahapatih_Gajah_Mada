using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Light_Hole_Hotkey_Controller : MonoBehaviour
{
    private SpriteRenderer sr;
    private KeyCode myHotKey;
    private TextMeshProUGUI myText;

    private Transform enemiesTransform;
    private Light_Hole_Skill_Controller lightHole;



    public void SetupHotKey(KeyCode _myHotKey, Transform _myEnemy, Light_Hole_Skill_Controller _myLightHole)
    {
        sr = GetComponent<SpriteRenderer>();
        myText = GetComponentInChildren<TextMeshProUGUI>();

        enemiesTransform = _myEnemy;
        lightHole = _myLightHole;
        myHotKey = _myHotKey;
        myText.text = _myHotKey.ToString();
    }

    private void Update()
    {
        if(Input.GetKeyDown(myHotKey))
        {
            lightHole.AddEnemyToList(enemiesTransform);

            myText.color = Color.clear;
            sr.color = Color.clear;
        }
    }
}

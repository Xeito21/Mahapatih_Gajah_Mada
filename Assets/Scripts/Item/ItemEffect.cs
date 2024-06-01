using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemEffect : ScriptableObject
{
    [TextArea]
    public string effectDesc;
    public virtual void EksekusiEffect(Transform _enemyPosition)
    {
        Debug.Log("effect terapply");
    }
}

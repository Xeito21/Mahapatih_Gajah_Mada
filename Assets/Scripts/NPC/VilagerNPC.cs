using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VilagerNPC : MonoBehaviour
{
    [SerializeField] private ScriptableObject pedangMahapatih;
    [SerializeField] private ScriptableObject kalungMahaptih;


    private void Update()
    {
        string namaEquipment = ((Ink.Runtime.StringValue)DialogueManager.GetInstance().GetVariableState("nama_equipment")).value;

        switch(namaEquipment)
        {
            case "":
                break;
            case "Pedang":
                //GameObject clone = Instantiate(vilagerPrefab, Vector3.zero, Quaternion.identity);
                break;
            case "Kalung":
                Instantiate(kalungMahaptih, Vector3.zero, Quaternion.identity);
                break;
            default:
                Debug.LogWarning("Nama equipment tidak adah di switch statement" + namaEquipment);
                break;

        }

    }
}

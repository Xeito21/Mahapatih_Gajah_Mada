using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;

public class EntityFX : MonoBehaviour
{
    protected GajahMada player;
    protected SpriteRenderer sr;
    [Header("Popup Text")]
    [SerializeField] private GameObject popupTextPrefab;


    [Header("Flash Effect")]
    [SerializeField] private Material hitMat;
    private Material originalMat;

    [Header("Element Colors")]
    [SerializeField] private Color[] bekuColor;
    [SerializeField] private Color[] terbakarColor;
    [SerializeField] private Color[] tersengatColor;
    
    [Header("Element Particles")]
    [SerializeField] private ParticleSystem burnFx;
    [SerializeField] private ParticleSystem freezeFx;
    [SerializeField] private ParticleSystem shockFx;

    [Header("Hit FX")]
    [SerializeField] private GameObject hitFx;
    [SerializeField] private GameObject critHitFx;

    protected virtual void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        player = PlayerManager.instance.player;
        originalMat = sr.material;
    }


    public void CreatePopUpText(string _text)
    {
        float randomX = Random.Range(-1,1);
        float randomY = Random.Range(1.5f,3);
        Vector3 positionOffset = new Vector3(randomX,randomY,0);
        GameObject newText = Instantiate(popupTextPrefab, transform.position + positionOffset, Quaternion.identity);

        newText.GetComponent<TextMeshPro>().text = _text;
    }
    private IEnumerator FlashEffect()
    {
        sr.material = hitMat;
        Color currentColor = sr.color;
        sr.color = Color.white;
        

        yield return new WaitForSeconds(.2f);
        sr.color = currentColor;

        sr.material = originalMat;
    }

    private void RedColorBlink()
    {
        if(sr.color != Color.white)
            sr.color = Color.white;
        else
            sr.color = Color.red;
    }

    private void CancelColorChange()
    {
        CancelInvoke();
        sr.color = Color.white;

        burnFx.Stop();
        freezeFx.Stop();
        shockFx.Stop();
    }

    public void bekuColorFxFor(float _seconds)
    {
        freezeFx.Play();
        InvokeRepeating("BekuColorFx", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }

    public void TersengatFxFor(float _seconds)
    {
        shockFx.Play();
        InvokeRepeating("TersengatColorFx", 0,.3f);
        Invoke("CancelColorChange", _seconds);
    }

    public void TerbakarFxFor(float _seconds)
    {
        burnFx.Play();
        InvokeRepeating("TerbakarColorFx", 0,.3f);
        Invoke("CancelColorChange", _seconds);
    }

    private void TerbakarColorFx()
    {
        if(sr.color != terbakarColor[0])
            sr.color = terbakarColor[0];
        else
            sr.color = terbakarColor[1];
    }

    private void TersengatColorFx()
    {
        if(sr.color != tersengatColor[0])
            sr.color = tersengatColor[0];
        else
            sr.color = tersengatColor[1];
    }

    public void MakeTransparent(bool _transparent)
    {
        if(_transparent)
            sr.color = Color.clear;
        else
            sr.color = Color.white;
    }

    private void BekuColorFx()
    {
        if(sr.color != bekuColor[0])
            sr.color = bekuColor[0];
        else
            sr.color = bekuColor[1];
    }

    public void CreateHitFX(Transform _target, bool _critical)
    {
        float zRotation = Random.Range(-90,90);
        float xPosition = Random.Range(-.5f, .5f);
        float yPosition = Random.Range(-.5f, .5f);

        Vector3 hitFxRotation = new Vector3(0,0, zRotation);

        GameObject hitPrefab = hitFx;

        if(_critical)
        {
            hitPrefab = critHitFx;
            float yRotation = 0;
            zRotation = Random.Range(-45,45);

            if(GetComponent<Entity>().facingDir == -.5f)
                yRotation = 180;

            hitFxRotation = new Vector3(0,yRotation,zRotation);

        }

        GameObject newHitFx = Instantiate(hitPrefab,_target.position + new Vector3(xPosition,yPosition),Quaternion.identity, _target);
        newHitFx.transform.Rotate(hitFxRotation);
        Destroy(newHitFx, .5f);

    }
}

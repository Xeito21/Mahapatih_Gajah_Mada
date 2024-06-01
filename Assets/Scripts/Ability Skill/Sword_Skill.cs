using System;
using UnityEngine;
using UnityEngine.UI;

public enum TipePedang
{
    Biasa,
    Mantul,
    Tembus,
    Memutar
}

public class Sword_Skill : Skill
{
    public TipePedang tipePedang = TipePedang.Biasa;
    [Header("Mantul Info")]
    [SerializeField] private int jumlahMantul;
    [SerializeField] private float mantulGravity;
    [SerializeField] private float mentalSpeed;
    [SerializeField] private UI_SlotSkillTree bounceUnlockBtn;


    [Header("Tembus Info")]
    [SerializeField] private int jumlahTembus;
    [SerializeField] private float tembusGravity;
    [SerializeField] private UI_SlotSkillTree pierceUnlockBtn;

    [Header("Memutar Info")]
    [SerializeField] private float hitCooldown = .35f;
    [SerializeField] private float maxTravelDistance = 7;
    [SerializeField] private float spinDuration = 2;
    [SerializeField] private float spinGravity = 1;
    [SerializeField] private UI_SlotSkillTree spinUnlockBtn;

    [Header("Skill Info")]
    [SerializeField] private UI_SlotSkillTree swordUnlockBtn;
    [SerializeField] private GameObject pedangPrefab;
    [SerializeField] private Vector2 launchForce;
    [SerializeField] private float swordGravity;
    [SerializeField] private float freezeTimeDuration;
    [SerializeField] private float returnSpeed;
    public bool swordUnlocked{get; private set;}

    [Header("Passive Skills")]
    [SerializeField] private UI_SlotSkillTree temporalValorBtn;
    [SerializeField] private UI_SlotSkillTree shatterstrikeHurlBtn;
    public bool temporalValorUnlocked {get; private set;}
    public bool shatterstrikeHurlUnlocked {get; private set;}
    

    private Vector2 finalDir;

    [Header("Aim Sword Target")]
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBetweenDots;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotsParent;

    private GameObject[] dots;


    protected override void Start()
    {
        base.Start();
        GenerateDots();

        SetupGravity();

        swordUnlockBtn.GetComponent<Button>().onClick.AddListener(UnlockSword);
        bounceUnlockBtn.GetComponent<Button>().onClick.AddListener(UnlockBounceSword);
        pierceUnlockBtn.GetComponent<Button>().onClick.AddListener(UnlockPierceSword);
        spinUnlockBtn.GetComponent<Button>().onClick.AddListener(UnlockSpinSword);
        temporalValorBtn.GetComponent<Button>().onClick.AddListener(UnlockTemporalValor);
        shatterstrikeHurlBtn.GetComponent<Button>().onClick.AddListener(UnlockShatterStrikeHurl);
    }

    private void SetupGravity()
    {
        if (tipePedang == TipePedang.Mantul)
            swordGravity = mantulGravity;
        else if (tipePedang == TipePedang.Tembus)
            swordGravity = tembusGravity;
        else if (tipePedang == TipePedang.Memutar)
            swordGravity = spinGravity;
    }

    protected override void Update()
    {
        if (Input.GetKeyUp(KeyCode.O))
            finalDir = new Vector2(AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y);

        if (Input.GetKey(KeyCode.O))
        {
            for (int i = 0; i < dots.Length; i++)
            {
                dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
            }
        }
    }

    protected override void CheckUnlock()
    {
        UnlockSword();
        UnlockShatterStrikeHurl();
        UnlockPierceSword();
        UnlockBounceSword();
        UnlockSpinSword();
        UnlockTemporalValor();
    }

    private void UnlockTemporalValor()
    {
        if(temporalValorBtn.unlocked)
            temporalValorUnlocked = true;
    }

    private void UnlockShatterStrikeHurl()
    {
        if(shatterstrikeHurlBtn.unlocked)
            shatterstrikeHurlUnlocked = true;
    }

    private void UnlockSword()
    {
        if(swordUnlockBtn.unlocked)
        {
            tipePedang = TipePedang.Biasa;
            swordUnlocked = true;
        }
    }

    private void UnlockBounceSword()
    {
        if(bounceUnlockBtn.unlocked)
            tipePedang = TipePedang.Mantul;
    }

    private void UnlockPierceSword()
    {
        if(pierceUnlockBtn.unlocked)
            tipePedang = TipePedang.Tembus;
    }

    private void UnlockSpinSword()
    {
        if(spinUnlockBtn.unlocked)
            tipePedang = TipePedang.Memutar;
    }

    public void CreateSword()
    {
        GameObject newSword = Instantiate(pedangPrefab, player.transform.position, transform.rotation);
        Sword_Skill_Controller newSwordScript = newSword.GetComponent<Sword_Skill_Controller>();

        newSwordScript.SetupSword(finalDir, swordGravity, player, freezeTimeDuration,returnSpeed);

        if (tipePedang == TipePedang.Mantul)
            newSwordScript.SetupMental(true, jumlahMantul,mentalSpeed);
        else if (tipePedang == TipePedang.Tembus)
            newSwordScript.SetupTembus(jumlahTembus);
        else if (tipePedang == TipePedang.Memutar)
            newSwordScript.SetupMemutar(true, maxTravelDistance, spinDuration,hitCooldown);

        player.AssignNewSword(newSword);
        DotsActive(false);
    }

    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - playerPosition;

        return direction;
    }

    public void DotsActive(bool _isActive)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(_isActive);
        }
    }

    private void GenerateDots()
    {
        dots = new GameObject[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParent);
            dots[i].SetActive(false);
        }
    }

    private Vector2 DotsPosition(float t)
    {
        Vector2 position = (Vector2)player.transform.position + new Vector2(
            AimDirection().normalized.x * launchForce.x,
            AimDirection().normalized.y * launchForce.y) * t + .5f * (Physics2D.gravity * swordGravity) * (t * t);
        return position;
    }
}

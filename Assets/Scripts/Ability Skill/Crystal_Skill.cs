using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Crystal_Skill : Skill
{
    [SerializeField] private float crystalDuration;
    [SerializeField] private GameObject crystalPrefab;
    private GameObject currentCrystal;

    [Header("Crystal Mirage")]
    [SerializeField] private bool cloneInsteadOfCrystal;
    [SerializeField] private UI_SlotSkillTree unlockCrystalMirageBtn;

    [Header("Crystal Simple")]
    [SerializeField] private UI_SlotSkillTree unlockCrystalBtn;
    public bool crystalUnlocked {get; private set;}

    [Header("Explosive Crystal")]
    [SerializeField] private UI_SlotSkillTree unlockCrystalExplodeBtn;
    [SerializeField] private float explosiveCooldown;
    [SerializeField] private bool canExplode;

    [Header("Moving Crystal")]
    [SerializeField] private UI_SlotSkillTree unlockCrystalMovingBtn;
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float moveSpeed;

    [Header("Multi Stacking Crstyal")]
    [SerializeField] private UI_SlotSkillTree unlockMultipleCrystalBtn;
    [SerializeField] private bool canUseMultiStacks;
    [SerializeField] private int amountofStacks;
    [SerializeField] private float multiStackCooldown;
    [SerializeField] private float useTimeWindow;
    [SerializeField] private List<GameObject> crystalLeft = new List<GameObject>();

    protected override void Start()
    {
        base.Start();
        unlockCrystalBtn.GetComponent<Button>().onClick.AddListener(UnlockCrystal);
        unlockCrystalMirageBtn.GetComponent<Button>().onClick.AddListener(UnlockCrystalMirage);
        unlockCrystalExplodeBtn.GetComponent<Button>().onClick.AddListener(UnlockExplosionCrystal);
        unlockCrystalMovingBtn.GetComponent<Button>().onClick.AddListener(UnlockMovingCrystal);
        unlockMultipleCrystalBtn.GetComponent<Button>().onClick.AddListener(UnlockMultiStack);
    }

    protected override void CheckUnlock()
    {
        UnlockCrystal();
        UnlockCrystalMirage();
        UnlockExplosionCrystal();
        UnlockMultiStack();
    }

    private void UnlockCrystal()
    {
        if(unlockCrystalBtn.unlocked)
            crystalUnlocked = true;
    }

    private void UnlockCrystalMirage()
    {
        if(unlockCrystalMirageBtn.unlocked)
            cloneInsteadOfCrystal = true;
    }

    private void UnlockExplosionCrystal()
    {
        if(unlockCrystalExplodeBtn.unlocked)
        {
            canExplode = true;
            coolDown = explosiveCooldown;
        }
    }

    private void UnlockMovingCrystal()
    {
        if(unlockCrystalMovingBtn)
            canMoveToEnemy = true;
    }

    private void UnlockMultiStack()
    {
        if(unlockMultipleCrystalBtn.unlocked)  
            canUseMultiStacks = true;
    }

    public override void UseSkill()
    {
        base.UseSkill();
        if(CanUseMultiCrystal())
            return;

        if(currentCrystal == null)
        {
            CreateCrystal();
        }

        else
        {
            if(canMoveToEnemy)
                return;
            Vector2 playerPos = player.transform.position;
            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPos;

            if(cloneInsteadOfCrystal)
            {
                SkillManager.instance.clone.CreateClone(currentCrystal.transform, Vector3.zero);
                Destroy(currentCrystal);
            }
            else
            {
                currentCrystal.GetComponent<Crystal_Skill_Controller>()?.CrystalHasGone();
            }
        }
    }

    public void CreateCrystal()
    {
        currentCrystal = Instantiate(crystalPrefab, player.transform.position, quaternion.identity);
        Crystal_Skill_Controller currentCrystalScript = currentCrystal.GetComponent<Crystal_Skill_Controller>();

        currentCrystalScript.SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindMusuhTerdekat(currentCrystal.transform), player);
    }

    public void CurrentCrystalRandomTarget() => currentCrystal.GetComponent<Crystal_Skill_Controller>().ChooseRandomEnemy();

    private bool CanUseMultiCrystal()
    {
        if(canUseMultiStacks)
        {
            if(crystalLeft.Count >0)
            {
                if(crystalLeft.Count == amountofStacks)
                    Invoke("ResetAbility", useTimeWindow);
                coolDown = 0;
                GameObject crystaltoSpawn = crystalLeft[crystalLeft.Count - 1];
                GameObject newCrystal = Instantiate(crystaltoSpawn,player.transform.position, quaternion.identity);

                crystalLeft.Remove(crystaltoSpawn);
                newCrystal.GetComponent<Crystal_Skill_Controller>().
                SetupCrystal(crystalDuration,canExplode,canMoveToEnemy, moveSpeed,FindMusuhTerdekat(newCrystal.transform), player);

                if(crystalLeft.Count <= 0)
                {
                    coolDown = multiStackCooldown;
                    MengisiUlang();
                }
            return true;
            }

        }

        return false;
    }

    private void MengisiUlang()
    {
        int amountToAdd = amountofStacks - crystalLeft.Count;
        for (int i = 0; i < amountToAdd; i++)
        {
            crystalLeft.Add(crystalPrefab);
        }
    }

    private void ResetAbility()
    {
        if(cooldownTimer > 0)
            return;
        cooldownTimer = multiStackCooldown;
        MengisiUlang();
    }

}

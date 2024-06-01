using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Lighthole_Skill : Skill
{
    [SerializeField] private UI_SlotSkillTree vortexUnlockBtn;
    [SerializeField] private int amountOfAttacks;
    [SerializeField] private GameObject lightHolePrefab;
    [SerializeField] private float cloneCooldown;
    [SerializeField] private float lightholeDuration;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;
    public bool vortexUnlocked {get; private set;}

    Light_Hole_Skill_Controller currentLighthole;

    private void UnlockLuminousVortex()
    {
        if(vortexUnlockBtn.unlocked)
            vortexUnlocked = true;
    }

    public override bool CanUseSkillI()
    {
        return base.CanUseSkillI();
    }

    public override void UseSkill()
    {
        base.UseSkill();
        GameObject newLightHole = Instantiate(lightHolePrefab, player.transform.position, quaternion.identity);

        currentLighthole = newLightHole.GetComponent<Light_Hole_Skill_Controller>();
        currentLighthole.SetupLightHole(maxSize, growSpeed, shrinkSpeed, amountOfAttacks, cloneCooldown,lightholeDuration);

        AudioManager.instance.PlaySFX(13,player.transform);
    }

    protected override void Start()
    {
        base.Start();

        vortexUnlockBtn.GetComponent<Button>().onClick.AddListener(UnlockLuminousVortex);
    }

    protected override void Update()
    {
        base.Update();
    }

    public bool SkillComplete()
    {
        if(!currentLighthole)
            return false;
        if(currentLighthole.playerCanExitState)
        {
            currentLighthole = null;
            return true;
        }
        return false;
    }

    public float GetLightholeRadius()
    {
        return maxSize / 2;
    }

    protected override void CheckUnlock()
    {
        UnlockLuminousVortex();
    }
}

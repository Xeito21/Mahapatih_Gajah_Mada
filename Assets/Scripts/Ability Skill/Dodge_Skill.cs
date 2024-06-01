using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dodge_Skill : Skill
{
    [Header("Dodge")]
    [SerializeField] UI_SlotSkillTree unlockDodgeBtn;
    [SerializeField] private int evasionAmount;
    public bool dodgeUnlocked;

    [Header("Clone Dodge")]
    [SerializeField] private UI_SlotSkillTree unlockCloneDodgeBtn;
    public bool cloneDodgeUnlocked;

    protected override void Start()
    {
        base.Start();
        unlockDodgeBtn.GetComponent<Button>().onClick.AddListener(UnlockDodge);
        unlockCloneDodgeBtn.GetComponent<Button>().onClick.AddListener(UnlockCloneDodge);
    }

    protected override void CheckUnlock()
    {
        UnlockDodge();
        UnlockCloneDodge();
    }

    private void UnlockDodge()
    {
        if(unlockDodgeBtn.unlocked)
        {
            player.stats.penghindar.AddModifier(evasionAmount);
            Bagspace.instance.UpdateStatsUI();
            dodgeUnlocked = true;
        }
    }

    private void UnlockCloneDodge()
    {
        if(unlockCloneDodgeBtn.unlocked && !dodgeUnlocked)
            cloneDodgeUnlocked = true;
    }

    public void CreateCloneDodge()
    {
        if(cloneDodgeUnlocked)
            SkillManager.instance.clone.CreateClone(player.transform, new Vector3(2 * player.facingDir ,0));
    }

}

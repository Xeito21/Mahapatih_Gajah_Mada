using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Dash_Skill : Skill
{
    [Header("Dash")]
    [SerializeField] private UI_SlotSkillTree dashUnlockBtn;
    public bool dashUnlocked {get; private set;}

    [Header("Clone Dash")]
    [SerializeField] private UI_SlotSkillTree dashCloneUnlockBtn;
    public bool cloneOnDashUnlocked {get; private set;}
    [Header("Clone Arrival")]
    [SerializeField] private UI_SlotSkillTree dashArrivalUnlockBtn;
    public bool cloneOnArrivalUnlocked {get; private set;}

    public override void UseSkill()
    {
        base.UseSkill();
    }

    protected override void Start()
    {
        base.Start();
        dashUnlockBtn.GetComponent<Button>().onClick.AddListener(UnlockDash);
        dashCloneUnlockBtn.GetComponent<Button>().onClick.AddListener(UnlockCloneOnDash);
        dashArrivalUnlockBtn.GetComponent<Button>().onClick.AddListener(UnlockArrivalOnDash);
    }

    protected override void CheckUnlock()
    {
        UnlockDash();
        UnlockCloneOnDash();
        UnlockArrivalOnDash();
    }


    private void UnlockDash()
    {
        if(dashUnlockBtn.unlocked)
            dashUnlocked = true;
    }

    private void UnlockCloneOnDash()
    {
        if(dashCloneUnlockBtn.unlocked)
            cloneOnDashUnlocked = true;
    }

    private void UnlockArrivalOnDash()
    {
        if(dashArrivalUnlockBtn.unlocked)
            cloneOnArrivalUnlocked = true;
    }

        public void CloneOnDash()
    {
        if(cloneOnDashUnlocked)
            SkillManager.instance.clone.CreateClone(player.transform, Vector3.zero);

    }

    public void CloneOnArrival()
    {
        if(cloneOnArrivalUnlocked)
            SkillManager.instance.clone.CreateClone(player.transform, Vector3.zero);
    }

}

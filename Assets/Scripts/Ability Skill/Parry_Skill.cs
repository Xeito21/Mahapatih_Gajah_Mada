using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Parry_Skill : Skill
{

    [Header("Resolute parry")]
    [SerializeField] private UI_SlotSkillTree resoluteParryUnlockBtn;
    public bool resoluteParryUnlocked {get; private set;}

    [Header("Vital parry")]
    [SerializeField] private UI_SlotSkillTree vitalParryUnlockBtn;
    [Range(0f, 1f)]
    [SerializeField] private float restoreHealthPercent;
    public bool vitalParryUnlocked {get; private set;}

    [Header("Mirage Riposte")]
    [SerializeField] private UI_SlotSkillTree mirageRiposteUnlockBtn;
    public bool mirageRiposteUnlocked {get; private set;}


    public override void UseSkill()
    {
        base.UseSkill();

        if(resoluteParryUnlocked)
        {
            int restoreAmount = Mathf.RoundToInt(player.stats.GetMaxHealthValue() * restoreHealthPercent);
            player.stats.IncreaseHealthBy(restoreAmount);
        }

    }

    protected override void Start()
    {
        base.Start();
        resoluteParryUnlockBtn.GetComponent<Button>().onClick.AddListener(UnlockResoluteParry);
        vitalParryUnlockBtn.GetComponent<Button>().onClick.AddListener(UnlockVitalParry);
        mirageRiposteUnlockBtn.GetComponent<Button>().onClick.AddListener(UnlockMirageRiposte);

    }

    protected override void CheckUnlock()
    {
        UnlockResoluteParry();
        UnlockMirageRiposte();
        UnlockVitalParry();
    }

    private void UnlockResoluteParry()
    {
        if(resoluteParryUnlockBtn.unlocked)
            resoluteParryUnlocked = true;
    }

    private void UnlockVitalParry()
    {
        if(vitalParryUnlockBtn.unlocked)
            vitalParryUnlocked = true;
    }

    private void UnlockMirageRiposte()
    {
        if(mirageRiposteUnlockBtn.unlocked)
            mirageRiposteUnlocked = true;
    }

    public void MirageRiposteOnParry(Transform _respawnTransform)
    {
        if(mirageRiposteUnlocked)
            SkillManager.instance.clone.CreateCloneWithDelay(_respawnTransform);
    }
}

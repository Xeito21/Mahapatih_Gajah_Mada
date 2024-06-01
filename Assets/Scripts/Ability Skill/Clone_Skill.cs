using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clone_Skill : Skill
{
    [Header("Clone Info")]
    [SerializeField] private float attackMultiplier;
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [Space]

    [Header("clone Attack")]
    [SerializeField] UI_SlotSkillTree cloneAttackUnlockBtn;
    [SerializeField] private float cloneAttackMultiplier;
    [SerializeField] private bool canAttack;

    [Header("Aggresive Clone")]
    [SerializeField] private UI_SlotSkillTree aggresiveCloneUnlockBtn;
    [SerializeField] private float aggresiveCloneAttackMultiplier;
    public bool canApplyOnHitEffect {get; private set;}

    [Header("Clone Can Duplicate")]
    [SerializeField] private UI_SlotSkillTree multiplierUnlockBtn;
    [SerializeField] private float multiCloneAttackMultiplier;
    [SerializeField] private bool canDuplicateClone;
    [SerializeField] private float chanceToDuplicate;

    [Header("Crystal Instead Clone")]
    [SerializeField] private UI_SlotSkillTree crystalCloneBtn;
    public bool crystalInsteadOfClone;

    protected override void Start()
    {
        base.Start();
        cloneAttackUnlockBtn.GetComponent<Button>().onClick.AddListener(UnlockMirageAssault);
        aggresiveCloneUnlockBtn.GetComponent<Button>().onClick.AddListener(UnlockPhantamOnslaught);
        multiplierUnlockBtn.GetComponent<Button>().onClick.AddListener(UnlockIllusionary);
        crystalCloneBtn.GetComponent<Button>().onClick.AddListener(UnlockCrystalMirage);
    }

    protected override void CheckUnlock()
    {
        UnlockMirageAssault();
        UnlockPhantamOnslaught();
        UnlockIllusionary();
        UnlockCrystalMirage();
    }


    private void UnlockMirageAssault()
    {
        if(cloneAttackUnlockBtn.unlocked)
        {
            canAttack = true;
            attackMultiplier = cloneAttackMultiplier;
        }
    }

    private void UnlockPhantamOnslaught()
    {
        if(aggresiveCloneUnlockBtn.unlocked)
        {
            canApplyOnHitEffect = true;
            attackMultiplier = aggresiveCloneAttackMultiplier;
        }
    }

    private void UnlockIllusionary()
    {
        if(multiplierUnlockBtn.unlocked)
        {
            canDuplicateClone = true;
            attackMultiplier = multiCloneAttackMultiplier;
        }
    }

    private void UnlockCrystalMirage()
    {
        if(crystalCloneBtn.unlocked)
        {
            crystalInsteadOfClone = true;
        }
    }

    public void CreateClone(Transform _clonePosition,Vector3 _offset)
    {
        if(crystalInsteadOfClone)
        {
            SkillManager.instance.crystal.CreateCrystal();
            return;
        }
        GameObject newClone = Instantiate(clonePrefab);
        newClone.GetComponent<Clone_Skill_Controller>().
        SetupClone(_clonePosition,cloneDuration,canAttack, _offset, canDuplicateClone, chanceToDuplicate,player, attackMultiplier);
            
    }


    public void CreateCloneWithDelay(Transform _enemyTransform)
    {
        StartCoroutine(CreateCloneDelayCorotine(_enemyTransform,new Vector3(2 * player.facingDir, 0)));
    }

    private IEnumerator CreateCloneDelayCorotine(Transform _transform, Vector3 _offset)
    {
        yield return new WaitForSeconds(.4f);
        CreateClone(_transform, _offset);
    }
}

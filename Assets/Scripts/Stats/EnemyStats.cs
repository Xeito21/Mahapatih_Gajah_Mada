using System;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy enemy;
    private ItemDrop myDropSystem;
    public BaseStats gobogDropAmount;


    [Header("Difficult details")]
    [SerializeField] private int level = 1;
    [Range(0f,1f)]
    [SerializeField] private float percentageModifier = .4f;
    protected override void Start()
    {
        gobogDropAmount.SetDefaultValue(100);
        ApplyLevelModifier();
        base.Start();
        enemy = GetComponent<Enemy>();
        myDropSystem =  GetComponent<ItemDrop>();

    }

    private void ApplyLevelModifier()
    {
        Modify(ketangkasan);
        Modify(kelincahan);
        Modify(kecerdasan);
        Modify(energi);

        Modify(damage);
        Modify(critChance);
        Modify(critPower);

        Modify(maxHealth);
        Modify(pelindung);
        Modify(penghindar);
        Modify(magicResistance);

        Modify(fireDamage);
        Modify(iceDamage);
        Modify(lightingDamage);

        Modify(gobogDropAmount);
    }

    private void Modify(BaseStats _stat)
    {
        for(int i = 1; i < level; i++)
        {
            float modifier = _stat.GetValue() * percentageModifier;
            _stat.AddModifier(Mathf.RoundToInt(modifier));
        }
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
    }

    protected override void Die()
    {

        base.Die();
        enemy.Die();
        enemy.prajurtisDead = true;
        QuestIsActive();
        PlayerManager.instance.currency += gobogDropAmount.GetValue();
        myDropSystem.GenerateDrop();
        Destroy(gameObject, 3f);
        SaveManager.instance.SaveGame();
    }

    private void QuestIsActive()
    {
        if (quest.goal.goalType == GoalType.Kill)
            PlayerManager.instance.QuestOnProgress();
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    private GajahMada player;
    protected override void Start()
    {
        base.Start();
        player = GetComponent<GajahMada>();
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
    }

    protected override void Die()
    {
        base.Die();
        player.Die();
        GameManager.instance.lostCurrencyAmount = PlayerManager.instance.currency;
        PlayerManager.instance.currency = 0;
        GetComponent<PlayerItemDrop>()?.GenerateDrop();
    }

    protected override void DecreaseHealthBy(int _damage)
    {
        base.DecreaseHealthBy(_damage);

        AudioManager.instance.PlaySFX(17,null);
        if(_damage > GetMaxHealthValue() * .7f)
        {
            player.SetupKnockBackPower(new Vector2(10,6));
            player.fx.ScreenShake(player.fx.shakeHighDamage);
            GetComponent<GajahMada>().SetZeroVelocity();
            AudioManager.instance.PlaySFX(26,null);
        }


        ItemData_Equipment currentArmor = Bagspace.instance.GetEquipment(TipeEquiment.Pelindung);

        if(currentArmor != null)
            currentArmor.CallItemEffect(player.transform);
    }

    public override void OnEvasion()
    {
        player.skill.dodge.CreateCloneDodge();
    }

    public void CloneDoDamage(CharacterStats _targetStats, float _multiplier)
    {
        AudioManager.instance.PlaySFX(4,null);
        if (TargetCanVoidAttack(_targetStats))
            return;
            
            

        int totalDamage = damage.GetValue() + ketangkasan.GetValue();

        if(_multiplier > 0)
            totalDamage = Mathf.RoundToInt(totalDamage * _multiplier);

        if(CanCrit())
        {
            totalDamage = CalculateCritDamage(totalDamage);
        }
        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
        _targetStats.TakeDamage(totalDamage);
        DoMagicalDamage(_targetStats);
    }
}

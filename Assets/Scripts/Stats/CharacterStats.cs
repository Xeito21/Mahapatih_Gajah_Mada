
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
public enum tipeStat
{
    ketangkasan,
    kelincahan,
    kecerdasan,
    energi,
    damage,
    critChance,
    critPower,
    maxHealth,
    pelindung,
    penghindar,
    magicResistance,
    fireDamage,
    iceDamage,
    lightingDamage
}
public class CharacterStats : MonoBehaviour
{
    private EntityFX fx;
    public Quest quest;
    [Header("Major Stats")]
    public BaseStats ketangkasan;
    public BaseStats kelincahan;
    public BaseStats kecerdasan;
    public BaseStats energi;

    [Header("Offensive Stats")]
    public BaseStats damage;
    public BaseStats critChance;
    public BaseStats critPower;

    [Header("Defense Stats")]
    public BaseStats maxHealth;
    public BaseStats pelindung;
    public BaseStats penghindar;

    public BaseStats magicResistance;

    [Header("Magic Stats")]
    public BaseStats fireDamage;
    public BaseStats iceDamage;
    public BaseStats lightingDamage;
    public bool terbakar;
    public bool dingin;
    public bool tersengat;

    [SerializeField] private float elementDuration = 4;

    private float terbakarTimer;
    private float bekuTimer;
    private float tersengatTimer;
    private float terbakarDamageCooldown = .3f;
    private float terbakarDamageTimer;
    [SerializeField] private GameObject shockStrikePrefab;
    private int shockDamage;

    private int terbakarDamage;

    public int currentHealth;

    public System.Action onHealthChanged;
    public bool isDead {get; private set;}
    public bool isInvincible {get; private set;}
    public bool isShattered;
    

    protected virtual void Start()
    {
        critPower.SetDefaultValue(50);
        currentHealth = GetMaxHealthValue();

        fx = GetComponent<EntityFX>();
    }

    protected virtual void Update()
    {
        terbakarTimer -= Time.deltaTime;
        bekuTimer -= Time.deltaTime;
        tersengatTimer = Time.deltaTime;

        terbakarDamageTimer -= Time.deltaTime;

        if (terbakarTimer < 0)
            terbakar = false;
        if (bekuTimer < 0)
            dingin = false;
        if (tersengatTimer < 0)
            tersengat = false;

        if(terbakar)
            TerkenaTerbakar();
    }

    public void MakeShatteredFor(float _duration)
    {
        StartCoroutine(ShatteredHurlCoroutine(_duration));
    }

    private IEnumerator ShatteredHurlCoroutine(float _duration)
    {
        isShattered = true;
        yield return new WaitForSeconds(_duration);
        isShattered = false;

    }

    public virtual void IncreaseStatBy(int _modifier, float _duration, BaseStats _statToModify)
    {
        StartCoroutine(StatModDuration(_modifier, _duration, _statToModify));
    }

    private IEnumerator StatModDuration(int _modifier, float _duration, BaseStats _statToModify)
    {
        _statToModify.AddModifier(_modifier);

        yield return new WaitForSeconds(_duration);
        _statToModify.RemoveModifier(_modifier);
    }

    private void TerkenaTerbakar()
    {
        if (terbakarDamageTimer < 0)
        {

            DecreaseHealthBy(terbakarDamage);
            if (currentHealth < 0 && !isDead)
                Die();
            terbakarDamageTimer = terbakarDamageCooldown;
        }
    }

    public virtual void DoDamage(CharacterStats _targetStats)
    {
        bool ciriticalStrike = false;

        if (TargetCanVoidAttack(_targetStats))
            return;

        _targetStats.GetComponent<Entity>().SetupKnockBackDir(transform);



        int totalDamage = damage.GetValue() + ketangkasan.GetValue();

        if(CanCrit())
        {
            totalDamage = CalculateCritDamage(totalDamage);
            ciriticalStrike = true;
        }

        fx.CreateHitFX(_targetStats.transform,ciriticalStrike);

        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
        _targetStats.TakeDamage(totalDamage);
        DoMagicalDamage(_targetStats);
    }

    public virtual void DoMagicalDamage(CharacterStats _targetStats)
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightingDamage = lightingDamage.GetValue();
        int totalMagicalDamage = _fireDamage + _iceDamage + _lightingDamage + kecerdasan.GetValue();
        totalMagicalDamage = CheckResistance(_targetStats, totalMagicalDamage);
        _targetStats.TakeDamage(totalMagicalDamage);


        if (Mathf.Max(_fireDamage, _iceDamage, _lightingDamage) <= 0)
            return;
        AttemptyToApplyElements(_targetStats, _fireDamage, _iceDamage, _lightingDamage);

    }

    private void AttemptyToApplyElements(CharacterStats _targetStats, int _fireDamage, int _iceDamage, int _lightingDamage)
    {
        bool canApplyTerbakar = _fireDamage > _iceDamage && _fireDamage > _lightingDamage;
        bool canApplyDingin = _iceDamage > _fireDamage && _iceDamage > _lightingDamage;
        bool canApplyTersengat = _lightingDamage > _fireDamage && _lightingDamage > _iceDamage;


        while (!canApplyTerbakar && !canApplyDingin && !canApplyTersengat)
        {
            if (UnityEngine.Random.value < .3f && _fireDamage > 0)
            {
                canApplyTerbakar = true;
                _targetStats.ApplyAliments(canApplyTerbakar, canApplyDingin, canApplyTersengat);
                return;
            }

            if (UnityEngine.Random.value < .2f && _iceDamage > 0)
            {
                canApplyDingin = true;
                _targetStats.ApplyAliments(canApplyTerbakar, canApplyDingin, canApplyTersengat);
                return;
            }

            if (UnityEngine.Random.value < .5f && _lightingDamage > 0)
            {
                canApplyTersengat = true;
                _targetStats.ApplyAliments(canApplyTerbakar, canApplyDingin, canApplyTersengat);
                return;
            }
        }
        if (canApplyTerbakar)
            _targetStats.SetupTerbakarDamage(Mathf.RoundToInt(_fireDamage * .2f));

        if (canApplyTersengat)
            _targetStats.SetupTersengatDamage(Mathf.RoundToInt(_lightingDamage * .1f));

        _targetStats.ApplyAliments(canApplyTerbakar, canApplyDingin, canApplyTersengat);
    }

    private int CheckResistance(CharacterStats _targetStats, int totalMagicalDamage)
    {
        totalMagicalDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.kecerdasan.GetValue() * 3);
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
        return totalMagicalDamage;
    }

    public void SetupTerbakarDamage(int _damage) => terbakarDamage = _damage;

    public void SetupTersengatDamage(int _damage) => shockDamage = _damage;

    public void ApplyAliments(bool _terbakar, bool _dingin, bool _tersengat)
    {
        bool canApplyTerbakar = !terbakar && !dingin && !tersengat;
        bool canApplyDingin = !terbakar && !dingin && !tersengat;
        bool canApplyTersengat = !terbakar && !dingin;
        

        if(_terbakar && canApplyTerbakar)
        {
            terbakar = _terbakar;
            terbakarTimer = elementDuration;

            fx.TerbakarFxFor(elementDuration);
        }

        if(_dingin && canApplyDingin)
        {
            bekuTimer = elementDuration;
            dingin = _dingin;

            float slowPercentage = .2f;
            GetComponent<Entity>().SlowEntityBy(slowPercentage, elementDuration);
            fx.bekuColorFxFor(elementDuration);
        }

        if(_tersengat && canApplyTersengat)
        {
            if(!tersengat)
            {
                ApplyShock(_tersengat);
            }
            else
            {
                if (GetComponent<GajahMada>() != null)
                    return;
                TargetShockTerdekat();
            }
        }
    }

    public void ApplyShock(bool _tersengat)
    {
        if(tersengat)
            return;
        tersengatTimer = elementDuration;
        tersengat = _tersengat;
        fx.TersengatFxFor(elementDuration);
    }

    private void TargetShockTerdekat()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null && Vector2.Distance(transform.position, hit.transform.position) > 1)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }

            if (closestEnemy == null)
                closestEnemy = transform;
        }

        if (closestEnemy != null)
        {
            GameObject newShockStrike = Instantiate(shockStrikePrefab, transform.position, quaternion.identity);
            newShockStrike.GetComponent<Shock_Controller>().Setup(shockDamage, closestEnemy.GetComponent<CharacterStats>());
        }
    }

    public virtual void TakeDamage(int _damage)
    {
        if(isInvincible)
            return;
        
        DecreaseHealthBy(_damage);
        
        GetComponent<Entity>().DamageImpact();
        fx.StartCoroutine("FlashEffect");
        //Debug.Log(_damage);

        if(currentHealth < 0 && !isDead)
            Die();

        
    }

    public virtual void IncreaseHealthBy(int _amount)
    {
        currentHealth += _amount;

        if(currentHealth > GetMaxHealthValue())
            currentHealth = GetMaxHealthValue();

        if(onHealthChanged != null)
            onHealthChanged();
    }

    protected virtual void DecreaseHealthBy(int _damage)
    {
        if(isShattered)
            _damage = Mathf.RoundToInt(_damage * 1.1f);

        currentHealth -= _damage;

        if(_damage > 0)
            fx.CreatePopUpText(_damage.ToString());
        
        if(onHealthChanged != null)
            onHealthChanged();
    }

    protected virtual void Die()
    {
        isDead = true;
    }
    protected int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {
        if(_targetStats.dingin)
            totalDamage -= Mathf.RoundToInt(_targetStats.pelindung.GetValue() * .8f);
        else
            totalDamage -= _targetStats.pelindung.GetValue();
        totalDamage -= _targetStats.pelindung.GetValue();
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }



    public virtual void OnEvasion()
    {

    }
    protected bool TargetCanVoidAttack(CharacterStats _targetStats)
    {
        int totalPenghindar = _targetStats.penghindar.GetValue() + _targetStats.kelincahan.GetValue();
        if(tersengat)
            totalPenghindar += 20;
        if (UnityEngine.Random.Range(0, 100) < totalPenghindar)
        {
            _targetStats.OnEvasion();
            AudioManager.instance.PlaySFX(25,null);
            return true;

        }
        return false;
    }

    protected bool CanCrit()
    {
        int totalCritChance = critChance.GetValue() + kelincahan.GetValue();
        if(UnityEngine.Random.Range(0,100) <= totalCritChance)
        {
            return true;
        }

        return false;
    }

    protected int CalculateCritDamage(int _damage)
    {
        float totalCritPower = critPower.GetValue() + ketangkasan.GetValue() * .01f;
        float critDamage = _damage * totalCritPower;

        return Mathf.RoundToInt(critDamage);
    }

    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue() + energi.GetValue() * 5;
    }

    public void KillEntity()
    {
        if(!isDead)
            Die();
    }

    public void MakeInvincible(bool _invincible) => isInvincible = _invincible;

        public BaseStats GetStat(tipeStat _statType)
    {
        if(_statType == tipeStat.ketangkasan) return ketangkasan;
        else if(_statType == tipeStat.kelincahan) return kelincahan;
        else if(_statType == tipeStat.kecerdasan) return kecerdasan;
        else if(_statType == tipeStat.kelincahan) return kelincahan;
        else if(_statType == tipeStat.energi) return energi;
        else if(_statType == tipeStat.damage) return damage;
        else if(_statType == tipeStat.critChance) return critChance;
        else if(_statType == tipeStat.critPower) return critPower;
        else if(_statType == tipeStat.maxHealth) return maxHealth;
        else if(_statType == tipeStat.pelindung) return pelindung;
        else if(_statType == tipeStat.penghindar) return penghindar;
        else if(_statType == tipeStat.magicResistance) return magicResistance;
        else if(_statType == tipeStat.fireDamage) return fireDamage;
        else if(_statType == tipeStat.iceDamage) return iceDamage;
        else if(_statType == tipeStat.lightingDamage) return lightingDamage;

        return null;
    }
}

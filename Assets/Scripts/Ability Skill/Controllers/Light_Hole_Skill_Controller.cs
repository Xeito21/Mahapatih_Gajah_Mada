using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Light_Hole_Skill_Controller : MonoBehaviour
{
    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> KeyCodeList;
    private float maxSize;
    private float growSpeed;
    private float shrinkSpeed;
    private float lightholeTimer;
    private bool canGrow = true;
    private bool canShrink;

    private bool canCreateHotKeys = true;
    private bool cloneAttackRelease;
    private bool playerCanDiseappear = true;
    private int amountOfAttacks = 4;
    private float cloneAttackCooldown = .3f;
    private float cloneAttackTimer;

    private List<GameObject> createdHotKey = new List<GameObject>();
    private List<Transform> targets = new List<Transform>();

    public bool playerCanExitState {get; private set;}


    public void SetupLightHole(float _maxSize, float _growSpeed, float _shrinkSpeed, int _amountOfAttacks, float _cloneAttackCooldown, float _lightholeDuration)
    {
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        amountOfAttacks = _amountOfAttacks;
        cloneAttackCooldown = _cloneAttackCooldown;
        lightholeTimer = _lightholeDuration;

        if(SkillManager.instance.clone.crystalInsteadOfClone)
            playerCanDiseappear = false;
    }


    private void Update()
    {
        cloneAttackTimer -= Time.deltaTime;
        lightholeTimer -= Time.deltaTime;

        if(lightholeTimer < 0)
        {
            lightholeTimer = Mathf.Infinity;
            if(targets.Count > 0)
                ReleaseCloneAttack();
            else
                FinishLightHoleAbility();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ReleaseCloneAttack();
        }

        CloneAttackLogic();
        if (canGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }
        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);

            if (transform.localScale.x < 0)
                Destroy(gameObject);
        }
    }

    private void ReleaseCloneAttack()
    {
        if(targets.Count <= 0)
            return;
        DestroyHotKeys();
        cloneAttackRelease = true;
        canCreateHotKeys = false;
        if(playerCanDiseappear)
        {
            playerCanDiseappear = false;
            PlayerManager.instance.player.fx.MakeTransparent(true);
        }

    }

    private void CloneAttackLogic()
    {
        if (cloneAttackTimer < 0 && cloneAttackRelease && amountOfAttacks > 0)
        {
            cloneAttackTimer = cloneAttackCooldown;
            int randomIndex = UnityEngine.Random.Range(0, targets.Count);
            float xOffset;
            if (UnityEngine.Random.Range(0, 100) > 50)
                xOffset = 2;
            else
                xOffset = -2;
            if(SkillManager.instance.clone.crystalInsteadOfClone)
            {
                SkillManager.instance.crystal.CreateCrystal();
                SkillManager.instance.crystal.CurrentCrystalRandomTarget();
            }
            else
            {
                SkillManager.instance.clone.CreateClone(targets[randomIndex], new Vector3(xOffset, 0));
            }

            amountOfAttacks--;

            if (amountOfAttacks <= 0)
            {
                Invoke("FinishLightHoleAbility", 1f);
            }
        }
    }

    private void FinishLightHoleAbility()
    {
        DestroyHotKeys();
        playerCanExitState = true;
        canShrink = true;
        cloneAttackRelease = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(true);
            CreateHotKey(collision);
        }
    }

    private void DestroyHotKeys()
    {
        if (createdHotKey.Count <= 0)
            return;

        for (int i = 0; i < createdHotKey.Count; i++)
        {
            Destroy(createdHotKey[i]);
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
            collision.GetComponent<Enemy>().FreezeTime(false);
    }

    private void CreateHotKey(Collider2D collision)
    {
        if (KeyCodeList.Count <= 0)
        {
            Debug.LogWarning("Not enough hott key");
            return;
        }

        if (!canCreateHotKeys)
            return;

        GameObject newHotKey = Instantiate(hotKeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);
        createdHotKey.Add(newHotKey);
        KeyCode choosenKey = KeyCodeList[UnityEngine.Random.Range(0, KeyCodeList.Count)];
        KeyCodeList.Remove(choosenKey);
        Light_Hole_Hotkey_Controller newHotKeyScript = newHotKey.GetComponent<Light_Hole_Hotkey_Controller>();
        newHotKeyScript.SetupHotKey(choosenKey, collision.transform, this);
    }

    public void AddEnemyToList(Transform _enemyTransform) => targets.Add(_enemyTransform);
}

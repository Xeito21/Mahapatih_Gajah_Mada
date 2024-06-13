using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class UI_SlotSkillTree : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISaveManager
{

    private User_Interfaces ui;
    [SerializeField] private int skillCost;
    [SerializeField] private string namaSkill;
    [TextArea]
    [SerializeField] private string descSkill;
    [SerializeField] private Color lockedSkillColor;
    public bool unlocked;
    [SerializeField] private UI_SlotSkillTree[] shouldBeUnlocked;
    [SerializeField] private UI_SlotSkillTree[] shouldBeLocked;

    private Image gambarSkill;

    private void OnValidate()
    {
        gameObject.name = "SkillTreeSlot_UI - " + namaSkill;
    }

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => SlotSkillTerbuka());
    }


    private void Start()
    {
        gambarSkill = GetComponent<Image>();
        ui = GetComponentInParent<User_Interfaces>();

        gambarSkill.color = lockedSkillColor;
        if (unlocked)
            gambarSkill.color = Color.white;
    }

    public void SlotSkillTerbuka()
    {
        if(PlayerManager.instance.HaveEnoughGobog(skillCost) == false)
        {
            AudioManager.instance.PlaySFX(35,null);
            User_Interfaces.instance.StartCoroutine(User_Interfaces.instance.DisplayPopupText("Gobog anda tidak cukup!"));
            return;
        }
        for (int i = 0; i < shouldBeUnlocked.Length; i++)
        {
            
            if(shouldBeUnlocked[i].unlocked == false)
            {
                AudioManager.instance.PlaySFX(35,null);
                User_Interfaces.instance.StartCoroutine(User_Interfaces.instance.DisplayPopupText("Anda harus membuka skill sebelumnya!"));
                return;
            }
        }

        for (int i = 0; i < shouldBeLocked.Length; i++)
        {
            if(shouldBeLocked[i].unlocked == true)
            {
                AudioManager.instance.PlaySFX(35,null);
                User_Interfaces.instance.StartCoroutine(User_Interfaces.instance.DisplayPopupText("Anda harus membuka skill sebelumnya!"));
                return;
            }
        }

        AudioManager.instance.PlaySFX(37,null);
        unlocked = true;
        gambarSkill.color = Color.white;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        CursorUI.Instance.SetCursorPoint();
        ui.skillToolTip.ShowToolTip(descSkill, namaSkill, skillCost);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CursorUI.Instance.SetCustomCursor();
        ui.skillToolTip.HideToolTip();
    }

    public void LoadData(GameData _data)
    {
        if(_data.skillTree.TryGetValue(namaSkill, out bool value))
        {
            unlocked = value;
        }
    }

    public void SaveData(ref GameData _data)
    {
        if(_data.skillTree.TryGetValue(namaSkill, out bool value))
        {
            _data.skillTree.Remove(namaSkill);
            _data.skillTree.Add(namaSkill, unlocked);
        }
        else
            _data.skillTree.Add(namaSkill, unlocked);
    }
}


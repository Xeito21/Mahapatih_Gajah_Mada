using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD_UI : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Slider slider;
    [SerializeField] private Image dashImage;
    [SerializeField] private Image parryImage;
    [SerializeField] private Image crystalImage;
    [SerializeField] private Image swordImage;
    [SerializeField] private Image vortexImage;
    [SerializeField] private Image potionImage;

    [Header("Gobog Info")]
    [SerializeField] private TextMeshProUGUI currentGobog;
    [SerializeField] private TextMeshProUGUI currentKeys;
    public float keysAmount { get; private set; }
    [SerializeField] private float gobogAmount;
    [SerializeField] private float increaseRate = 100;

    private SkillManager skills;


    // Start is called before the first frame update
    void Start()
    {
        if(playerStats != null)
            playerStats.onHealthChanged += UpdateHealthUI;

            skills = SkillManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        GobogUpdateUI();
        if (Input.GetKeyDown(KeyCode.LeftShift) && skills.dash.dashUnlocked)
            SetCoolDownOf(dashImage);

        if (Input.GetMouseButtonDown(1) && skills.parry.vitalParryUnlocked)
            SetCoolDownOf(parryImage);

        if (Input.GetKeyDown(KeyCode.F) && skills.crystal.crystalUnlocked)
            SetCoolDownOf(crystalImage);

        if (Input.GetKeyDown(KeyCode.Q) && skills.sword.swordUnlocked)
            SetCoolDownOf(swordImage);

        if (Input.GetKeyDown(KeyCode.R) && skills.lighthole.vortexUnlocked)
            SetCoolDownOf(vortexImage);

        if (Input.GetKeyDown(KeyCode.Alpha1) && Bagspace.instance.GetEquipment(TipeEquiment.Ramuan) != null)
            SetCoolDownOf(potionImage);

        CheckCooldownOf(dashImage, skills.dash.coolDown);
        CheckCooldownOf(parryImage, skills.parry.coolDown);
        CheckCooldownOf(crystalImage, skills.crystal.coolDown);
        CheckCooldownOf(swordImage, skills.sword.coolDown);
        CheckCooldownOf(vortexImage, skills.lighthole.coolDown);
        CheckCooldownOf(potionImage, Bagspace.instance.ramuanCooldown);
    }

    private void GobogUpdateUI()
    {
        if (gobogAmount < PlayerManager.instance.GetCurrency())
        {
            gobogAmount += Time.deltaTime * increaseRate;
            AudioManager.instance.PlaySFX(18,null);
        }
        else
            gobogAmount = PlayerManager.instance.GetCurrency();

        if(keysAmount < PlayerManager.instance.GetKeysCurrency())
        {
            keysAmount += Time.deltaTime * increaseRate;
            AudioManager.instance.PlaySFX(34, null);
        }
        else
            keysAmount = PlayerManager.instance.GetKeysCurrency();

        currentGobog.text = ((int)gobogAmount).ToString();
        currentKeys.text = ((int)keysAmount).ToString();
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = playerStats.GetMaxHealthValue();
        slider.value = playerStats.currentHealth;
    }

    private void SetCoolDownOf(Image _image)
    {
        if(_image.fillAmount <= 0)
            _image.fillAmount = 1;
    }

    private void CheckCooldownOf(Image _image, float _coolDown)
    {
        if(_image.fillAmount > 0)
            _image.fillAmount -= 1 / _coolDown * Time.deltaTime;
    }
}

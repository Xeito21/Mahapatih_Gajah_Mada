using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, ISaveManager
{
    public static PlayerManager instance;
    public GajahMada player;
    public Quest quest;
    public int currency;
    public int keysCurrency;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    public bool HaveEnoughGobog(int _price)
    {
        if(_price > currency)
        {
            return false;
        }
        currency = currency - _price;

        return true;
    }
    public void QuestOnProgress()
    {
        if (quest.isActive)
        {
            if (quest.goal.goalType == GoalType.Kill || quest.goal.goalType == GoalType.Gathering)
            {
                quest.goal.QuestType();

                if (quest.goal.isReached())
                {
                    User_Interfaces.instance.StartCoroutine(User_Interfaces.instance.DisplayPopupText(quest.title + " Telah Selesai"));
                    QuestComplete();
                }
            }
        }
    }


    private void QuestComplete()
    {
        if (quest.goal.isReached())
        {
            currency += quest.gobogReward;
            keysCurrency += quest.keysReward;
            quest.Complete();
        }
    }

    public int GetCurrency() => currency;

    public int GetKeysCurrency() => keysCurrency;

    public void LoadData(GameData _data)
    {
        this.currency = _data.currency;
        this.keysCurrency = _data.keysCurrency;
    }

    public void SaveData(ref GameData _data)
    {
        _data.currency = this.currency;
        _data.keysCurrency = this.keysCurrency;
    }
}

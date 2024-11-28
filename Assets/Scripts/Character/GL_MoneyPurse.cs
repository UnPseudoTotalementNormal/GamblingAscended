using System;
using GameEvents;
using GameEvents.Enum;
using UnityEngine;

public class GL_MoneyPurse : MonoBehaviour, GL_ICoinHolder
{
    public float MoneyInserted { get; private set; }
    [field:SerializeField] public float BaseMoney { get; private set; }
    public bool DropPurseOnDeath = false;
    [SerializeField] private GameObject _pursePrefab;

    private void Awake()
    {
        GameEventEnum.OnDeath.AddListener(OnDeath);
        GameEventEnum.PickupPurse.AddListener(PickUpPurse);
        GameEventEnum.SetEnemyInfo.AddListener(SetEnemyInfo);
        AddMoney(BaseMoney);
    }

    private void SetEnemyInfo(GameEventInfo eventInfo)
    {
        if (!gameObject.HasGameID(eventInfo.Ids) || !eventInfo.TryTo(out GameEventEnemyInfo gameEventEnemyInfo))
        {
            return;
        }
        
        MoneyInserted = gameEventEnemyInfo.EnemyInfo.MoneyOnDeath;
    }


    public void AddMoney(float value)
    {
        MoneyInserted += value;
        MoneyInserted = Math.Max(0, MoneyInserted);
    }

    public void RemoveMoney(float value)
    {
        MoneyInserted -= value;
        MoneyInserted = Math.Max(0, MoneyInserted);
    }

    private void OnDeath(GameEventInfo eventInfo)
    {
        if (!gameObject.HasGameID(eventInfo.Ids) || !DropPurseOnDeath)
        {
            return;
        }
        
        Instantiate(_pursePrefab, transform.position, Quaternion.identity).GetComponent<GL_ICoinHolder>().AddMoney(MoneyInserted);
    }
    
    private void PickUpPurse(GameEventInfo eventInfo)
    {
        if (!gameObject.HasGameID(eventInfo.Ids) || !eventInfo.TryTo(out GameEventGameObject gameEventGameObject))
        {
            return;
        }
        
        gameEventGameObject.Value.GetComponentInParent<GL_ICoinHolder>().AddMoney(MoneyInserted);
        RemoveMoney(MoneyInserted);
        Destroy(gameObject);
    }
}

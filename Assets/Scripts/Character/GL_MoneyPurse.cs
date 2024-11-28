using System;
using GameEvents;
using GameEvents.Enum;
using UnityEngine;

public class GL_MoneyPurse : MonoBehaviour, GL_ICoinHolder
{
    public float MoneyInserted { get; private set; }
    public bool DropPurseOnDeath = false;
    [SerializeField] private GameObject _pursePrefab;

    private void Awake()
    {
        GameEventEnum.OnDeath.AddListener(OnDeath);
        AddMoney(15);
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
}

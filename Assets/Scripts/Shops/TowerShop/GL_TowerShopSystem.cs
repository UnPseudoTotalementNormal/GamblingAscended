using System;
using System.Collections.Generic;
using Extensions;
using GameEvents;
using GameEvents.Enum;
using Interactables.ObjectHolding_Placing;
using TMPro;
using Towers;
using UnityEngine;

public class GL_TowerShopSystem : MonoBehaviour
{
    [SerializeField] private GL_TowerPlaceable _towerBoxPrefab;
    
    [SerializeField] private List<GL_TowerInfo> _buyableTowers = new();
    private GL_TowerInfo _selectedTower;
    
    [SerializeField] private float _shippingZoneSize = 3;
    
    [SerializeField] private Transform _towerDisplayParent;
    [SerializeField] private Transform _towerBoxSpawnPoint;

    [Header("Texts")] 
    [SerializeField] private TextMeshPro _towerCostText;
    [SerializeField] private TextMeshPro _towerDPSText;
    [SerializeField] private TextMeshPro _towerDamageTypeText;
    [SerializeField] private TextMeshPro _towerRangeText;
    [SerializeField] private TextMeshPro _towerTargetTypeText;
    [SerializeField] private TextMeshPro _towerNameText;
    
    [SerializeField] private string _costBaseText = "Prix: ";
    [SerializeField] private string _dpsBaseText = "DPS: ";
    [SerializeField] private string _damageTypeBaseText = "Type: ";
    [SerializeField] private string _rangeBaseText = "Rayon: ";
    [SerializeField] private string _targetTypeBaseText = "Ciblage: ";

    private void Awake()
    {
        GameEventEnum.NextObj.AddListener(TryNextObj);
        GameEventEnum.PreviousObj.AddListener(TryPreviousObj);
        GameEventEnum.TryBuyObj.AddListener(TryBuyObj);
        
        _selectedTower = _buyableTowers[0];
        DisplayTower(_selectedTower);
    }

    private void TryBuyObj(GameEventInfo eventInfo)
    {
        if (!gameObject.HasGameID(eventInfo.Ids) || !eventInfo.TryTo(out GameEventGameObject gameEventGameObject))
        {
            return;
        }
        
        gameEventGameObject.Value.TryGetComponentInParents(out GL_ICoinHolder coinHolder);
        
        if (coinHolder == null || coinHolder.MoneyInserted < _selectedTower.Cost)
        {
            return;
        }
        
        coinHolder.RemoveMoney(_selectedTower.Cost);

        
        //try to add amount to an existing tower box
        Collider[] shipZoneOverlap = Physics.OverlapBox(
            _towerBoxSpawnPoint.position,
            Vector3.one * _shippingZoneSize / 2, 
            Quaternion.identity);
        foreach (Collider colliderOverlap in shipZoneOverlap)
        {
            if (!colliderOverlap.transform.TryGetComponentInParents<GL_TowerPlaceable>(out GL_TowerPlaceable towerBoxInZone))
            {
                continue;
            }
            
            if (towerBoxInZone.TowerInfo != _selectedTower)
            {
                continue;
            }
            
            towerBoxInZone.Amount += 1;
            return;
        }
        
        //create a new tower box
        GL_TowerPlaceable newTower = Instantiate(_towerBoxPrefab, _towerBoxSpawnPoint.position, Quaternion.identity);
        newTower.TowerInfo = _selectedTower;
    }

    private void TryPreviousObj(GameEventInfo eventInfo)
    {
        if (!gameObject.HasGameID(eventInfo.Ids))
        {
            return;
        }
        
        int index = _buyableTowers.IndexOf(_selectedTower);
        index = Mathf.Clamp(index - 1, 0, _buyableTowers.Count - 1);
        SelectTower(_buyableTowers[index]);
    }

    private void TryNextObj(GameEventInfo eventInfo)
    {
        if (!gameObject.HasGameID(eventInfo.Ids))
        {
            return;
        }
        
        int index = _buyableTowers.IndexOf(_selectedTower);
        index = Mathf.Clamp(index + 1, 0, _buyableTowers.Count - 1);
        SelectTower(_buyableTowers[index]);
    }

    private void SelectTower(GL_TowerInfo selectTower)
    {
        _selectedTower = selectTower;
        DisplayTower(_selectedTower);
    }

    private void ResetDisplay()
    {
        foreach (Transform child in _towerDisplayParent)
        {
            Destroy(child.gameObject);
        }
    }
    
    private void DisplayTower(GL_TowerInfo showTower = null)
    {
        ResetDisplay();
        if (showTower == null)
        {
            showTower = _selectedTower;
        }

        Transform spawnedModel = Instantiate(showTower.TowerModel, _towerDisplayParent).transform;
        spawnedModel.localPosition = new Vector3(0, 0, 0);
        spawnedModel.localScale = Vector3.one * 0.75f;
        
        _towerCostText.text = _costBaseText + showTower.Cost;
        _towerDPSText.text = _dpsBaseText + (showTower.AttackDamage / showTower.AttackCooldown).ToString("0.00");
        _towerDamageTypeText.text = _damageTypeBaseText + showTower.DamageType;
        _towerRangeText.text = _rangeBaseText + showTower.AttackRadius;
        _towerTargetTypeText.text = _targetTypeBaseText + showTower.AttackType;
        _towerNameText.text = showTower.TowerName;
    }
}

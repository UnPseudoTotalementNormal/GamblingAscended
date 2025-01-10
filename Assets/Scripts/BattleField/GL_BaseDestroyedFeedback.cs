using System;
using GameEvents;
using GameEvents.Enum;
using TMPro;
using UnityEngine;

public class GL_BaseDestroyedFeedback : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _defeatText;

    [SerializeField] private string _prefix = "Perdu !\nVous avez résisté ";
    [SerializeField] private string _suffix = "jours";

    private int _dayCount = 0;
    
    private void Awake()
    {
        GameEventEnum.BaseDestroyed.AddListener(OnBaseDestroyed);
        GameEventEnum.OnWaveEnded.AddListener(UpdateWaveCounter);
    }

    private void UpdateWaveCounter(GameEventInfo obj)
    {
        if (!obj.TryTo(out GameEventInt gameEventInt))
        {
            return;
        }
        
        _dayCount = gameEventInt.Value + 1;
    }

    private void OnBaseDestroyed(GameEventInfo eventInfo)
    {
        _defeatText.gameObject.SetActive(true);
        _defeatText.text = $"{_prefix}{_dayCount} {_suffix}";
    }
}

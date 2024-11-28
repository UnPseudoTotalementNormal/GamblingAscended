using System;
using AYellowpaper.SerializedCollections;
using GameEvents;
using GameEvents.Enum;
using TMPro;
using UnityEngine;

public class GL_WaveCounterPanel : MonoBehaviour
{
    [SerializeField] private TextMeshPro _text;

    [SerializeField] private string _prefix;
    [SerializeField] private string _sufix;

    [SerializeField] private SerializedDictionary<int, string> _specialDaySentences;

    private void Awake()
    {
        GameEventEnum.OnWaveEnded.AddListener(UpdateWaveCounter);
        GameEventEnum.OnWaveStarted.AddListener(UpdateWaveCounter);
    }

    private void UpdateWaveCounter(GameEventInfo eventInfo)
    {
        if (!eventInfo.TryTo(out GameEventInt gameEventInt))
        {
            return;
        }

        int actualDay = gameEventInt.Value + 1;

        if (_specialDaySentences.TryGetValue(actualDay, out string specialSentence))
        {
            _text.text = specialSentence;
            return;
        }
        _text.text = $"{_prefix}{actualDay}{_sufix}";
    }
}

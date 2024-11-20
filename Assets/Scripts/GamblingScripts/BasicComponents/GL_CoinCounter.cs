using System;
using GameEvents;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class GL_CoinCounter : MonoBehaviour
{
    [FormerlySerializedAs("_text")] [SerializeField] private TextMeshPro _textMesh;
    [SerializeField] private TextMeshProUGUI _textUI;
    [SerializeField] private GameObject _coinHolder;
    private GL_ICoinHolder _coinHolderScript;

    [SerializeField] private string _prefix;
    private void Awake()
    {
        if (_coinHolder)
        {
            SetCoinHolder(_coinHolder);
        }
    }

    private void Update()
    {
        if (_coinHolderScript == null)
        {
            return;
        }
        
        if (_textMesh) _textMesh.text = _prefix + _coinHolderScript.MoneyInserted;
        if (_textUI) _textUI.text = _prefix + _coinHolderScript.MoneyInserted;
    }

    public void SetCoinHolder(GameObject coinHolderObject)
    {
        _coinHolder = coinHolderObject;
        _coinHolder.TryGetComponent(out _coinHolderScript);
    }
}

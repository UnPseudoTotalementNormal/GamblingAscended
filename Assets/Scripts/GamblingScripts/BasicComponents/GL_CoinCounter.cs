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
        _coinHolderScript = _coinHolder.GetComponent<GL_ICoinHolder>();
    }

    private void Update()
    {
        if (_textMesh) _textMesh.text = _prefix + _coinHolderScript.MoneyInserted;
        if (_textUI) _textUI.text = _prefix + _coinHolderScript.MoneyInserted;
    }
}

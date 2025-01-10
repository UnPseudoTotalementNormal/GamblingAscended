using System;
using TMPro;
using UnityEngine;

public class GL_BaseHealthCounter : MonoBehaviour
{
    [SerializeField] private GL_Health _baseHealth;
    [SerializeField] private TextMeshPro _text;
    [SerializeField] private string _prefix = "Base health: ";
    private void Start()
    {
        _baseHealth.OnDamaged += OnBaseDamaged;
        OnBaseDamaged(_baseHealth);
    }

    private void OnBaseDamaged(GL_Health obj)
    {
        _text.text = _prefix + obj.CurrentHealth / obj.MaxHealth * 100 + "%";
    }
}

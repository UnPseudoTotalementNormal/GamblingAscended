using System;
using GameEvents;
using GameEvents.Enum;
using UnityEngine;

public class GL_LightDaySensor : MonoBehaviour
{
    [SerializeField] private Light _light;
    [SerializeField] private GameObject _lightMesh;
    
    private void Awake()
    {
        GameEventEnum.OnWaveEnded.AddListener((info) => { TurnOnLight(); });
        GameEventEnum.OnWaveStarted.AddListener((info) => { TurnOffLight(); });
    }
    
    private void TurnOnLight()
    {
        Component[] components = GetComponents<Behaviour>();
        foreach (Behaviour component in components)
        {
            component.enabled = true;
            _light.enabled = true;
            _lightMesh.SetActive(true);
        }
    }
    
    private void TurnOffLight()
    {
        Component[] components = GetComponents<Behaviour>();
        foreach (Behaviour component in components)
        {
            component.enabled = false;
            _light.enabled = false;
            _lightMesh.SetActive(false);
        }
    }
}

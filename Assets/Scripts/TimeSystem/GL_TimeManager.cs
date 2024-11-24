using System;
using Unity.Mathematics;
using UnityEngine;

[ExecuteAlways]
public class GL_TimeManager : MonoBehaviour
{
    [SerializeField] private Light _directionalLight;
    [SerializeField] private GL_LightingPreset _lightingPreset;
    
    [SerializeField, Range(0f, 24f)] private float _timeOfDay;

    private void Update()
    {
        if (!_lightingPreset)
        {
            return;
        }

        //_timeOfDay += Time.deltaTime;
        //_timeOfDay %= 24;
        
        UpdateLighting(_timeOfDay/24f);
    }

    private void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = _lightingPreset.AmbiantColor.Evaluate(timePercent);
        RenderSettings.fogColor = _lightingPreset.FogColor.Evaluate(timePercent);

        if (_directionalLight)
        {
            _directionalLight.color = _lightingPreset.DirectionalColor.Evaluate(timePercent);
            _directionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
        }
    }

    private void OnValidate()
    {
        if (_directionalLight)
        {
            return;
        }

        if (RenderSettings.sun != null)
        {
            _directionalLight = RenderSettings.sun;
        }
        else
        {
            _directionalLight = gameObject.GetComponent<Light>();
        }
    }
}

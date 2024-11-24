using DG.Tweening;
using GameEvents;
using UnityEngine;

public class GL_TimeManager : MonoBehaviour
{
    [SerializeField] private Light _directionalLight;
    [SerializeField] private GL_LightingPreset _lightingPreset;
    
    [SerializeField, Range(0f, 24f)] private float _timeOfDay;

    [SerializeField] private GameEvent<GameEventInfo> _switchTimeOfDay;

    private const float DAY_TIME = 12;
    private const float NIGHT_TIME = 24;
    
    private enum StateOfDay
    {
        Day,
        Night,
    }

    private void Awake()
    {
        _switchTimeOfDay?.AddListener(SwitchTimeOfDay);
    }

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

    private void SwitchTimeOfDay(GameEventInfo eventInfo)
    {
        switch (GetStateOfDay())
        {
            case StateOfDay.Day:
                DOTween.To(() => _timeOfDay, x => _timeOfDay = x, NIGHT_TIME, 3)
                    .SetEase(Ease.Linear);
                break;
            case StateOfDay.Night:
                _timeOfDay = 0;
                DOTween.To(() => _timeOfDay, x => _timeOfDay = x, DAY_TIME + 2, 3)
                    .SetEase(Ease.Linear);
                break;
        }
    }

    private StateOfDay GetStateOfDay()
    {
        if (_timeOfDay is < DAY_TIME + DAY_TIME/2f and > DAY_TIME - DAY_TIME/2f)
        {
            return StateOfDay.Day;
        }
        
        return StateOfDay.Night;
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
        UpdateLighting(_timeOfDay/24f);
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

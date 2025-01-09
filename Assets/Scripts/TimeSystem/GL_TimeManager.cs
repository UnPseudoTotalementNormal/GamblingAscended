using System.Collections.Generic;
using DG.Tweening;
using GameEvents;
using GameEvents.Enum;
using UnityEngine;
using UnityEngine.Serialization;

public class GL_TimeManager : MonoBehaviour
{
    [SerializeField] private Light _dayDirectionalLight;
    [SerializeField] private Light _nightDirectionalLight;
    [SerializeField] private GL_LightingPreset _lightingPreset;
    private Dictionary<Light, float> _lightIntensity = new();
    
    [SerializeField, Range(0f, 24f)] private float _timeOfDay;

    [SerializeField] private GameEvent<GameEventInfo> _switchTimeOfDay;

    public enum StateOfDay
    {
        Day,
        Night,
    }

    public StateOfDay CurrentStateOfDay { get; private set; }

    private const float DAY_TIME = 14;
    private const float NIGHT_TIME = 24;
    
    private void Awake()
    {
        _lightIntensity.Add(_dayDirectionalLight, _dayDirectionalLight.intensity);
        _lightIntensity.Add(_nightDirectionalLight, _nightDirectionalLight.intensity);
        GameEventEnum.OnWaveStarted.AddListener(TimeSetDay);
        GameEventEnum.OnWaveEnded.AddListener(TimeSetNight);
        GameEventEnum.TrySleep.AddListener(TrySleep);
    }

    private void TrySleep(GameEventInfo eventInfo)
    {
        if (CurrentStateOfDay != StateOfDay.Night)
        {
            return;
        }
        
        TimeSetDay(new GameEventInfo());
        GameEventEnum.OnSleep.Invoke(new GameEventInfo());
    }

    private void TimeSetNight(GameEventInfo eventInfo)
    {
        var duration = 3;
        _dayDirectionalLight.DOIntensity(0, duration);
        _nightDirectionalLight.DOIntensity(_lightIntensity[_nightDirectionalLight], duration);
        
        CurrentStateOfDay = StateOfDay.Night;
        DOTween.To(() => _timeOfDay, x => _timeOfDay = x, NIGHT_TIME, duration)
            .SetEase(Ease.Linear);
        GameEventEnum.OnDayEnded.Invoke(new GameEventInfo());
    }

    private void TimeSetDay(GameEventInfo eventInfo)
    {
        var duration = 3;
        _nightDirectionalLight.DOIntensity(0, duration);
        _dayDirectionalLight.DOIntensity(_lightIntensity[_dayDirectionalLight], duration);
        
        CurrentStateOfDay = StateOfDay.Day;
        DOTween.To(() => _timeOfDay, x => _timeOfDay = x, DAY_TIME, duration)
            .SetEase(Ease.Linear);
        GameEventEnum.OnNightEnded.Invoke(new GameEventInfo());
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
    
    private void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = _lightingPreset.AmbiantColor.Evaluate(timePercent);
        RenderSettings.fogColor = _lightingPreset.FogColor.Evaluate(timePercent);

        if (_dayDirectionalLight)
        {
            _dayDirectionalLight.color = _lightingPreset.DirectionalColor.Evaluate(timePercent);
            _dayDirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
        }
    }

    private void OnValidate()
    {
        UpdateLighting(_timeOfDay/24f);
    }
}

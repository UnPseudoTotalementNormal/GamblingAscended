using DG.Tweening;
using GameEvents;
using GameEvents.Enum;
using UnityEngine;

public class GL_TimeManager : MonoBehaviour
{
    [SerializeField] private Light _directionalLight;
    [SerializeField] private GL_LightingPreset _lightingPreset;
    
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
        CurrentStateOfDay = StateOfDay.Night;
        DOTween.To(() => _timeOfDay, x => _timeOfDay = x, NIGHT_TIME, 10)
            .SetEase(Ease.Linear);
        GameEventEnum.OnDayEnded.Invoke(new GameEventInfo());
    }

    private void TimeSetDay(GameEventInfo eventInfo)
    {
        CurrentStateOfDay = StateOfDay.Day;
        DOTween.To(() => _timeOfDay, x => _timeOfDay = x, DAY_TIME, 10)
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

        if (_directionalLight)
        {
            _directionalLight.color = _lightingPreset.DirectionalColor.Evaluate(timePercent);
            _directionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
        }
    }

    private void OnValidate()
    {
        UpdateLighting(_timeOfDay/24f);
    }
}

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

    private const float DAY_TIME = 14;
    private const float NIGHT_TIME = 24;
    
    private void Awake()
    {
        GameEventEnum.OnWaveStarted.AddListener(TimeSetDay);
        GameEventEnum.OnWaveEnded.AddListener(TimeSetNight);
    }

    private void TimeSetNight(GameEventInfo eventInfo)
    {
        DOTween.To(() => _timeOfDay, x => _timeOfDay = x, NIGHT_TIME, 20)
            .SetEase(Ease.Linear);
    }

    private void TimeSetDay(GameEventInfo eventInfo)
    {
        DOTween.To(() => _timeOfDay, x => _timeOfDay = x, DAY_TIME, 20)
            .SetEase(Ease.Linear);
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

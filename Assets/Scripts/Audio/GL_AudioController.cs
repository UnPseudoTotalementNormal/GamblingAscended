using System;
using DG.Tweening;
using GameEvents;
using GameEvents.Enum;
using UnityEngine;

public class GL_AudioController : MonoBehaviour
{
    private AudioSource _audioSource;
    private float _audioVolume;

    [SerializeField] private SwitchStateType _switchStateType;
    
    [SerializeField] private float _fadeTime = 1f;
    
    public enum SwitchStateType
    {
        Fade,
        Cut,
    }
    
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioVolume = _audioSource.volume;
        GameEventEnum.TurnOn.AddListener(TurnOn);
        GameEventEnum.TurnOff.AddListener(TurnOff);
    }
    
    private void TurnOn(GameEventInfo eventInfo)
    {
        if (!gameObject.HasGameID(eventInfo.Ids))
        {
            return;
        }

        ResetAudio();
        switch (_switchStateType)
        {
            case SwitchStateType.Fade:
                _audioSource.DOFade(_audioVolume, _fadeTime);
                break;
            case SwitchStateType.Cut:
                _audioSource.volume = _audioVolume;
                break;
        }
    }
    
    private void TurnOff(GameEventInfo eventInfo)
    {
        if (!gameObject.HasGameID(eventInfo.Ids))
        {
            return;
        }
        
        switch (_switchStateType)
        {
            case SwitchStateType.Fade:
                _audioSource.DOFade(0, _fadeTime);
                break;
            case SwitchStateType.Cut:
                _audioSource.volume = 0;
                break;
        }
    }

    private void ResetAudio()
    {
        _audioSource.Stop();
        _audioSource.Play();
    }
}

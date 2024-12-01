using System;
using Audio;
using AYellowpaper.SerializedCollections;
using GameEvents;
using GameEvents.Enum;
using UnityEngine;

public class GL_GameEventAudioReactor : MonoBehaviour
{
    [SerializeField] private SerializedDictionary<GameEventEnum, SerializableAudioInfo> _audioReactions = new();

    private void Awake()
    {
        foreach (GameEventEnum gameEventEnum in _audioReactions.Keys)
        {
            gameEventEnum.AddListener((eventInfo) =>
            {
                ReactToEvent(eventInfo, _audioReactions[gameEventEnum]);
            });
        }
    }

    private void ReactToEvent(GameEventInfo eventInfo, SerializableAudioInfo audioInfo)
    {
        if (!gameObject.HasGameID(eventInfo.Ids))
        {
            return;
        }

        audioInfo.Clip = audioInfo.ClipEnum.PickRandom();
        AudioSource audioSource = GL_AudioPlayer.PlayAudio(audioInfo);
        audioSource.Play();
    }
}

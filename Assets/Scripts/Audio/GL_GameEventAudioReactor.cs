using System;
using System.Collections.Generic;
using Audio;
using AYellowpaper.SerializedCollections;
using GameEvents;
using GameEvents.Enum;
using UnityEngine;

public class GL_GameEventAudioReactor : MonoBehaviour
{
    [SerializeField] private SerializedDictionary<GameEventEnum, List<SerializableAudioInfo>> _audioReactions = new();

    private void Awake()
    {
        foreach (GameEventEnum gameEventEnum in _audioReactions.Keys)
        {
            gameEventEnum.AddListener((eventInfo) =>
            {
                foreach (SerializableAudioInfo audio in _audioReactions[gameEventEnum])
                {
                    ReactToEvent(eventInfo, audio);
                }
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

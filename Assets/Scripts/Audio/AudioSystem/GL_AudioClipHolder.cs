using System.Collections.Generic;
using Audio;
using AYellowpaper.SerializedCollections;
using UnityEngine;

public class GL_AudioClipHolder : MonoBehaviour
{
    private static GL_AudioClipHolder Instance;
    public SerializedDictionary<AudioClipEnum, List<AudioClip>> AudioClips = new();

    public static List<AudioClip> GetAudioClips(AudioClipEnum audioCLipEnum)
    {
        return GetAudioClipHolder().AudioClips[audioCLipEnum];
    }

    private static GL_AudioClipHolder GetAudioClipHolder()
    {
        if (Instance)
        {
            return Instance;
        }

        return Instance = FindFirstObjectByType<GL_AudioClipHolder>();
    }
        
    public void SetAudioClips(Dictionary<AudioClipEnum, List<AudioClip>> newAudioClips)
    {
        AudioClips.Clear();
        foreach (var (key, value) in newAudioClips)
        {
            AudioClips.Add(key, value);
        }
    }
}

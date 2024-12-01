using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    public static class AudioEnumExtensions
    {
        public static void PlayAudio(this AudioClipEnum audioClipEnum, AudioInfo audioInfo)
        {
            AudioClip audioClip = PickRandom(audioClipEnum);
            audioInfo.Clip = audioClip;
            //todo: play sound
        }
        
        public static List<AudioClip> GetAudioCLips(this AudioClipEnum audioClipEnum)
        {
            return GL_AudioClipHolder.GetAudioClips(audioClipEnum);
        }
        
        public static AudioClip PickRandom(this AudioClipEnum audioClipEnum)
        {
            return GL_AudioClipHolder.GetAudioClips(audioClipEnum).PickRandom();
        }
    }
}
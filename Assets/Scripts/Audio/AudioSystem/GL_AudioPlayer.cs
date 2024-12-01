using DG.Tweening;
using UnityEngine;

namespace Audio
{
    public static class GL_AudioPlayer
    {
        public static AudioSource PlayAudio(AudioInfo audioInfo)
        {
            GameObject newSourceObject = new GameObject("AudioSource", typeof(AudioSource));
            newSourceObject.hideFlags = HideFlags.HideInHierarchy;
            AudioSource newSource = newSourceObject.GetComponent<AudioSource>();
            
            newSource.clip = audioInfo.Clip;
            newSource.volume = audioInfo.Volume;
            newSource.pitch = audioInfo.Pitch;
            newSource.maxDistance = audioInfo.MaxDistance;
            newSource.minDistance = audioInfo.MinDistance;
            newSource.dopplerLevel = 0;
            
            SetPositionBehaviour(audioInfo, newSource, newSourceObject);

            SetDurationBehaviour(audioInfo, newSource);

            newSource.Play();
            
            return newSource;
        }

        private static void SetDurationBehaviour(AudioInfo audioInfo, AudioSource newSource)
        {
            switch (audioInfo.DurationBehaviour)
            {
                case AudioDurationBehaviour.Once:
                    newSource.loop = false;
                    break;
                case AudioDurationBehaviour.InfiniteLoop:
                    newSource.loop = true;
                    break;
                case AudioDurationBehaviour.TimedLoopCutoff:
                    newSource.loop = true;
                    Timer.Timer.NewTimer(audioInfo.Duration, newSource.Stop);
                    break;
                case AudioDurationBehaviour.TimedLoopFade:
                    newSource.loop = true;
                    Timer.Timer.NewTimer(audioInfo.Duration - audioInfo.FadeOutDuration, 
                        () => { newSource.DOFade(0, audioInfo.FadeOutDuration).onComplete += newSource.Stop; });
                    break;
            }
        }

        private static void SetPositionBehaviour(AudioInfo audioInfo, AudioSource newSource,
            GameObject newSourceObject)
        {
            switch (audioInfo.PositionBehaviour)
            {
                default:
                case AudioPositionBehaviour.No3D:
                    break;
                case AudioPositionBehaviour.AttachToTransform:
                    newSource.transform.SetParent(audioInfo.AttachToTransform);
                    newSource.transform.localPosition = Vector3.zero;
                    newSource.spatialBlend = 1;
                    break;
                case AudioPositionBehaviour.UsePosition:
                    newSourceObject.transform.position = audioInfo.Position;
                    newSource.spatialBlend = 1;
                    break;
            }
        }
    }
}
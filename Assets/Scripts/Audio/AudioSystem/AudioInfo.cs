using System;
using UnityEngine;

namespace Audio
{
    [Serializable]
    public class AudioInfo
    {
        public AudioClip Clip;
        public float Volume;
        public float Pitch = 1;

        public AudioPositionBehaviour PositionBehaviour = AudioPositionBehaviour.No3D;
        public Transform AttachToTransform;
        public Vector3 Position;
        
        public float MinDistance;
        public float MaxDistance;
        
        public AudioDurationBehaviour DurationBehaviour = AudioDurationBehaviour.Once;
        public float Duration;
        public float FadeOutDuration;
    }
    
    [Serializable]
    public class SerializableAudioInfo : AudioInfo
    {
        public AudioClipEnum ClipEnum;
    }

    public enum AudioPositionBehaviour
    {
        No3D,
        AttachToTransform,
        UsePosition,
    }
    
    public enum AudioDurationBehaviour
    {
        Once,
        InfiniteLoop,
        TimedLoopCutoff,
        TimedLoopFade,
    }
}
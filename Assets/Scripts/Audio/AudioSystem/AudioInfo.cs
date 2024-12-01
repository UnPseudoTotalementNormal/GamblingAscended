using UnityEngine;

namespace Audio
{
    public class AudioInfo
    {
        public AudioClip Clip;
        public float Volume;
        public float Pitch;

        public AudioPositionBehaviour PositionBehaviour = AudioPositionBehaviour.No3D;
        public Transform AttachToTransform;
        public Vector3 Position;
        
        public float MinDistance;
        public float MaxDistance;
        
        public AudioDurationBehaviour DurationBehaviour = AudioDurationBehaviour.Once;
        public float Duration;
        public float FadeOutDuration;
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
using UnityEngine;

namespace Audio
{
    public class AudioInfoPresets
    {
        public static AudioInfo Basic2D(float pitchRange = 0)
        {
            AudioInfo audioInfo = new AudioInfo
            {
                Clip = null,
                Volume = 1,
                Pitch = 1 + Random.Range(-pitchRange, pitchRange),
                PositionBehaviour = AudioPositionBehaviour.No3D,
                AttachToTransform = null,
                Position = default,
                MinDistance = 0,
                MaxDistance = 0
            };
            return audioInfo;
        }
        
        public static AudioInfo Basic3DGameObject(Transform attachedTransform, float pitchRange = 0)
        {
            AudioInfo audioInfo = new AudioInfo
            {
                Clip = null,
                Volume = 1,
                Pitch = 1 + Random.Range(-pitchRange, pitchRange),
                PositionBehaviour = AudioPositionBehaviour.AttachToTransform,
                AttachToTransform = attachedTransform,
                Position = default,
                MinDistance = 5,
                MaxDistance = 10
            };
            return audioInfo;
        }
        
        public static AudioInfo Basic3DPosition(Vector3 position, float pitchRange = 0)
        {
            AudioInfo audioInfo = new AudioInfo
            {
                Clip = null,
                Volume = 1,
                Pitch = 1 + Random.Range(-pitchRange, pitchRange),
                PositionBehaviour = AudioPositionBehaviour.UsePosition,
                AttachToTransform = null,
                Position = position,
                MinDistance = 5,
                MaxDistance = 10
            };
            return audioInfo;
        }
    }
}
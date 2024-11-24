using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "LightingPreset", menuName = "LightingPreset")]
public class GL_LightingPreset : ScriptableObject
{
    public Gradient AmbiantColor;
    public Gradient DirectionalColor;
    public Gradient FogColor;
}

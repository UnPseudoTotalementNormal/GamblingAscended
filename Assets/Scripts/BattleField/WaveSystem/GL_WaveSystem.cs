using System.Collections.Generic;
using BattleField.WaveSystem;
using UnityEngine;

public class GL_WaveSystem : MonoBehaviour
{
    public int CurrentWave = 0;

    [SerializeField] private List<GL_WaveInfo> _waves = new();
}

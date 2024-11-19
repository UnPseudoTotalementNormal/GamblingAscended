using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using GameEvents;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSlotMachineImage", menuName = "Gambling/SlotMachine/Slot Machine Image")]
public class GL_SlotMachineImage : ScriptableObject
{
    public string Name;
    public Sprite ObjectSprite;

    [Header("Gambling values")] 
    [SerializedDictionary("Image Amount", "Value Amount")]
    public SerializedDictionary<int, float> ValuesOnAmount;
    [SerializedDictionary("Image Amount", "GameEvent")]
    public SerializedDictionary<int, GameEvent<GameEventInfo>> GameEventOnAmount;
}

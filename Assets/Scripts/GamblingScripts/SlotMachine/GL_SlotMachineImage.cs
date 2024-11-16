using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSlotMachineImage", menuName = "Gambling/SlotMachine/Slot Machine Image")]
public class GL_SlotMachineImage : ScriptableObject
{
    public string Name;
    public Sprite ObjectSprite;

    [Header("Gambling values")] 
    [SerializedDictionary("Image Amount", "Value Amount")]
    public SerializedDictionary<int, float> ValuesOnAmount;
}

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSlotMachineImage", menuName = "Gambling/SlotMachine/Slot Machine Image")]
public class GL_SlotMachineImage : ScriptableObject
{
    public string Name;
    public Sprite ObjectSprite;

    [Header("Gambling values")] 
    public Dictionary<int, float> ValuesOnAmount;
}

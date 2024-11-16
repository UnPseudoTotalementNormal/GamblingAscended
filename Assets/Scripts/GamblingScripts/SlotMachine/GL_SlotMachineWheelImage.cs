using UnityEngine;

namespace GamblingScripts.SlotMachine
{
    public class GL_SlotMachineWheelImage : MonoBehaviour
    {
        private GL_SlotMachineWheel _slotMachineWheel;

        private GL_SlotMachineImage _slotMachineImage;

        public void Init(GL_SlotMachineWheel wheel)
        {
            _slotMachineWheel = wheel;
            RandomizeImage();
        }

        private void RandomizeImage()
        {
            _slotMachineImage = _slotMachineWheel.GetPossibleResults().PickRandom();
        }
        
        public GL_SlotMachineImage GetSlotMachineImage() => _slotMachineImage;
    }
}
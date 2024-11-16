using UnityEngine;
using UnityEngine.UI;

namespace GamblingScripts.SlotMachine
{
    public class GL_SlotMachineWheelImage : MonoBehaviour
    {
        private GL_SlotMachineWheel _slotMachineWheel;

        private GL_SlotMachineImage _slotMachineImage;

        private SpriteRenderer _spriteRenderer;

        public void Init(GL_SlotMachineWheel wheel)
        {
            _slotMachineWheel = wheel;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            RandomizeImage();
        }

        private void RandomizeImage()
        {
            _slotMachineImage = _slotMachineWheel.GetPossibleResults().PickRandom();
            _spriteRenderer.sprite = _slotMachineImage.ObjectSprite;
        }
        
        public GL_SlotMachineImage GetSlotMachineImage() => _slotMachineImage;
    }
}
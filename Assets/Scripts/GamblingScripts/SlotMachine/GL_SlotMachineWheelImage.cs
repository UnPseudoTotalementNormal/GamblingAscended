using System;
using UnityEngine;
using UnityEngine.UI;

namespace GamblingScripts.SlotMachine
{
    public class GL_SlotMachineWheelImage : MonoBehaviour
    {
        private GL_SlotMachineWheel _slotMachineWheel;

        private GL_SlotMachineImage _slotMachineImage;
        private Transform _switchPoint;

        private SpriteRenderer _spriteRenderer;

        private bool _shouldSwitch = true;

        public void Init(GL_SlotMachineWheel wheel, Transform switchPoint)
        {
            _slotMachineWheel = wheel;
            _switchPoint = switchPoint;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            RandomizeImage();
        }

        private void Update()
        {
            if (Vector3.Distance(transform.position, _switchPoint.position) < 0.2f)
            {
                if (!_shouldSwitch) return;
                
                RandomizeImage();
                _shouldSwitch = false;
            }
            else
            {
                _shouldSwitch = true;
            }
        }

        private void RandomizeImage()
        {
            _slotMachineImage = _slotMachineWheel.GetPossibleResults().PickRandom();
            _spriteRenderer.sprite = _slotMachineImage.ObjectSprite;
        }
        
        public GL_SlotMachineImage GetSlotMachineImage() => _slotMachineImage;
    }
}
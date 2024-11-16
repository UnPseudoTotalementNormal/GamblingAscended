using System.Collections.Generic;
using UnityEngine;

namespace GamblingScripts.SlotMachine
{
    public class GL_SlotMachineImageRow : MonoBehaviour
    {
        [SerializeField] private List<GL_SlotMachineImage> _possibleResultImages;
        
        private GL_SlotMachineImage _resultImage;

        private bool _isRolling = false;
        public bool IsRolling => _isRolling;

        public void StartRolling()
        {
            _isRolling = true;
        }
        
        public void StopRolling()
        {
            _isRolling = false;
        }
        
        public GL_SlotMachineImage GetResultImage()
        {
            return _resultImage;
        }
    }
}
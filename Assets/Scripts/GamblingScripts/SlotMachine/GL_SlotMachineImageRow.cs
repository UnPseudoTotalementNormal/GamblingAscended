using UnityEngine;

namespace GamblingScripts.SlotMachine
{
    public class GL_SlotMachineImageRow : MonoBehaviour
    {
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
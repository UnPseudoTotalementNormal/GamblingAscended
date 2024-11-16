using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Transform = UnityEngine.Transform;

namespace GamblingScripts.SlotMachine
{
    public class GL_SlotMachineWheel : MonoBehaviour
    {
        private Transform _transform;
        [SerializeField] private Transform _wheelPivot;
        [SerializeField] private Transform _imageHolder;
        [SerializeField] private Transform _resultPoint;
        [SerializeField] private List<GL_SlotMachineImage> _possibleResultImages;
        [SerializeField] private float _spinningSpeed;

        private bool _isRolling = false;
        public bool IsRolling => _isRolling;
        
        public List<GL_SlotMachineImage> GetPossibleResults() => _possibleResultImages;
        
        private void Awake()
        {
            _transform = GetComponent<Transform>();
            foreach (Transform image in _imageHolder)
            {
                image.GetComponent<GL_SlotMachineWheelImage>().Init(this);
            }
        }
        
        private void Update()
        {
            if (_isRolling)
            {
                _wheelPivot.Rotate(Vector3.back, _spinningSpeed * Time.deltaTime);
            }
        }

        public void StartRolling()
        {
            _isRolling = true;
        }
        
        public void StopRolling()
        {
            _isRolling = false;
            _wheelPivot.localEulerAngles = new Vector3(0, 0, Mathf.Round(_wheelPivot.localEulerAngles.z / 45) * 45);
            Debug.Log(GetResultImage().Name);
        }
        
        public GL_SlotMachineImage GetResultImage()
        {
            Transform closestImage = null;
            float closestDistance = float.MaxValue;
            foreach (Transform image in _imageHolder)
            {
                float distance = Vector3.Distance(image.position, _resultPoint.position);
                if (!(distance < closestDistance)) continue;
                
                closestDistance = distance;
                closestImage = image;
            }
            return closestImage ? closestImage.GetComponent<GL_SlotMachineWheelImage>().GetSlotMachineImage() : null;
        }
    }
}
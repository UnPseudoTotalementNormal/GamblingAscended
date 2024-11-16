using System;
using UnityEngine;

namespace Timer
{
    public class TimerHost : MonoBehaviour
    {
        private Action _timerEndCallback;
        
        private float _duration;
        private float _timer;

        private bool _isTimerRunning;

        public void StartTimer(float duration, Action callback)
        {
            _timer = 0;
            _duration = duration;
            _timerEndCallback = callback;
            ResumeTimer();
        }

        public void ResumeTimer()
        {
            _isTimerRunning = true;
        }

        public void PauseTimer()
        {
            _isTimerRunning = false;
        }
        
        private void Update()
        {
            if (!_isTimerRunning)
            {
                return;
            }
            
            _timer += Time.deltaTime;
            if (!(_timer >= _duration))
            {
                return;
            }
            
            _timerEndCallback();
            Destroy(gameObject);
        }
    }
}
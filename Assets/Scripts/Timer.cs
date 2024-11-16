using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Timer
{
    /// <summary>
    /// A timer that calls a callback when the timer ends.
    /// </summary>
    public class Timer
    {
        private readonly Action _timerEndCallback;
        private TimerHost _timerHost;
        
        public Timer(float duration, Action callback)
        {
            _timerEndCallback = callback;
            StartTimer(duration);
        }

        private void StartTimer(float duration)
        {
            _timerHost = new GameObject("Timer (duration: " + duration + ")").AddComponent<TimerHost>();
            _timerHost.StartTimer(duration, OnTimerEnd);
        }
        
        private void OnTimerEnd()
        {
            _timerEndCallback();
        }
        
        /// <summary>
        /// Resumes the timer. If the timer was paused, it will start updating again.
        /// </summary>
        public void ResumeTimer()
        {
            _timerHost.ResumeTimer();
        }

        /// <summary>
        /// Pauses the timer. If the timer is running, it will stop updating.
        /// </summary>
        public void PauseTimer()
        {
            _timerHost.PauseTimer();
        }
        
        /// <summary>
        /// Stops the timer and disposes of it.
        /// </summary>
        public void StopTimer()
        {
            _timerHost.PauseTimer();
            Object.Destroy(_timerHost.gameObject);
        }

        /// <summary>
        /// Immediately ends the timer and calls the callback.
        /// </summary>
        public void FinishTimer()
        {
            StopTimer();
            _timerEndCallback();
        }
    }

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
            Destroy(this);
        }
    }
}
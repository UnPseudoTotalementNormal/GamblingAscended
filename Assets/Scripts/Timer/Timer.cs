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

        /// <summary>
        /// Begins a new timer with the specified duration and callback.
        /// </summary>
        /// <param name="duration">The duration of the timer in seconds.</param>
        /// <param name="callback">The callback to invoke when the timer ends.</param>
        /// <returns>A new Timer instance.</returns>
        public static Timer NewTimer(float duration, Action callback)
        {
            return new Timer(duration, callback);
        }

        private void StartTimer(float duration)
        {
            _timerHost = new GameObject("Timer (duration: " + duration + ")").AddComponent<TimerHost>();
            _timerHost.hideFlags = HideFlags.HideInHierarchy;
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
}
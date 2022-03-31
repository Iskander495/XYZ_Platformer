using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Components
{
    public class TimerComponent : MonoBehaviour
    {
        [SerializeField] private TimerData[] _timers;

        public void SetTimer(int timerIndex)
        {
            var timer = _timers[timerIndex];

            StartCoroutine(StartTimer(timer));
        }

        private IEnumerator StartTimer(TimerData timer)
        {
            yield return new WaitForSeconds(timer.Delay);

            timer.OnTimesUp?.Invoke();
        }
    }

    [Serializable]
    public class TimerData
    {
        public float Delay;

        public UnityEvent OnTimesUp;
    }
}
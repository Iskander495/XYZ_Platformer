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

        private void Awake()
        {
            for(int i = 0; i < _timers.Length; i++)
            {
                if(_timers[i].AutoStart)
                {
                    SetTimer(i);
                }
            }
        }

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
        public bool AutoStart = false;

        public float Delay;

        public UnityEvent OnTimesUp;
    }
}
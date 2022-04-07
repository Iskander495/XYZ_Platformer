using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    [Serializable]
    public class Cooldown
    {
        [SerializeField] public float value;

        private float _timesUp;

        public void Reset()
        {
            _timesUp = Time.time + value;
        }

        public bool IsReady => _timesUp <= Time.time;
    }
}
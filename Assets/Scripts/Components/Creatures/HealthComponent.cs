using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Components.Creatures
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int _health;
        [SerializeField] private UnityEvent _onDamage;
        [SerializeField] private UnityEvent _onHealth;
        [SerializeField] private UnityEvent _onDie;

        [SerializeField] private HealthChangeEvent _onHealthChange;

        public void ModifyHealth(int healthDelta)
        {
            if (_health <= 0) return;

            _health += healthDelta;
            _onHealthChange?.Invoke(_health);

            if(healthDelta < 0)
            {
                _onDamage?.Invoke();
            }

            if(healthDelta > 0)
            {
                _onHealth?.Invoke();
            }

            if(_health <= 0)
            {
                _onDie?.Invoke();
            }
        }

        public void SetHealth(int health)
        {
            _health = health;
        }
    }

    [Serializable]
    public class HealthChangeEvent : UnityEvent<int>
    {
    }
}
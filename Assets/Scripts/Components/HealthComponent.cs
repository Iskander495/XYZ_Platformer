using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Components
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] public int _health;

        [SerializeField] private UnityEvent _onDamage;
        [SerializeField] private UnityEvent _onHealth;
        [SerializeField] private UnityEvent _onDie;

        public void ModifyHealth(int healthDelta)
        {
            _health += healthDelta;

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
    }
}
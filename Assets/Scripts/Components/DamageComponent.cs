using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Components
{
    public class DamageComponent : MonoBehaviour
    {
        /// <summary>
        /// Количество наносимого урона
        /// </summary>
        [SerializeField] private int _damage;

        /// <summary>
        /// Наносим урон
        /// </summary>
        /// <param name="target"></param>
        public void ApplyDamage(GameObject target)
        {
            var healthComponent = target.GetComponent<HealthComponent>();

            if(healthComponent!= null)
            {
                healthComponent.ApplyDamage(_damage);
            }
        }
    }
}
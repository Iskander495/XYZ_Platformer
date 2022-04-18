using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Components.Creatures
{
    public class ModifyHealthComponent : MonoBehaviour
    {
        /// <summary>
        /// Количество наносимого урона
        /// </summary>
        [SerializeField] private int _hpDelta;

        /// <summary>
        /// Наносим урон
        /// </summary>
        /// <param name="target"></param>
        public void ApplyHealthDelta(GameObject target)
        {
            var healthComponent = target.GetComponent<HealthComponent>();

            if(healthComponent!= null)
            {
                healthComponent.ModifyHealth(_hpDelta);
            }
        }
    }
}
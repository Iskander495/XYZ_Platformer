using Components.Creatures;
using UnityEngine;

namespace Components.Collectables
{

    public class BoostComponent : MonoBehaviour
    {
        /// <summary>
        /// Кратность увеличения скорости при бусте
        /// </summary>
        [SerializeField] private float _boostValue;

        public void ApplyBoost(GameObject target)
        {
            var creature = target.GetComponent<Creature>();

            if (creature != null)
            {
                creature.ApplyBoost(_boostValue);
            }
        }
    }
}
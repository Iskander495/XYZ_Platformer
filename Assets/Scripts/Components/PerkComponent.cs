using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Components
{
    public class PerkComponent : MonoBehaviour
    {
        [SerializeField] private Perk _perk;
        [SerializeField] private GameObject _target;

        public void GivePerk()
        {
            var component = _target.GetComponent<PerkStore>();

            component?.AddPerk(_perk);
        }
    }

    public enum Perk {
        RockClimber,
        Immortality,
        Sword
    }
}
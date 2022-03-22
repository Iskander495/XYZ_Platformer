using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Components
{
    public class PerkComponent : MonoBehaviour
    {
        [SerializeField] private Perk _perk;
        [SerializeField] private GameObject _target;
        [SerializeField] private UnityEvent _action;

        public void GivePerk()
        {
            var component = _target.GetComponent<PerkStore>();

            component?.AddPerk(_perk);
        }

        public void ActionIfPresentPerk()
        {
            var component = _target.GetComponent<PerkStore>();
            if (component.PresentPerk(_perk) && _action != null)
            {
                _action.Invoke();
            }
        }
    }

    [Serializable]
    public enum Perk {
        RockClimber,
        Immortality,
        Sword,
        Key
    }
}
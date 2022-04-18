using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Components.Animation
{
    public class SwitchComponent : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        [SerializeField] private string _triggerKey;

        [SerializeField] private bool _state;

        public void Switch()
        {
            _state = !_state;

            _animator.SetBool(_triggerKey, _state);
        }

        [ContextMenu("Switch")]
        public void SwitchIt()
        {
            Switch();
        }
    }
}
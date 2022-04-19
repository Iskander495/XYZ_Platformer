using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

namespace Components.Animation
{
    public class SwitchAnimatorComponent : MonoBehaviour
    {
        [SerializeField] private AnimatorController _headAnimator;
        [SerializeField] private AnimatorController _secondAnimator;
        [SerializeField] private Animator _animator;

        public void Switch(bool isFirst)
        {
            if (isFirst)
                _animator.runtimeAnimatorController = _headAnimator;
            else
                _animator.runtimeAnimatorController = _secondAnimator;
        }
    }
}
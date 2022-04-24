using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Components.Creatures.Mobs
{
    public class BaseAI : MonoBehaviour
    {
        [SerializeField] protected Creature _creature;
        [SerializeField] protected Animator _animator;
        /// <summary>
        /// Цель атаки
        /// </summary>
        protected GameObject _targetAttack;
        /// <summary>
        /// Признак смерти
        /// </summary>
        protected bool _isDead;

        protected int _dieLayer;

        protected static readonly int _isDeadKey = Animator.StringToHash("isDead");

        protected Coroutine _currentCoroutine;

        protected virtual void Awake()
        {
            _creature = GetComponent<Creature>();
            _animator = GetComponent<Animator>();
        }

        public virtual void OnDie()
        {
            _isDead = true;
            _animator.SetBool(_isDeadKey, true);

            if (_currentCoroutine != null)
                StopCoroutine(_currentCoroutine);

            gameObject.layer = _dieLayer;
            gameObject.tag = "Untagged";
            _creature.SetDirection(Vector2.zero);
        }

        protected virtual void Stop()
        {
            if (_currentCoroutine != null)
                StopCoroutine(_currentCoroutine);

            _creature.SetDirection(Vector2.zero);
        }

        protected void StartState(IEnumerator coroutine)
        {
            _creature.SetDirection(Vector2.zero);

            if (_currentCoroutine != null)
                StopCoroutine(_currentCoroutine);

            if (!enabled) return;

            _currentCoroutine = StartCoroutine(coroutine);
        }

        protected virtual void SetDirectionToTarget()
        {
            // получаем вектор направления движения
            var direction = GetDirectionToTarget();
            _creature.SetDirection(direction.normalized);
        }

        protected virtual Vector2 GetDirectionToTarget()
        {
            var direction = _targetAttack.transform.position - transform.position;
            direction.y = 0;
            return direction.normalized;
        }
    }
}

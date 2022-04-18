using Components.Collision;
using Components.GameObjects;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Components.Creatures
{
    public class MobAI : MonoBehaviour
    {
        /// <summary>
        /// Зона поиска
        /// </summary>
        [SerializeField] private LayerCheck _vision;
        /// <summary>
        /// Зона атаки
        /// </summary>
        [SerializeField] private LayerCheck _canAttack;
        /// <summary>
        /// Цель атаки
        /// </summary>
        private GameObject _targetAttack;
        /// <summary>
        /// Время задержки до атаки
        /// </summary>
        [SerializeField] private float _alarmDelay = 0.5f;
        /// <summary>
        /// Время восстановления атаки
        /// </summary>
        [SerializeField] private float _attackCooldown = 0.5f;
        /// <summary>
        /// Время дальнейших действий после потери врага
        /// </summary>
        [SerializeField] private float _missHeroCooldown = 0.5f;

        [SerializeField] private LayerMask _layerOnDie;

        [SerializeField] private SpawnListComponent _particles;
        [SerializeField] private Creature _creature;
        [SerializeField] private Animator _animator;

        /// <summary>
        /// Признак смерти
        /// </summary>
        private bool _isDead;

        private int _dieLayer;

        private Patrol _patrol;

        private Coroutine _currentCoroutine;

        private static readonly int _isDeadKey = Animator.StringToHash("isDead");

        private void Awake()
        {
            _dieLayer = Mathf.RoundToInt(Mathf.Log(_layerOnDie.value, 2));

            _particles = GetComponent<SpawnListComponent>();
            _creature = GetComponent<Creature>();
            _animator = GetComponent<Animator>();
            _patrol = GetComponent<Patrol>();
        }

        private void Start()
        {
            StartState(_patrol.DoPatrol());
        }

        /// <summary>
        /// Цель в зоне видимости
        /// </summary>
        /// <param name="go"></param>
        public void OnHeroInVision(GameObject go)
        {
            if (_isDead) return;

            _targetAttack = go;

            StartState(AgroToHero());
        }

        /// <summary>
        /// Агримся
        /// </summary>
        /// <returns></returns>
        public IEnumerator AgroToHero()
        {
            LookAtHero();

            _particles.Spawn("Agro");

            yield return new WaitForSeconds(_alarmDelay);

            StartState(GoToHero());
        }

        private void LookAtHero()
        {
            var direction = GetDirectionToTarget();
            _creature.UpdateSpriteDirection(direction);
        }

        /// <summary>
        /// Движемся к цели
        /// </summary>
        /// <returns></returns>
        public IEnumerator GoToHero()
        {
            while (_vision.IsTouchingLayer)
            {
                // если цель в зоне поражения - атакуем
                if (_canAttack.IsTouchingLayer)
                {
                    StartState(Attack());
                }
                else // иначе движемся к ней
                {
                    SetDirectionToTarget();
                }

                yield return null;
            }

            _creature.SetDirection(Vector2.zero);
            _particles.Spawn("Miss");
            yield return new WaitForSeconds(_missHeroCooldown);

            StartState(_patrol.DoPatrol());
        }

        private IEnumerator Attack()
        {
            while (_canAttack.IsTouchingLayer)
            {
                _creature.Attack();

                yield return new WaitForSeconds(_attackCooldown);
            }

            StartState(GoToHero());
        }

        private void SetDirectionToTarget()
        {
            // получаем вектор направления движения
            var direction = GetDirectionToTarget();
            _creature.SetDirection(direction.normalized);
        }

        private Vector2 GetDirectionToTarget()
        {
            var direction = _targetAttack.transform.position - transform.position;
            direction.y = 0;
            return direction.normalized;
        }

        public void OnDie()
        {
            _isDead = true;
            _animator.SetBool(_isDeadKey, true);

            if (_currentCoroutine != null)
                StopCoroutine(_currentCoroutine);

            //GetComponent<SpriteRenderer>().sortingLayerID = _layerOnDie;
            gameObject.layer = _dieLayer;
            gameObject.tag = "Untagged";
            _creature.SetDirection(Vector2.zero);
        }

        private void Stop()
        {
            if (_currentCoroutine != null)
                StopCoroutine(_currentCoroutine);

            _creature.SetDirection(Vector2.zero);
        }

        private void StartState(IEnumerator coroutine)
        {
            _creature.SetDirection(Vector2.zero);

            if(_currentCoroutine != null)
                StopCoroutine(_currentCoroutine);

            if (!enabled) return;

            _currentCoroutine = StartCoroutine(coroutine);
        }
    }
}
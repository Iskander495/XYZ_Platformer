using Components.Collision;
using Components.GameObjects;
using System.Collections;
using UnityEngine;

namespace Components.Creatures.Mobs
{
    public class MobAI : BaseAI
    {
        /// <summary>
        /// Зона поиска
        /// </summary>
        [SerializeField] private LayerCheck _vision;
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

        private Patrol _patrol;


        protected override void Awake()
        {
            base.Awake();

            _dieLayer = Mathf.RoundToInt(Mathf.Log(_layerOnDie.value, 2));

            _particles = GetComponent<SpawnListComponent>();
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

        /// <summary>
        /// Разворот в сторону противника
        /// </summary>
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
                if (_canMeleeAttack)
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
            while (_canMeleeAttack)
            {
                _creature.Attack();

                yield return new WaitForSeconds(_attackCooldown);
            }

            StartState(GoToHero());
        }
    }
}
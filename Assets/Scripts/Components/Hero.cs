using Components.Creatures;
using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Components
{
    public class Hero : Creature
    {
        [Header("Properties")]
        /// <summary>
        /// На пересечение с каким слоем будет производиться проверка
        /// </summary>
        [SerializeField] private LayerMask _interactionLayerMask;


        /// <summary>
        /// Кулдаун для броска меча
        /// </summary>
        [SerializeField] private Cooldown _throwCooldown;

        /// <summary>
        /// признак, что мы соприкасаемся со стенами
        /// </summary>
        private bool _isOnWall;

        /// <summary>
        /// Можно ли делать дабл-джамп
        /// </summary>
        private bool _allowDoubleJump;

        /// <summary>
        /// Радиус проверки пересечения с интерактивными объектами
        /// </summary>
        private float _interactionRadius;

        /// <summary>
        /// Массив объектов, с которыми пересеклись
        /// </summary>
        //private Collider2D[] _interactionObjects = new Collider2D[1];
        [SerializeField] private CheckCicrcleOverlap _interactionCheck;

        [Space]
        [Header("Components")]
        /// <summary>
        /// Компонент проверки соприкосновения со стенами
        /// </summary>
        [SerializeField] private LayerCheck _wallCheck;
        [SerializeField] private PropabilityDropComponent _hitDrop;


        private PerkStore _perkStore;

        [Space]
        [Header("Animation")]
        [SerializeField] private AnimatorController _armed;
        [SerializeField] private AnimatorController _unarmed;

        private GameSession _sessionOnStartLevel;
        private GameSession _session;

        protected static readonly int _throwTrigger = Animator.StringToHash("isThrow");

        protected override void Awake()
        {
            base.Awake();

            _perkStore = GetComponent<PerkStore>();
        }

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            _session.Load(SceneManager.GetActiveScene().buildIndex);

            var health = GetComponent<HealthComponent>();

            var sessionHP = _session.GetValue<int>("Hp");
            if (sessionHP > 0)
                health.SetHealth(sessionHP);

            UpdateHeroWeapon();
        }

        protected override void Update()
        {
            base.Update();

            _isOnWall = IsWalled();
        }

        protected override float CalculateYVelocity()
        {
            // признак прыжка
            var isJumpPressing = Direction.y > 0;

            // сброс двойного прыжка когда приземлились либо при соприкосновении со стеной (при имеющемся перке)
            if (IsOnGround || _isOnWall)
            {
                _allowDoubleJump = true;
                IsJumping = false;
            }

            return base.CalculateYVelocity();
        }

        protected override float CalculateJumpVelocity(float yVelocity)
        {
            if (!IsOnGround &&  _allowDoubleJump)
            {
                // анимация прыжка
                _particles.Spawn("Jump");
                
                // сбрасываем флаг, чтоб не было тройных и пр. прыжков
                _allowDoubleJump = false;
                
                // "совершаем" двойной прыжок
                return JumpSpeed;
            }

            return base.CalculateJumpVelocity(yVelocity);
        }

        /// <summary>
        /// Проверка на соприкосновение со стенами
        /// </summary>
        /// <returns></returns>
        private bool IsWalled()
        {
            // если способности нет - отключаем
            if (!_perkStore.PresentPerk(Perk.RockClimber)) return false;

            return _wallCheck.IsTouchingLayer;
        }

        /// <summary>
        /// Взаимодействие с объектами (например штурвал открытия дверей)
        /// </summary>
        public void Interact()
        {
            _interactionCheck.Check();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // проверяем пересечение с землей
            if (collision.gameObject.IsInLayer(GroundLayer))
            {
                var contact = collision.contacts[0];
                // при большой скорости вертикального столкновения
                if (contact.relativeVelocity.y >= 12f)
                {
                    // проигрываем анимацию приземления
                      _particles.Spawn("SlamDown");
                }
            }
        }

        public override void Attack()
        {
            if(_session.GetValue<int>("Swords") > 0)
            {
                base.Attack();
            }
        }

        public void UpdateHeroWeapon()
        {
            var count = _session.GetValue<int>("Swords");

            Animator.runtimeAnimatorController = count > 0 ? _armed : _unarmed;
        }

        public void OnHealthChanged(int currentHealth)
        {
            //_session.Data.Hp = currentHealth;
            _session.SetValue<int>("Hp", currentHealth);
        }

        /// <summary>
        /// Бросок меча (вызывается в аниматоре)
        /// </summary>
        public void OnDoThrow()
        {
            _particles.Spawn("Throw");
        }

        /// <summary>
        /// Анимация броска меча
        /// </summary>
        public IEnumerator Throw()
        {
            var count = _session.GetValue<int>("Swords");
            if (count <= 1) yield break;

            if (_throwCooldown.IsReady)
            {
                Animator.SetTrigger(_throwTrigger);
                _throwCooldown.Reset();

                _session.SetValue<int>("Swords", count - 1);
            }

            yield return null;
        }

        public void SuperThrow()
        {
            StartCoroutine(SuperThrowCoroutine());
        }

        public IEnumerator SuperThrowCoroutine()
        {
            var i = 0;
            while(i < 3)
            {
                i++;
                yield return Throw();
                yield return new WaitForSeconds(_throwCooldown.value);
            }

            yield return null;
        }

        public override void TakeDamage()
        {
            base.TakeDamage();
            var count = _session.GetValue<int>("Coins");

            if (count > 0)
            {
                SpawnCoins();
            }
        }

        private void SpawnCoins()
        {
            var count = _session.GetValue<int>("Coins");
            var numCoinsToDrop = Mathf.Min(count, 5);

            _session.SetValue<int>("Coins", count - numCoinsToDrop);

            _hitDrop.SetCount(numCoinsToDrop);
            _hitDrop.CalculateDrop();
        }

        protected override void OnDrawGizmos()
        {
        }
    }
}
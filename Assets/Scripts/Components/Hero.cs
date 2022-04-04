using Components.Creatures;
using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        private PerkStore _perkStore;

        [Space]
        [Header("Animation")]
        [SerializeField] private AnimatorController _armed;
        [SerializeField] private AnimatorController _unarmed;

        private GameSession _sessionOnStartLevel;
        private GameSession _session;

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
            if (_session.Data.Hp > 0)
                health.SetHealth(_session.Data.Hp);

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

            /*if(isJumpPressing && _isOnWall)
            {
                return 0f;
            }*/

            return base.CalculateYVelocity();
        }

        protected override float CalculateJumpVelocity(float yVelocity)
        {
            if (!IsOnGround &&  _allowDoubleJump)
            {
                // анимация прыжка
                Particles.Spawn("Jump");
                
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

        /// <summary>
        /// Эффект пыли из под ног при начале движения
        /// </summary>
        public override void SpawnFootDust()
        {
            Particles.Spawn("Run");
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
                      Particles.Spawn("SlamDown");
                }
            }
        }

        public override void Attack()
        {
            if (_perkStore.PresentPerk(Perk.Sword))
            {
                base.Attack();
            }
        }

        public void ArmHero(bool isArmed)
        {
            if (isArmed)
            {
                _perkStore.AddPerk(Perk.Sword);
            }
            else
            {
                _perkStore.RemovePerk(Perk.Sword);
            }

            UpdateHeroWeapon();
        }

        private void UpdateHeroWeapon()
        {
            Animator.runtimeAnimatorController = _perkStore.PresentPerk(Perk.Sword) ? _armed : _unarmed;
        }

        public void OnHealthChanged(int currentHealth)
        {
            _session.Data.Hp = currentHealth;
        }

        protected override void OnDrawGizmos()
        {
        }
    }
}
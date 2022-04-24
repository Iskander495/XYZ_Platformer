using Components.Collectables;
using Components.Collision;
using Components.Creatures;
using Model;
using Model.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Components
{
    public class Hero : Creature , ICanAddInInventory
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

        [SerializeField] private float _defaultGravityScale;

        [Space]
        [Header("Components")]
        /// <summary>
        /// Компонент проверки соприкосновения со стенами
        /// </summary>
        [SerializeField] private LayerCheck _wallCheck;
        [SerializeField] private PropabilityDropComponent _hitDrop;

        [Space]
        [Header("Animation")]
        [SerializeField] private AnimatorController _armed;
        [SerializeField] private AnimatorController _unarmed;

        private GameSession _sessionOnStartLevel;
        private GameSession _session;

        private HealthComponent _health;

        protected static readonly int _throwTrigger = Animator.StringToHash("isThrow");
        protected static readonly int _wallKey = Animator.StringToHash("isOnWall");

        private int SwordsCount => _session.Data.Inventory.Count("Sword");
        private int CoinsCount => _session.Data.Inventory.Count("Coin");

        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            _session.Data.Inventory.OnChanged += OnInventoryChanged;
            _session.Load(SceneManager.GetActiveScene().buildIndex);

            _health = GetComponent<HealthComponent>();

            var sessionHP = _session.Data.HP;
            if (sessionHP > 0)
                _health.SetHealth(sessionHP);

            UpdateHeroWeapon();
        }

        private void OnDestroy()
        {
            _session.Data.Inventory.OnChanged -= OnInventoryChanged;
        }


        private void OnInventoryChanged(string id, int value)
        {
            if(id == "Sword")
                UpdateHeroWeapon();
        }

        protected override void Update()
        {
            base.Update();

            WallCheck();
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

            if(!isJumpPressing && _isOnWall)
            {
                return 0f;
            }

            return base.CalculateYVelocity();
        }

        protected override float CalculateJumpVelocity(float yVelocity)
        {
            if (!IsOnGround &&  _allowDoubleJump && !_isOnWall)
            {
                // сбрасываем флаг, чтоб не было тройных и пр. прыжков
                _allowDoubleJump = false;
                DoJumpVfx();
                // "совершаем" двойной прыжок
                return JumpSpeed;
            }

            return base.CalculateJumpVelocity(yVelocity);
        }

        /// <summary>
        /// Проверка на соприкосновение со стенами
        /// </summary>
        /// <returns></returns>
        private void WallCheck()
        {
            // если способности нет - отключаем
            //if (!_perkStore.PresentPerk(Perk.RockClimber)) return false;

            // давим ли мы в сторону текущего разворота
            var moveToSameDirection = Direction.x * transform.lossyScale.x > 0;
            if(_wallCheck.IsTouchingLayer && moveToSameDirection)
            {
                _isOnWall = true;
                Rigidbody.gravityScale = 0;
            } 
            else
            {
                _isOnWall = false;
                Rigidbody.gravityScale = _defaultGravityScale;
            }

            Animator.SetBool(_wallKey, _isOnWall);
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

                // при очень большой - ойкаем
                if(contact.relativeVelocity.y >= 18f)
                {
                    Sounds.Play("SlamDown");
                }
            }
        }

        public override void Attack()
        {
            if (SwordsCount <= 0) return;

            base.Attack();
        }

        private void UpdateHeroWeapon()
        {
            Animator.runtimeAnimatorController = SwordsCount > 0 ? _armed : _unarmed;
        }

        public void OnHealthChanged(int currentHealth)
        {
            _session.Data.HP = currentHealth;
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
            if (SwordsCount <= 1) yield break;

            if (_throwCooldown.IsReady)
            {
                Animator.SetTrigger(_throwTrigger);
                Sounds.Play("Range");
                _throwCooldown.Reset();

                _session.Data.Inventory.Remove("Sword", 1);
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
            if (CoinsCount > 0)
            {
                SpawnCoins();
            }
        }

        private void SpawnCoins()
        {
            var numCoinsToDrop = Mathf.Min(CoinsCount, 5);
            _session.Data.Inventory.Remove("Coin", numCoinsToDrop);

            _hitDrop.SetCount(numCoinsToDrop);
            _hitDrop.CalculateDrop();
        }

        public void AddInInventory(string id, int value)
        {
            _session.Data.Inventory.Add(id, value);
        }

        public void UsePotion ()
        {
            var id = _session.Data.Inventory.IsPresent("HealthPotion");
            if(id != null)
            {
                var item = _session.Data.Inventory.GetItem((Guid)id);
                _health.ModifyHealth(item.Value);

                _session.Data.Inventory.Remove(item.guid, item.Value);
            }
        }

        protected override void OnDrawGizmos()
        {
        }
    }
}
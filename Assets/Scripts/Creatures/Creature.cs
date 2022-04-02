using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Components.Creatures
{
    public class Creature : MonoBehaviour
    {
        [Header("Params")]
        /// <summary>
        /// Наносимый урон
        /// </summary>
        [SerializeField] protected int Damage;
        /// <summary>
        /// Скорость передвижения
        /// </summary>
        [SerializeField] protected float Speed;
        /// <summary>
        /// Скорость (импульс) прыжка
        /// </summary>
        [SerializeField] protected float JumpSpeed;
        /// <summary>
        /// Импульс вверх при получении урона
        /// </summary>
        [SerializeField] protected float DamageJumpSpeed;

        [Header("Checkers")]
        /// <summary>
        /// Какой слой означает землю
        /// </summary>
        [SerializeField] protected LayerMask GroundLayer;
        /// <summary>
        /// Зона охвата для урона
        /// </summary>
        [SerializeField] protected CheckCicrcleOverlap AttackRange;

        [Space] [Header("Components")]
        /// <summary>
        /// Партиклы
        /// </summary>        
        [SerializeField] protected SpawnListComponent Particles;
        /// <summary>
        /// Компонент проверки соприкосновения с землей
        /// </summary>
        [SerializeField] protected LayerCheck GroundCheck;


        /// <summary>
        /// Вектор движения
        /// </summary>
        protected Vector2 Direction;
        /// <summary>
        /// признак, что мы стоим на земле
        /// </summary>
        protected bool IsOnGround;
        /// <summary>
        /// Признак, что мы находимся в прыжке
        /// </summary>
        protected bool IsJumping;
        protected Rigidbody2D Rigidbody;
        protected Animator Animator;

        protected static readonly int _isGround = Animator.StringToHash("isGround");
        protected static readonly int _isRunning = Animator.StringToHash("isRunning");
        protected static readonly int _verticalVelocity = Animator.StringToHash("verticalVelocity");
        protected static readonly int _hitTrigger = Animator.StringToHash("hitTrigger");
        protected static readonly int _attackTrigger = Animator.StringToHash("attack");

        protected virtual void Awake()
        {
            Particles = GetComponent<SpawnListComponent>();
            Rigidbody = GetComponent<Rigidbody2D>();
            Animator = GetComponent<Animator>();
        }

        protected virtual void Update()
        {
            IsOnGround = IsGrounded();
        }

        private void FixedUpdate()
        {
            var xVelocity = Direction.x * Speed;
            var yVelocity = CalculateYVelocity();

            Rigidbody.velocity = new Vector2(xVelocity, yVelocity);

            ////// ANIMATION ///////
            Animator.SetFloat(_verticalVelocity, Rigidbody.velocity.y);
            Animator.SetBool(_isRunning, Direction.x != 0);
            Animator.SetBool(_isGround, IsOnGround);

            UpdateSpriteDirection(Direction);
        }

        /// <summary>
        /// Подсчет ускорения по оси Y
        /// </summary>
        /// <returns></returns>
        protected virtual float CalculateYVelocity()
        {
            var retYVelocity = Rigidbody.velocity.y;

            // сброс двойного прыжка когда приземлились либо при соприкосновении со стеной (при имеющемся перке)
            if (IsOnGround)
            {
                IsJumping = false;
            }

            // признак прыжка
            var isJumpPressing = Direction.y > 0;

            // если прыгнули - придаем импульс направленный вверх
            if (isJumpPressing)
            {
                IsJumping = true;

                // признак того, что летим вниз
                var isFalling = Rigidbody.velocity.y <= 0.001f;
                // если не падаем - ничего не делаем
                retYVelocity = isFalling ? CalculateJumpVelocity(retYVelocity) : retYVelocity;
            }
            else if (Rigidbody.velocity.y > 0 && IsJumping) // если движемся вверх (находимся в прыжке)
            {
                // уменьшаем импульс в 2 раза
                retYVelocity *= 0.5f;
            }

            return retYVelocity;
        }

        /// <summary>
        /// Расчет импульса прыжка
        /// </summary>
        /// <param name="yVelocity"></param>
        /// <returns></returns>
        protected virtual float CalculateJumpVelocity(float yVelocity)
        {
            // если стоим на земле
            if (IsOnGround)
            {
                // то просто прыгаем
                yVelocity = JumpSpeed;
                Particles.Spawn("Jump");
            }

            return yVelocity;
        }

        /// <summary>
        /// Задание вектора движения героя
        /// </summary>
        /// <param name="direction"></param>
        public virtual void SetDirection(Vector2 direction)
        {
            Direction = direction;
        }

        /// <summary>
        /// "Поворот" героя в сторону движения
        /// </summary>
        /// <param name="_direction"></param>
        private void UpdateSpriteDirection(Vector2 _direction)
        {
            if (_direction.x > 0)
            {
                transform.localScale = Vector3.one;
            }
            else if (_direction.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }

        /// <summary>
        /// получение урона (анимация и "прыжок")
        /// </summary>
        public virtual void TakeDamage()
        {
            IsJumping = false;
            Animator.SetTrigger(_hitTrigger);
            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, DamageJumpSpeed);
        }

        /// <summary>
        /// лечение
        /// </summary>
        public virtual void TakeHealth()
        {
        }

        /// <summary>
        /// Проверка на нахождение персонажа "на земле"
        /// </summary>
        /// <returns></returns>
        private bool IsGrounded()
        {
            return GroundCheck.IsTouchingLayer;
        }

        public virtual void Attack()
        {
            Animator.SetTrigger(_attackTrigger);
            Particles.Spawn("Attack");
        }

        public void OnDoAttack()
        {
            //var objects = AttackRange.GetObjectsInRange(new string[] { "Enemy" });
            //var objects = _attackRange.GetObjectsInRange();

            AttackRange.Check();

            /*foreach (var obj in objects)
            {
                var healthComp = obj.GetComponent<HealthComponent>();
                healthComp?.ModifyHealth(-Damage);
            }*/
        }

        protected virtual void OnDrawGizmos()
        {
            //Debug.DrawRay(transform.position, Vector2.down, IsGrounded() ? Color.green : Color.red);
            //Gizmos.color = IsGrounded() ? Color.green : Color.red;
            //Gizmos.DrawSphere(transform.position, 0.3f);
        }
    }
}
using Components;
using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hero : MonoBehaviour
{
    [Header("Properties")]
    /// <summary>
    /// Вектор движения
    /// </summary>
    private Vector2 _direction;

    /// <summary>
    /// Наносимый урон
    /// </summary>
    [SerializeField] private int _damage;

    /// <summary>
    /// Скорость передвижения
    /// </summary>
    [SerializeField] private float _speed;

    /// <summary>
    /// Скорость (импульс) прыжка
    /// </summary>
    [SerializeField] private float _jumpSpeed;

    /// <summary>
    /// Импульс вверх при получении урона
    /// </summary>
    [SerializeField] private float _damageJumpSpeed;

    /// <summary>
    /// На пересечение с каким слоем будет производиться проверка
    /// </summary>
    [SerializeField] private LayerMask _interactionLayerMask;

    /// <summary>
    /// Какой слой означает землю
    /// </summary>
    [SerializeField] private LayerMask _groundLayer;

    /// <summary>
    /// признак, что мы стоим на земле
    /// </summary>
    private bool _isGrounded;

    /// <summary>
    /// признак, что мы соприкасаемся со стенами
    /// </summary>
    private bool _isWalled;

    private bool _isJumping;

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
    private Collider2D[] _interactionObjects = new Collider2D[1];

    [Space] 
    [Header("Components")]
    /// <summary>
    /// Партиклы эффекта пыли из под ног
    /// </summary>
    [SerializeField] private SpawnComponent _footStepParticles;
    /// <summary>
    /// Партиклы прыжка
    /// </summary>
    [SerializeField] private SpawnComponent _jumpParticles;
    /// <summary>
    /// Партиклы приземления
    /// </summary>
    [SerializeField] private SpawnComponent _fallParticles;
    /// <summary>
    /// Партиклы меча
    /// </summary>
    [SerializeField] private SpawnComponent _swordParticles;
    /// <summary>
    /// Компонент проверки соприкосновения с землей
    /// </summary>
    [SerializeField] private GroundCheck _groundCheck;
    /// <summary>
    /// Компонент проверки соприкосновения со стенами
    /// </summary>
    [SerializeField] private GroundCheck _wallCheck;

    [SerializeField] private CheckCicrcleOverlap _attackRange;


    ////// COMPONENTS ///////
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private PerkStore _perkStore;

    [Space]
    [Header("Animation")]
    [SerializeField] private AnimatorController _armed;
    [SerializeField] private AnimatorController _unarmed;

    private static readonly int _isGround = Animator.StringToHash("isGround");
    private static readonly int _isRunning = Animator.StringToHash("isRunning");
    private static readonly int _verticalVelocity = Animator.StringToHash("verticalVelocity");
    private static readonly int _hitTrigger = Animator.StringToHash("hitTrigger");
    private static readonly int _attackTrigger = Animator.StringToHash("attack");

    private GameSession _sessionOnStartLevel;
    private GameSession _session;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _perkStore = GetComponent<PerkStore>();
    }

    private void Start()
    {
        _session = FindObjectOfType<GameSession>();
        _session.Load(SceneManager.GetActiveScene().buildIndex);

        var health = GetComponent<HealthComponent>();
        if(_session.Data.Hp > 0)
            health.SetHealth(_session.Data.Hp);

        UpdateHeroWeapon();
    }

    private void Update()
    {
        _isGrounded = IsGrounded();
        _isWalled = IsWalled();
    }

    private void FixedUpdate()
    {
        var xVelocity = _direction.x * _speed;
        var yVelocity = CelculateYVelocity();

        _rigidbody.velocity = new Vector2(xVelocity, yVelocity);

        ////// ANIMATION ///////
        _animator.SetFloat(_verticalVelocity, _rigidbody.velocity.y);
        _animator.SetBool(_isRunning, _direction.x != 0);
        _animator.SetBool(_isGround, _isGrounded);

        UpdateSpriteDirection(_direction);
    }

    /// <summary>
    /// Подсчет ускорения по оси Y
    /// </summary>
    /// <returns></returns>
    private float CelculateYVelocity()
    {
        var retYVelocity = _rigidbody.velocity.y;

        // сброс двойного прыжка когда приземлились либо при соприкосновении со стеной (при имеющемся перке)
        if (_isGrounded || _isWalled)
        {
            _allowDoubleJump = true;
            _isJumping = false;
        }

        // признак прыжка
        var isJumpPressing = _direction.y > 0;

        // если прыгнули - придаем импульс направленный вверх
        if (isJumpPressing)
        {
            _isJumping = true;
            retYVelocity = CalculateJumpVelocity(retYVelocity);
        }
        else if (_rigidbody.velocity.y > 0 && _isJumping) // если движемся вверх (находимся в прыжке)
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
    private float CalculateJumpVelocity(float yVelocity)
    {
        // признак того, что летим вниз
        var isFalling = _rigidbody.velocity.y <= 0.001f;

        // если не падаем - ничего не делаем
        if (!isFalling)
        {
            return yVelocity;
        }

        // если стоим на земле
        if (_isGrounded)
        {
            // то просто прыгаем
            yVelocity += _jumpSpeed;
            JumpParticles();
        } 
        else if(_allowDoubleJump) // иначе, если доступен двойной прыжок
        {
            // "совершаем" двойной прыжок
            yVelocity = _jumpSpeed;
            JumpParticles();
            // сбрасываем флаг, чтоб не было тройных и пр. прыжков
            _allowDoubleJump = false;
        }

        return yVelocity;
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
    /// Проверка на нахождение персонажа "на земле"
    /// </summary>
    /// <returns></returns>
    private bool IsGrounded()
    {
        return _groundCheck.IsTouchingLayer;
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
    /// Задание вектора движения героя
    /// </summary>
    /// <param name="direction"></param>
    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }

    /// <summary>
    /// получение урона (анимация и "прыжок")
    /// </summary>
    public void TakeDamage()
    {
        _isJumping = false;
        _allowDoubleJump = true;

        _animator.SetTrigger(_hitTrigger);

        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _damageJumpSpeed);
    }

    /// <summary>
    /// Заглушка на лечение
    /// </summary>
    public void TakeHealth()
    {
    }

    /// <summary>
    /// Взаимодействие с объектами (например штурвал открытия дверей)
    /// </summary>
    public void Interact()
    {
        // нам нужно получить количество пересечений
        var size = Physics2D.OverlapCircleNonAlloc(transform.position, _interactionRadius, _interactionObjects, _interactionLayerMask);

        for(var i = 0; i < size; i++)
        {
            _interactionObjects[i].GetComponent<InteractibleComponent>()?.Interact();
        }
    }

    /// <summary>
    /// Эффект пыли из под ног при начале движения
    /// </summary>
    public void SpawnFootDust()
    {
        _footStepParticles.Spawn();
    }

    /// <summary>
    /// Анимация партиклов прыжка
    /// </summary>
    public void JumpParticles()
    {
        if (_isGrounded || _allowDoubleJump) //оттолкнулись от земли
            _jumpParticles.Jump();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // проверяем пересечение с землей
        if(collision.gameObject.IsInLayer(_groundLayer))
        {
            var contact = collision.contacts[0];
            // при большой скорости вертикального столкновения
            if(contact.relativeVelocity.y >= 12f)
            {
                // проигрываем анимацию приземления
                _fallParticles.Process();
            }
        }
    }

    public void Attack()
    {
        if (_perkStore.PresentPerk(Perk.Sword))
        {
            _animator.SetTrigger(_attackTrigger);

            _swordParticles.Process();
        }
    }

    public void OnHeroAttack()
    {
        var objects = _attackRange.GetObjectsInRange(new string[] { "Enemy" });
        foreach (var obj in objects)
        {
            var healthComp = obj.GetComponent<HealthComponent>();
            healthComp?.ModifyHealth(-_damage);
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
        _animator.runtimeAnimatorController = _perkStore.PresentPerk(Perk.Sword) ? _armed : _unarmed;
    }

    public void OnHealthChanged(int currentHealth)
    {
        _session.Data.Hp = currentHealth;
    }

    private void OnDrawGizmos()
    {
        //Debug.DrawRay(transform.position, Vector2.down, IsGrounded() ? Color.green : Color.red);
        //Gizmos.color = IsGrounded() ? Color.green : Color.red;
        //Gizmos.DrawSphere(transform.position, 0.3f);
    }
}

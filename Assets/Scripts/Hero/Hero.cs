using Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    /// <summary>
    /// Вектор движения
    /// </summary>
    private Vector2 _direction;

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
    /// Компонент проверки соприкосновения с землей
    /// </summary>
    [SerializeField] private GroundCheck _groundCheck;

    /// <summary>
    /// признак, что мы стоим на земле
    /// </summary>
    private bool _isGrounded;

    /// <summary>
    /// Можно ли делать дабл-джамп
    /// </summary>
    private bool _allowDoubleJump;

    /// <summary>
    /// Признак долгого падения или дабл джампа
    /// </summary>
    [SerializeField] private bool _longFall;

    /// <summary>
    /// Радиус проверки пересечения с интерактивными объектами
    /// </summary>
    [SerializeField] private float _interactionRadius;
    /// <summary>
    /// Массив объектов, с которыми пересеклись
    /// </summary>
    private Collider2D[] _interactionObjects = new Collider2D[1];
    /// <summary>
    /// На пересечение с каким слоем будет производиться проверка
    /// </summary>
    [SerializeField] private LayerMask _interactionLayerMask;
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

    ////// COMPONENTS ///////
    private Rigidbody2D _rigidbody;
    private Animator _animator;

    ////// ANIMATION ///////
    private static readonly int _isGround = Animator.StringToHash("isGround");
    private static readonly int _isRunning = Animator.StringToHash("isRunning");
    private static readonly int _verticalVelocity = Animator.StringToHash("verticalVelocity");
    private static readonly int _hitTrigger = Animator.StringToHash("hitTrigger");

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _isGrounded = IsGrounded();
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

        FallParticles();
    }

    /// <summary>
    /// Подсчет ускорения по оси Y
    /// </summary>
    /// <returns></returns>
    private float CelculateYVelocity()
    {
        var retYVelocity = _rigidbody.velocity.y;

        // сброс двойного прыжка когда приземлились
        if (_isGrounded)
        {
            _allowDoubleJump = true;
        }

        // признак прыжка
        var isJumping = _direction.y > 0;

        // если прыгнули - придаем импульс направленный вверх
        if (isJumping)
        {
            retYVelocity = CalculateJumpVelocity(retYVelocity);
        }
        else if (_rigidbody.velocity.y > 0) // если движемся вверх (находимся в прыжке)
        {
            // уменьшаем импульс в 2 раза
            retYVelocity *= 0.5f;
        }
        
        if (!_isGrounded && !_longFall && _rigidbody.velocity.y <= -12.0f)
        {
            _longFall = true;
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
            _longFall = true;
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
        _animator.SetTrigger(_hitTrigger);

        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _damageJumpSpeed);
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

    /// <summary>
    /// Анимация партиклов приземления
    /// </summary>
    public void FallParticles()
    {
        // если падали и приземлились
        if (_isGrounded && _longFall)
        {
            _fallParticles.Process();
            _longFall = false;
        }
    }

    private void OnDrawGizmos()
    {
        //Debug.DrawRay(transform.position, Vector2.down, IsGrounded() ? Color.green : Color.red);
        //Gizmos.color = IsGrounded() ? Color.green : Color.red;
        //Gizmos.DrawSphere(transform.position, 0.3f);
    }
}

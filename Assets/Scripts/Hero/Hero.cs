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
    /// Компонент проверки соприкосновения с землей
    /// </summary>
    [SerializeField] private GroundCheck _groundCheck;

    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = new Vector2(_direction.x * _speed, _rigidbody.velocity.y);

        // признак прыжка
        var isJumping = _direction.y > 0;

        // если прыгнули - придаем импульс направленный вверх
        if(isJumping)
        {
            if (IsGrounded())
            {
                // придаем вектор силы вверх
                _rigidbody.AddForce(Vector2.up * _jumpSpeed, ForceMode2D.Impulse);
            }
        }
        else if (_rigidbody.velocity.y > 0) // если движемся вверх (находимся в прыжке)
        {
            // уменьшаем импульс в 2 раза
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y * 0.5f);
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


    private void OnDrawGizmos()
    {
        //Debug.DrawRay(transform.position, Vector2.down, IsGrounded() ? Color.green : Color.red);
        Gizmos.color = IsGrounded() ? Color.green : Color.red;
        Gizmos.DrawSphere(transform.position, 0.3f);
    }
}

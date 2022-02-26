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
    [SerializeField] private float _speed = 4;

    /// <summary>
    /// Задание вектора движения героя
    /// </summary>
    /// <param name="direction"></param>
    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }

    private void Update()
    {
        if (_direction.x != 0 || _direction.y != 0)
        {
            var delta = _direction * _speed * Time.deltaTime;
            var newXpos = transform.position.x + delta.x;
            var newYpos = transform.position.y + delta.y;

            transform.position = new Vector3(newXpos, newYpos, transform.position.z);
        }
    }
}

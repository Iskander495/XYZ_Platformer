using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Components.Movement
{
    public class HorizontalPointMoving : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private Transform[] _points;

        /// <summary>
        /// Порог вектора для приближения к точке
        /// </summary>
        [SerializeField] private float _treshold = 1f;

        private int _destinationPointIndex;
        private Rigidbody2D _rigidbody;
        private Vector2 _currentPosition;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            StartCoroutine(Moving());
        }

        private void Update()
        {
        }

        private IEnumerator Moving()
        {
            while (enabled)
            {
                if (IsOnPoint())
                {
                    _destinationPointIndex = (int)Mathf.Repeat(_destinationPointIndex + 1, _points.Length);
                }

                var direction = _points[_destinationPointIndex].position - transform.position;
                direction.y = 0;
                _rigidbody.velocity = direction.normalized * _speed;

                yield return null;
            }
        }

        private bool IsOnPoint()
        {
            // если длинна вектора меньше порогового значения - значит мы на месте
            return (_points[_destinationPointIndex].position - transform.position).magnitude < _treshold;
        }

        private void OnDestroy()
        {
            StopAllCoroutines();            
        }
    }
}
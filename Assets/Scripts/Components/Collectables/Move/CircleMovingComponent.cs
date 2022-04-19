using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Components.Collectables.Move
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CircleMovingComponent : MonoBehaviour
    {
        [SerializeField] private float _radius;

        private Rigidbody2D _rigidbody;
        private Vector2 _position;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _position = transform.position;
        }

        private void Update()
        {
            var term = Time.time;

            var x = Mathf.Sin(term) * _radius / 100;
            var y = Mathf.Cos(term) * _radius / 100;

            _position.x += x;
            _position.y += y;

            _rigidbody.MovePosition(_position);
        }
    }
}
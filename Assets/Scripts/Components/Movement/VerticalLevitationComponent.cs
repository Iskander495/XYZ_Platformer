using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Components.Movement
{
    public class VerticalLevitationComponent : MonoBehaviour
    {
        [SerializeField] private float _frequency = 1f;
        [SerializeField] private float _amplitude = 1f;
        [SerializeField] private bool _randomize;

        private Rigidbody2D _rigidbody;
        private float _originalY;

        private float _seed;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _originalY = _rigidbody.position.y;

            if (_randomize)
                _seed = Random.value * Mathf.PI * 2;
        }

        private void Update()
        {
            var position = _rigidbody.position;
            position.y = _originalY + Mathf.Sin(_seed + Time.time * _frequency) * _amplitude;

            _rigidbody.MovePosition(position);
        }
    }
}
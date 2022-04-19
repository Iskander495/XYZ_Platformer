using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Components.Weapons
{
    public class BaseProjectTile : MonoBehaviour
    {
        [SerializeField] protected float _speed;
        [SerializeField] protected bool _invertX;

        protected Rigidbody2D _rigidbody;
        protected int _direction;

        protected bool _isStopped = false;

        protected virtual void Start()
        {
            if (_isStopped) return;

            _isStopped = false;
            var mod = _invertX ? -1 : 1;

            _direction = mod * transform.lossyScale.x > 0 ? 1 : -1;
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public void Stop()
        {
            _rigidbody.velocity = Vector2.zero;
            _isStopped = true;
        }
    }
}
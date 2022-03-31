using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Components.Camera
{
    public class ShowTargetComponent : MonoBehaviour
    {
        /// <summary>
        /// Куда будем смотреть
        /// </summary>
        [SerializeField] private Transform _target;

        [SerializeField] private CameraStateController _controller;

        [SerializeField] private float _delay = 0.5f;

        private void OnValidate()
        {
            if (_controller == null)
                _controller = FindObjectOfType<CameraStateController>();
        }

        public void ShowTarget()
        {
            _controller.SetPosition(_target.position);
            _controller.SetState(true);

            Invoke(nameof(MoveBack), _delay);
        }

        private void MoveBack()
        {
            _controller.SetState(false);
        }
    }
}
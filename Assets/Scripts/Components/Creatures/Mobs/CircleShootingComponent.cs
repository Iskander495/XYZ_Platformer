using Model.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Components.Creatures.Mobs
{
    public class CircleShootingComponent : BaseAttack
    {
        [SerializeField] private GameObject[] _tileProjects;
        [SerializeField] private float _radius = 1f;
        [SerializeField] private float _angleShift = 0;
        [SerializeField] private float _speed = 1f;

        private Vector2[] _positions;
        private Vector2[] _vectors;

        private void Awake()
        {

        }

        private void Update()
        {
            CalculatePositions();
        }

        private void CalculatePositions()
        {
            _positions = new Vector2[_tileProjects.Length];
            _vectors = new Vector2[_tileProjects.Length];

            // полный круг, деленный на количество элементов. Шаг расстановки элементов
            var step = 2 * Mathf.PI / _tileProjects.Length;

            Vector2 containerPosition = transform.position;

            for (var i = 0; i < _tileProjects.Length; i++)
            {
                var angle = step * i + _angleShift;
                var pos = new Vector2(
                    Mathf.Cos(angle) * _radius,
                    Mathf.Sin(angle) * _radius
                );

                _positions[i] = containerPosition + pos;
                _vectors[i] = (Vector2)transform.position - _positions[i];
            }
        }

        [ContextMenu("Shoot")]
        public override void RangeAttack()
        {
            for (var i = 0; i < _tileProjects.Length; i++)
            {
                var newObj = Instantiate(_tileProjects[i], _positions[i], Quaternion.identity);
                var forceVector = ((Vector2)_positions[i] - (Vector2)transform.position) * _speed;
                newObj.GetComponent<Rigidbody2D>().AddForce(forceVector, ForceMode2D.Impulse);
                newObj.SetActive(true);
            }
        }

        public override void MeleeAttack()
        {
            throw new System.NotImplementedException();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            CalculatePositions();
        }

        private void OnDrawGizmosSelected()
        {
            for(var i = 0; i < _tileProjects.Length; i++) 
            {
                UnityEditor.Handles.DrawDottedLine(transform.position, _positions[i], 1 );
            }
        }
#endif
    }
}
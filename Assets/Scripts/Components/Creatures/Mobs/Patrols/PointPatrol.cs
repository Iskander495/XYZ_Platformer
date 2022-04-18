using Components.Creatures;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Components.Creatures.Patrols
{

    public class PointPatrol : Patrol
    {
        /// <summary>
        /// Точки патрулирования
        /// </summary>
        [SerializeField] private Transform[] _points;
        /// <summary>
        /// Порог вектора для приближения к точке
        /// </summary>
        [SerializeField] private float _treshold = 1f;

        private int _destinationPointIndex;

        private Creature _creature;

        private void Awake()
        {
            _creature = GetComponent<Creature>();
        }

        public override IEnumerator DoPatrol()
        {
            while (enabled)
            {
                if (IsOnPoint())
                {
                    _destinationPointIndex = (int)Mathf.Repeat(_destinationPointIndex + 1, _points.Length);
                }

                var direction = _points[_destinationPointIndex].position - transform.position;
                direction.y = 0;
                _creature.SetDirection(direction.normalized);

                yield return null;
            }
        }

        private bool IsOnPoint()
        {
            // если длинна вектора меньше порогового значения - значит мы на месте
            return (_points[_destinationPointIndex].position - transform.position).magnitude < _treshold;
        }
    }
}
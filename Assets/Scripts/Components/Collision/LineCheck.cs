using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Components.Collision
{

    public class LineCheck : LayerCheck
    {
        [SerializeField] private Transform _target;

        private readonly RaycastHit2D[] _result = new RaycastHit2D[1];

        private void Update()
        {
            IsTouchingLayer = Physics2D.LinecastNonAlloc(transform.position, _target.position, _result, _layer) > 0;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            UnityEditor.Handles.DrawLine(transform.position, _target.position);
        }
#endif
    }
}
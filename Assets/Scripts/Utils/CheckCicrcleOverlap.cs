using Components;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class CheckCicrcleOverlap : MonoBehaviour
{
    [SerializeField] private float _radius = 1f;

    [SerializeField] private LayerMask _mask;

    [SerializeField] private string[] _tags;

    [SerializeField] private OnOverlapEvent _onOverlap;

    private Collider2D[] _interactionObjects = new Collider2D[10];

    public void Check()
    {
        // получаем количество пересечений
        var size = Physics2D.OverlapCircleNonAlloc(transform.position, _radius, _interactionObjects, _mask);

        var overlaps = new List<GameObject>();
        for (var i = 0; i < size; i++)
        {
            var result = _tags.Any(tag => _interactionObjects[i].CompareTag(tag));
            if (result)
            {
                _onOverlap?.Invoke(_interactionObjects[i].gameObject);
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.color = HandlesUtils.TransparentRed;

        Handles.DrawSolidDisc(transform.position, Vector3.forward, _radius);
    }
#endif

    [Serializable]
    public class OnOverlapEvent : UnityEvent<GameObject>
    {
    }
}

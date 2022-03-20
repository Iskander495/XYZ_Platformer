using Components;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CheckCicrcleOverlap : MonoBehaviour
{
    [SerializeField] private float _radius = 1f;

    public GameObject[] GetObjectsInRange(string[] _tags)
    {
        Collider2D[] _interactionObjects = new Collider2D[5];
        // получаем количество пересечений
        var size = Physics2D.OverlapCircleNonAlloc(transform.position, _radius, _interactionObjects);

        var overlaps = new List<GameObject>();
        for(var i = 0; i < size; i++)
        {
            if(_tags != null)
            {
                var go = _interactionObjects[i].gameObject;
                foreach (var tag in _tags) {
                    if (go.tag.Equals(tag))
                    {
                        overlaps.Add(go);
                    }
                }
            } else
            {
                overlaps.Add(_interactionObjects[i].gameObject);
            }
        }

        return overlaps.ToArray();
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.color = HandlesUtils.TransparentRed;

        Handles.DrawSolidDisc(transform.position, Vector3.forward, _radius);
    }
#endif
}

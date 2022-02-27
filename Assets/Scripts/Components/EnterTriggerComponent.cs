using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnterTriggerComponent : MonoBehaviour
{
    /// <summary>
    /// С каким тегом проверяем пересечение
    /// </summary>
    [SerializeField] private string _tag;

    [SerializeField] private UnityEvent[] _events;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag(_tag))
        {
            foreach (UnityEvent _event in _events)
            {
                _event.Invoke();
            }
        }
    }
}

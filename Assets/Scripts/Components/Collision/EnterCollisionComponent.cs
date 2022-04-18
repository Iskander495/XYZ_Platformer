using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Components.Collision
{
    public class EnterCollisionComponent : MonoBehaviour
    {
        /// <summary>
        /// С каким тегом проверяем пересечение
        /// </summary>
        [SerializeField] private string _tag;

        [SerializeField] private EnterEvent[] _events;

        [SerializeField] private EnterEvent[] _onOutEvents;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag(_tag))
            {
                foreach (EnterEvent _event in _events)
                {
                    _event.Invoke(collision.gameObject);
                }
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            foreach (EnterEvent _event in _onOutEvents)
            {
                _event.Invoke(collision.gameObject);
            }
        }
    }

    [Serializable]
    public class EnterEvent : UnityEvent<GameObject>
    {

    }
}
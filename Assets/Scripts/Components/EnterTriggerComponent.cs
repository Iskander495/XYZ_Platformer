using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Components
{
    public class EnterTriggerComponent : MonoBehaviour
    {
        /// <summary>
        /// С каким тегом проверяем пересечение
        /// </summary>
        [SerializeField] private string _tag;

        [SerializeField] private EnterEvent[] _events;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(_tag))
            {
                foreach (EnterEvent _event in _events)
                {
                    _event.Invoke(collision.gameObject);
                }
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Components.Collision
{
    public class EnterTriggerComponent : MonoBehaviour
    {
        /// <summary>
        /// С каким тегом проверяем пересечение
        /// </summary>
        [SerializeField] private string _tag;

        [SerializeField] private LayerMask _layer = ~0;

        [SerializeField] private EnterEvent[] _events;

        [SerializeField] private EnterEvent[] _onOutEvents;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.IsInLayer(_layer)) return;

            if (!string.IsNullOrEmpty(_tag) && !other.gameObject.CompareTag(_tag)) return;

            foreach (EnterEvent _event in _events)
            {
                _event.Invoke(other.gameObject);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            foreach (EnterEvent _event in _onOutEvents)
            {
                _event.Invoke(other.gameObject);
            }
        }
    }
}
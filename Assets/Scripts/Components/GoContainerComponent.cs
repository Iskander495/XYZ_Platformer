using Components.Collectables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Components
{
    /// <summary>
    /// Класс для выбрасывания вещей
    /// </summary>
    public class GoContainerComponent : MonoBehaviour
    {
        [SerializeField] private GameObject[] _gameObjects;

        [SerializeField] private DropEvent _onDrop;

        [ContextMenu("Drop")]
        public void Drop()
        {
            _onDrop.Invoke(_gameObjects);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Components
{
    public class SpawnComponent : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private GameObject _prefab;

        [ContextMenu("Spawn")]
        public void Spawn()
        {
            var newObj = Instantiate(_prefab, _target.position, Quaternion.identity);
            newObj.transform.localScale = _target.lossyScale;

            var animator = newObj.GetComponent<SpriteAnimaion>();
            animator?.SetClip(0);
        }
    }
}
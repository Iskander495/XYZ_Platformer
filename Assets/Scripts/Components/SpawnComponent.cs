using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Components
{
    public class SpawnComponent : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private GameObject _prefab;
        //[SerializeField] private bool _invertXScale;

        [SerializeField] private string _animationName;

        [ContextMenu("Spawn")]
        public void Spawn()
        {
            InstantiateAndAnimation("Attack");
        }

        public void Jump()
        {
            InstantiateAndAnimation("jump");
        }

        public void Process()
        {
            InstantiateAndAnimation(_animationName);
        }

        private void InstantiateAndAnimation(string animationName)
        {
            var newObj = Instantiate(_prefab, _target.position, Quaternion.identity);

            var scale = _target.lossyScale;
            //scale.x = _invertXScale ? 1 : -1;

            newObj.transform.localScale = scale;

            var animator = newObj.GetComponent<SpriteAnimation>();

            animator?.SetClip(animationName);
        }
    }
}
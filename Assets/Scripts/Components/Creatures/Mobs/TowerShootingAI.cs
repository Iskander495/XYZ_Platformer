using Components.Collision;
using Components.GameObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Components.Creatures
{
    public class TowerShootingAI : MonoBehaviour
    {
        [SerializeField] public LayerCheck _vision;

        [Header("Range")]
        [SerializeField] private SpawnComponent _rangeAttack;
        [SerializeField] private Cooldown _rangeCooldown;

        private static readonly int Range = Animator.StringToHash("range");

        protected Animator _animator;

        protected virtual void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        protected virtual void Update()
        {
            if (_vision.IsTouchingLayer)
            {
                if (_rangeCooldown.IsReady)
                {
                    RangeAttack();
                }
            }
        }

        public void RangeAttack()
        {
            _rangeCooldown.Reset();
            _animator.SetTrigger(Range);
        }

        public void OnRangeAttack()
        {
            _rangeAttack.Spawn();
        }
    }
}
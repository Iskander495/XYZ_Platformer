using Components.Collision;
using Components.GameObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Components.Creatures {
    public class ShootingTrapAI : MonoBehaviour
    {
        [SerializeField] private LayerCheck _vision;

        [Header("Melee")]
        [SerializeField] private CheckCicrcleOverlap _meleeAttack;
        [SerializeField] private LayerCheck _meleeCanAttack;
        [SerializeField] private Cooldown _meleeCooldown;

        [Header("Range")]
        [SerializeField] private SpawnComponent _rangeAttack;
        [SerializeField] private Cooldown _rangeCooldown;

        private static readonly int Melee = Animator.StringToHash("melee");
        private static readonly int Range = Animator.StringToHash("range");

        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if(_vision.IsTouchingLayer)
            {
                if(_meleeCanAttack.IsTouchingLayer)
                {
                    if(_meleeCooldown.IsReady)
                    {
                        // атакуем зубами
                        MeleeAttack();
                        return;
                    }
                }

                if(_rangeCooldown.IsReady)
                {
                    // атакуем пулей
                    RangeAttack();
                }
            }
        }

        private void MeleeAttack()
        {
            _meleeCooldown.Reset();
            _animator.SetTrigger(Melee);
        }

        private void RangeAttack()
        {
            _rangeCooldown.Reset();
            _animator.SetTrigger(Range);
        }

        public void OnMeleeAttack()
        {
            _meleeAttack.Check();
        }

        public void OnRangeAttack()
        {
            _rangeAttack.Spawn();
        }
    }
}
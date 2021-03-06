using Components.Collision;
using Components.GameObjects;
using Model.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Components.Creatures.Mobs
{
    public class PinkStarAi : BaseAI
    {
        [SerializeField] private BaseAttack _rangeAttack;
        [SerializeField] private LayerCheck _obstacleCheck;

        private Rigidbody2D _rigidbody;

        protected override void Awake()
        {
            base.Awake();
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            StartState(DoMovement());
        }

        private IEnumerator DoMovement()
        {
            while (enabled)
            {
                var randomDirection = Random.Range(-1, 2);
                var direction = transform.position;

                var action = Random.Range(1, 4);

                if (_canMeleeAttack)
                {
                    action = 1;
                }
                else if (_obstacleCheck.IsTouchingLayer)
                {
                    _creature.SetDirection(direction.normalized * -1);
                }
                else
                {
                    direction = new Vector2(randomDirection, 0);
                    _creature.SetDirection(direction);
                }

                switch (action)
                {
                    case 1:
                        yield return DoMeleeAttack();
                        break;
                    case 2:
                        yield return DoJump();
                        break;
                    case 3:
                        yield return DoRangeAttack();
                        break;
                }

                yield return new WaitForSeconds(1);
            }
        }

        private IEnumerator DoMeleeAttack()
        {
            _creature.Attack();

            yield return null;
        }

        private IEnumerator DoJump()
        {
            _rigidbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
            yield return null;
        }

        private IEnumerator DoRangeAttack()
        {
            _rangeAttack?.RangeAttack();
            
            yield return null;
        }
    }
}
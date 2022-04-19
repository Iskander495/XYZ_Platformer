using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

namespace Components.Creatures
{
    public class TowerController : MonoBehaviour
    {
        [SerializeField] private List<TowerShootingAI> _mobs;
        [SerializeField] private Cooldown _cooldown;

        private int _current;

        private void Start()
        {
            foreach(var mob in _mobs)
            {
                mob.enabled = false;

                var hp = mob.GetComponent<HealthComponent>();
                hp._onDie.AddListener(() => OnDie(mob));
            }
        }

        private void OnDie(TowerShootingAI item)
        {
            var index = _mobs.IndexOf(item);
            _mobs.Remove(item);
            if(index < _current)
            {
                _current--;
            }
        }

        private void Update()
        {
            if (_mobs.Count == 0)
            {
                enabled = false;
                Destroy(gameObject, 1f);
            }

            if(_mobs.Count == 0)
            {
                enabled = false;
                Destroy(gameObject, 1f);
            }

            var inVision = _mobs.Any(m => m._vision.IsTouchingLayer);

            if(inVision)
            {
                if(_cooldown.IsReady)
                {
                    _mobs[_current].RangeAttack();
                    _cooldown.Reset();
                    _current = (int)Mathf.Repeat(_current + 1, _mobs.Count);
                }
            }
        }
    }
}
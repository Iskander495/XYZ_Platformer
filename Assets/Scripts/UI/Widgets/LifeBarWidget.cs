using Components.Creatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Utils.Disposables;

namespace UI.Widgets
{
    class LifeBarWidget : MonoBehaviour
    {
        [SerializeField] private ProgressBarWidget _lifeBar;
        [SerializeField] private HealthComponent _healthComponent;

        private readonly CompositeDisposable _trash = new CompositeDisposable();
        private int _maxHp;

        private void Start()
        {
            if (_healthComponent == null)
                _healthComponent = GetComponentInParent<HealthComponent>();

            _maxHp = _healthComponent.Health;

            _trash.Retain(_healthComponent._onDie.Subscribe(OnDie));
            _trash.Retain(_healthComponent._onHealthChange.Subscribe(OnHpChanged));
        }

        private void OnDie()
        {
            Destroy(gameObject);
        }

        private void OnHpChanged(int hp)
        {
            var progress = (float)hp / _maxHp;
            _lifeBar.SetProgress(progress);
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}

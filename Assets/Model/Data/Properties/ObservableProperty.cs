using System;
using UnityEngine;
using Utils.Disposables;

namespace Model.Data.Properties {
    [Serializable]
    public class ObservableProperty<TPropertyType>
    {
        [SerializeField] protected TPropertyType _value;

        public delegate void OnPropertyChanged(TPropertyType newValue, TPropertyType oldValue);
        public event OnPropertyChanged OnChanged;

        public IDisposable Subscribe(OnPropertyChanged call)
        {
            OnChanged += call;
            return new ActionDisposable(() => OnChanged -= call);
        }

        public virtual TPropertyType Value
        {
            get => _value;
            set
            {
                var isEquals = _value.Equals(value);
                if (isEquals) return;

                var oldValue = _value;
                InvokeChangedEvent(_value, oldValue);
                _value = value;
            }
        }

        protected void InvokeChangedEvent(TPropertyType newValue, TPropertyType oldValue)
        {
            OnChanged?.Invoke(newValue, oldValue);
        }
    }
}
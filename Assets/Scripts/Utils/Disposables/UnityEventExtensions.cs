using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;

namespace Utils.Disposables
{
    public static class UnityEventExtensions
    {
        public static IDisposable Subscribe(this UnityEvent unityEvent, UnityAction call)
        {
            unityEvent.AddListener(call);

            return new ActionDisposable(() => unityEvent.RemoveListener(call));
        }

        public static IDisposable Subscribe<TType>(this UnityEvent<TType> unityEvent, UnityAction<TType> call)
        {
            unityEvent.AddListener(call);

            return new ActionDisposable(() => unityEvent.RemoveListener(call));
        }
    }
}

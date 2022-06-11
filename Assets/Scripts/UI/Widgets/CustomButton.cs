using UnityEngine;
using UnityEngine.UI;

namespace Components.UI.Widgets
{
    public class CustomButton : Button
    {
        [SerializeField] private GameObject _normal;
        [SerializeField] private GameObject _pressed;

        protected override void DoStateTransition(SelectionState state, bool instatnt)
        {
            base.DoStateTransition(state, instatnt);

            _normal.SetActive(state != SelectionState.Pressed);
            _pressed.SetActive(state == SelectionState.Pressed);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HeroInputReader : MonoBehaviour
{
    [SerializeField] private Hero _hero;

    public void OnMovement(InputAction.CallbackContext context)
    {
        var vector = context.ReadValue<Vector2>();
        _hero.SetDirection(vector);
    }

    public void OnVector2Movement(InputValue context)
    {
        var vector = context.Get<Vector2>();
        _hero.SetDirection(vector);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if(context.canceled)
        {
            _hero.Interact();
        }
    }
}

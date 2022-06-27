using Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class HeroInputReader : MonoBehaviour
{
    [SerializeField] private Hero _hero;

    // **************** Invoke Unity Events  **************** //
    public void OnMovementEvent(CallbackContext context)
    {
        //if (context.performed)
        {
            var vector = context.ReadValue<Vector2>();
            _hero.SetDirection(vector);
        }
    }

    public void OnInteractEvent(CallbackContext context)
    {
        if(context.canceled)
        {
            _hero.Interact();
        }
    }

    public void OnAttackEvent(CallbackContext context)
    {
        if (context.canceled)
        {
            _hero.Attack();
        }
    }

    public void OnThrowEvent(CallbackContext context)
    {
        if (context.canceled && context.duration <= 1f)
        {
            StartCoroutine(_hero.Throw());
        }
    }

    public void OnSuperThrowEvent(CallbackContext context)
    {
        if (context.canceled && context.duration > 1f)
        {
            _hero.SuperThrow();
        }
    }

    //public void OnUsePotion(CallbackContext context)
    //{
    //    if (context.canceled)
    //    {
    //        _hero.UsePotion();
    //    }
    //}

    public void OnNextItem(CallbackContext context)
    {
        if (context.performed)
        {
            _hero.NextItem();
        }
    }

    // ************** Broadcast/Send Messages  ************** //
    public void OnVector2Movement(InputValue context)
    {
        var vector = context.Get<Vector2>();
        _hero.SetDirection(vector);
    }

    public void OnInteract(InputValue contex)
    {
        if (contex.isPressed)
        {
            _hero.Interact();
        }
    }

    public void OnAttack(InputValue contex)
    {
        if (contex.isPressed)
        {
            _hero.Attack();
        }
    }

    public void OnThrow(InputValue context)
    {
        if(!context.isPressed)
        {
            //_hero.Throw();
            StartCoroutine(_hero.Throw());
        }
    }

    public void OnSuperThrow(InputValue context)
    {
        //if (!context.isPressed)
        {
            _hero.SuperThrow();
            //Debug.Log("SuperThrow");
        }
    }
}

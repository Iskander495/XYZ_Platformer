using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchColliderComponent : MonoBehaviour
{
    public void SwitchColliders(bool state)
    {
        var colliders = GetComponents<Collider2D>();

        foreach(var collider in colliders)
        {
            collider.enabled = state;
        }
    }
}

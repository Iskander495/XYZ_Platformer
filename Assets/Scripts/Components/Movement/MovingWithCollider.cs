using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Components.Movement
{
    public class MovingWithCollider : MonoBehaviour
    {
        void OnCollisionEnter2D(Collision2D coll)
        {
            coll.transform.parent = transform;
        }

        void OnCollisionExit2D(Collision2D coll)
        {
            coll.transform.parent = null;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Components.Creatures
{
    public abstract class Patrol : MonoBehaviour
    {
        public abstract IEnumerator DoPatrol();
    }
}
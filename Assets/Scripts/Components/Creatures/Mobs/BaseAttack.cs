using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Components.Creatures.Mobs
{
    public abstract class BaseAttack : MonoBehaviour
    {
        public abstract void RangeAttack();

        public abstract void MeleeAttack();
    }
}
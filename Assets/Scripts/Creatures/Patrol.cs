using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Patrol : MonoBehaviour
{
    public abstract IEnumerator DoPatrol();
}

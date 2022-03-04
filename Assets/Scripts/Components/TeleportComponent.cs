using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportComponent : MonoBehaviour
{
    [SerializeField] private Transform _destinationPosition;

    public void Teleport(GameObject _targetObject)
    {
        _targetObject.transform.position = _destinationPosition.position;
    }
}

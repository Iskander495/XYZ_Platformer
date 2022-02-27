using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowToPlayer : MonoBehaviour
{
    /// <summary>
    /// Объект, за которым будет следить камера
    /// </summary>
    [SerializeField] private GameObject _targetObject;

    private void LateUpdate()
    {
        transform.position = new Vector3(_targetObject.transform.position.x, transform.position.y, transform.position.z);
    }    
}

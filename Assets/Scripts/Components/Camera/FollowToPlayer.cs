using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowToPlayer : MonoBehaviour
{
    /// <summary>
    /// Объект, за которым будет следить камера
    /// </summary>
    [SerializeField] private GameObject _targetObject;

    [SerializeField] private float _damping;

    private void LateUpdate()
    {
        var target = new Vector3(_targetObject.transform.position.x, _targetObject.transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * _damping);
    }    
}

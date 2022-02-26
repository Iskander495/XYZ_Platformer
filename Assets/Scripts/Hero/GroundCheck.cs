using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    /// <summary>
    /// Признак пересечения с другим слоем
    /// </summary>
    public bool IsTouchingLayer;

    /// <summary>
    /// Слой, считающийся землей
    /// </summary>
    [SerializeField] private LayerMask _groundLayer;

    private Collider2D _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        IsTouchingLayer = _collider.IsTouchingLayers(_groundLayer);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        IsTouchingLayer = _collider.IsTouchingLayers(_groundLayer);
    }
}

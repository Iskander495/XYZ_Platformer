using UnityEngine;

public class LayerCheck : MonoBehaviour
{
    /// <summary>
    /// Признак пересечения с другим слоем
    /// </summary>
    public bool IsTouchingLayer;

    /// <summary>
    /// Слой, считающийся землей
    /// </summary>
    [SerializeField] private LayerMask _layer;

    private Collider2D _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        IsTouchingLayer = _collider.IsTouchingLayers(_layer);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        IsTouchingLayer = _collider.IsTouchingLayers(_layer);
    }
}

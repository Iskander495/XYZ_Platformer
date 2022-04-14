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
    [SerializeField] protected LayerMask _layer;
}

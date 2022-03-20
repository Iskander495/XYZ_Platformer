using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtension
{
    /// <summary>
    /// Проверка на пересечение с нужным слоем
    /// </summary>
    /// <param name="go"></param>
    /// <param name="layer"></param>
    /// <returns></returns>
    public static bool IsInLayer(this GameObject go, LayerMask layer)
    {
        return layer == (layer | 1 << go.layer);
    }
}

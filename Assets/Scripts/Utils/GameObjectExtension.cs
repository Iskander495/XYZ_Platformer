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

    public static TInterfaceType GetInterface<TInterfaceType>(this GameObject go) 
    {
        var components = go.GetComponents<Component>();
        foreach(var component in components)
        {
            if(component is TInterfaceType type)
            {
                return type;
            }
        }
        return default;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadWindow : MonoBehaviour
{
    [SerializeField] string _resource;
    [SerializeField] Canvas _canvas;

    public void Load()
    {
        var window = Resources.Load<GameObject>(_resource);
        Instantiate(window, _canvas.transform);
    }
}

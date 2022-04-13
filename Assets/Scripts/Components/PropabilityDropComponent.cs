﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PropabilityDropComponent : MonoBehaviour
{
    [SerializeField] private int _count;
    [SerializeField] private DropData[] _drop;
    [SerializeField] private DropEvent _onDropCalculated;


    public void CalculateDrop()
    {
        var itemsToDrop = new GameObject[_count];

        var itemCount = 0;
        var total = _drop.Sum(dropData => dropData.Propability);
        var sordetDrop = _drop.OrderBy(dropData => dropData.Propability);

        while (itemCount < _count)
        {
            var random = UnityEngine.Random.value * total;

            foreach(var dropData in sordetDrop)
            {
                if(dropData.Propability >= random)
                {
                    itemsToDrop[itemCount] = dropData.DropObject;

                    itemCount++;
                    break;
                }
            }
        }

        _onDropCalculated?.Invoke(itemsToDrop);
    }

    [Serializable]
    public class DropData
    {
        /// <summary>
        /// Выпадающий объект
        /// </summary>
        public GameObject DropObject;

        /// <summary>
        /// Вероятность
        /// </summary>
        [Range(0f, 100f)] public float Propability;
    }

    [Serializable]
    public class DropEvent: UnityEvent<GameObject[]>
    {

    }
}

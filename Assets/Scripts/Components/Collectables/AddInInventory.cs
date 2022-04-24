using Components;
using Model.Data;
using Model.Definitions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Components.Collectables
{
    public class AddInInventory : MonoBehaviour
    {
        [InventoryId] [SerializeField] private string _id;
        [SerializeField] private int _count;

        public void Add(GameObject go)
        {
            var hero = go.GetInterface<ICanAddInInventory>();
            hero?.AddInInventory(_id, _count);
        }
    }
}
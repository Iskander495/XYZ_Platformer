using Components;
using Model.Definitions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddInInventory : MonoBehaviour
{
    [InventoryId] [SerializeField] private string _id;
    [SerializeField] private int _count;

    public void Add(GameObject go)
    {
        var hero = go.GetComponent<Hero>();

        if (hero != null)
            hero.AddInInventory(_id, _count);
    }
}

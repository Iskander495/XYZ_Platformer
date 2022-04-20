using Model.Definitions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model.Data
{
    [Serializable]
    public class InventoryData
    {
        [SerializeField] private List<InventoryItemData> _inventory = new List<InventoryItemData>();

        public delegate void OnInventoryChanged(string id, int value);

        public OnInventoryChanged OnChanged;

        public void Add(string id, int value)
        {
            if (value <= 0) return;

            var itemDef = DefsFacade.I.Items.Get(id);
            if (itemDef.IsVoid) return;

            if (itemDef.IsStackable)
            {
                var item = GetItem(id);
                if (item == null)
                {
                    item = new InventoryItemData(id);
                    _inventory.Add(item);
                }
                item.Value += value;
            } else
            {
                var item = new InventoryItemData(id);
                item.Value = value;
                _inventory.Add(item);
            }

            OnChanged?.Invoke(id, value);
        }

        public void Remove(string id, int value)
        {
            var itemDef = DefsFacade.I.Items.Get(id);
            if (itemDef.IsVoid) return;

            var item = GetItem(id);
            if (item == null) return;

            item.Value -= value;

            if (item.Value <= 0)
                _inventory.Remove(item);

            OnChanged?.Invoke(id, value);
        }

        public void Remove(Guid id, int value)
        {
            var item = GetItem(id);
            if (item == null) return;

            item.Value -= value;

            if (item.Value <= 0)
                _inventory.Remove(item);

            OnChanged?.Invoke(item.Id, value);
        }

        public int Count(string id)
        {
            var count = 0;

            foreach(var item in _inventory)
            {
                if (item.Id == id)
                    count += item.Value;
            }

            return count;
        }

        public Guid? IsPresent(string id)
        {
            foreach(var item in _inventory)
            {
                if(item.Id.Equals(id))
                {
                    return item.guid;
                }
            }

            return null;
        }

        public InventoryItemData GetItem(Guid id)
        {
            foreach (var itemData in _inventory)
            {
                if (itemData.guid.Equals(id))
                {
                    return itemData;
                }
            }

            return null;
        }

        private InventoryItemData GetItem(string id)
        {
            foreach(var itemData in _inventory)
            {
                if(itemData.Id == id)
                {
                    return itemData;
                }
            }

            return null;
        }
    }

    [Serializable]
    public class InventoryItemData 
    {
        public Guid guid;
        [InventoryId] public string Id;

        public int Value;

        public InventoryItemData(string id)
        {
            guid = Guid.NewGuid();
            Id = id;
        }
    }
}
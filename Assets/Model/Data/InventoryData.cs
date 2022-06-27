using Model.Definitions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Model.Data
{
    [Serializable]
    public class InventoryData
    {
        [SerializeField] private List<InventoryItemData> _inventory = new List<InventoryItemData>();

        public List<InventoryItemData> Inventory => _inventory;

        public delegate void OnInventoryChanged(string id, int value);

        public OnInventoryChanged OnChanged;

        public InventoryItemData[] GetAll(params ItemTag[] tags)
        {
            var retValue = new List<InventoryItemData>();
            foreach(var item in _inventory)
            {
                var itemDef = DefsFacade.I.Items.Get(item.Id);
                var isAllRequirementsMet = tags.All(x => itemDef.HasTag(x));

                if (isAllRequirementsMet)
                    retValue.Add(item);
            }

            return retValue.ToArray();
        }

        public void Add(string id, int value)
        {
            if (value <= 0) return;

            var itemDef = DefsFacade.I.Items.Get(id);
            if (itemDef.IsVoid) return;

            if (itemDef.HasTag(ItemTag.Stackable))
            {
                AddToStack(id, value);
            } 
            else
            {
                AddNonStack(id, value);
            }

            OnChanged?.Invoke(id, value);
        }

        private void AddToStack(string id, int value)
        {
            var isFull = _inventory.Count >= DefsFacade.I.Player.InventorySize;

            var item = GetItem(id);
            if (item == null)
            {
                if (isFull) return;

                item = new InventoryItemData(id);
                _inventory.Add(item);
            }
            item.Value += value;
        }

        private void AddNonStack(string id, int value)
        {
            var itemLasts = DefsFacade.I.Player.InventorySize - _inventory.Count;
            value = Mathf.Min(itemLasts, value);

            for (var i = 0; i < value; i++)
            {
                var item = new InventoryItemData(id) { Value = 1 };
                _inventory.Add(item);
            }
        }

        public void Remove(string id, int value)
        {
            var itemDef = DefsFacade.I.Items.Get(id);
            if (itemDef.IsVoid) return;

            if(itemDef.HasTag(ItemTag.Stackable))
            {
                RemoveFromStack(id, value);
            } 
            else
            {
                RemoveNonStack(id, value);
            }

            OnChanged?.Invoke(id, value);
        }

        private void RemoveFromStack(string id, int value)
        {
            var item = GetItem(id);
            if (item == null) return;

            item.Value -= value;

            if (item.Value <= 0)
                _inventory.Remove(item);
        }

        private void RemoveNonStack(string id, int value)
        {
            for (int i = 0; i < value; i++)
            {
                var item = GetItem(id);
                if (item == null) return;
                _inventory.Remove(item);
            }
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

        public InventoryItemData GetItem(string id)
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
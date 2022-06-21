using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model.Definitions
{
    [CreateAssetMenu(menuName = "Defs/PlayerDef", fileName = "PlayerDef")]

    public class PlayerDef : ScriptableObject
    {
        [SerializeField] private int _inventorySize;
        [SerializeField] private int _maxHealth;

        public int InventorySize => _inventorySize;

        public int MaxHealth => _maxHealth;
    }
}
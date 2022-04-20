using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model.Definitions
{
    [CreateAssetMenu(menuName = "Defs/PlayerDef", fileName = "PlayerDef")]

    public class PlayerDef : ScriptableObject
    {
        [SerializeField] private int _inventorySize;

        public int InventorySize => _inventorySize;
    }
}
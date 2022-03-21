using Components;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    [Serializable]
    public class PlayerData
    {
        [SerializeField] private int _coins;
        [SerializeField] private int _hp;

        public int Coins { 
            get
            {
                return _coins;
            }
            set
            {
                _coins = value;
                Debug.Log("Coins: " + _coins);
            }
        }

        public int Hp
        {
            get
            {
                return _hp;
            }
            set
            {
                _hp = value;
                Debug.Log("HP: " + _hp);
            }
        }

        public List<Perk> Perks;
    }
}
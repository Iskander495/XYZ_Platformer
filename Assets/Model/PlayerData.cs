using Components;
using Components.Collectables;
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
        [SerializeField] private int _swords;
        
        [SerializeField] public List<Perk> _perks;

        public List<Perk> Perks
        {
            get
            {
                if (_perks == null) _perks = new List<Perk>();

                return _perks;
            }
            set
            {
                _perks = value;
            }
        }


        public int Swords
        {
            get
            {
                return _swords;
            }
            set
            {
                _swords = value;
                Debug.Log("Swords: " + _swords);
            }
        }

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
    }
}
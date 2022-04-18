using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Components.Collectables
{
    public class AddCoinsComponent : MonoBehaviour
    {
        [SerializeField] private int _score;

        private GameSession _session;

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
        }

        public void AddCoins()
        {
            //_session.Data.Coins += _score;

            var old = _session.GetValue<int>("Coins");
            _session.SetValue<int>("Coins", old + _score);
        }

        public int Count()
        {
            //return _session.Data.Coins;

            return _session.GetValue<int>("Coins");
        }

        public void DecreaseCoins(int count)
        {
            /*
            if (_session.Data.Coins < count)
                _session.Data.Coins = 0;
            else
                _session.Data.Coins -= count;
            */

            var old = _session.GetValue<int>("Coins");

            if(old < count)
                _session.SetValue<int>("Coins", 0);
            else
                _session.SetValue<int>("Coins", old - count);
        }
    }
}
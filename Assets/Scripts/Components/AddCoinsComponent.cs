using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Components
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
            _session.Data.Coins += _score;
        }

        public int Count()
        {
            return _session.Data.Coins;
        }

        public void DecreaseCoins(int count)
        {
            if (_session.Data.Coins < count)
                _session.Data.Coins = 0;
            else
                _session.Data.Coins -= count;
        }
    }
}
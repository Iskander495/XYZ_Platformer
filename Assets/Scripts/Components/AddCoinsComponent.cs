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
    }

}
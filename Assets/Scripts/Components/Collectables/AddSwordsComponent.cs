using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Components.Collectables
{
    public class AddSwordsComponent : MonoBehaviour
    {
        [SerializeField] private int _count;

        private GameSession _session;

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
        }

        public void AddSwords()
        {
            //_session.Data.Swords += _count;

            var old = _session.GetValue<int>("Swords");
            _session.SetValue<int>("Swords", old + _count);
        }

        public void SubtractSwords()
        {
            //_session.Data.Swords -= _count;

            var old = _session.GetValue<int>("Swords");

            _session.SetValue<int>("Swords", old - _count);
        }

        public int Count()
        {
            //return _session.Data.Swords;
            return _session.GetValue<int>("Swords");
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Model
{
    [Serializable]
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private PlayerData _data;

        public void SetValue<T>(string propname, object value)
        {
            _data.GetType()
                    .GetProperty(propname)?
                    .SetValue(_data, (T)value);

            UpdateUI();
        }

        public T GetValue<T>(string propname)
        {
            return (T)_data.GetType()
                    .GetProperty(propname)?
                    .GetValue(_data, null);
        }

        // UI Labels
        private GameObject _swordCount;
        private GameObject _coinsCount;
        private GameObject _hpCount;

        public void Awake()
        {
            LoadUIComponents();

            // если сессия уже существует в сцене (приехала из предыдущей сцены), эта сессия вторая, её нужно уничтожить
            if (IsSessionExit())
            {
                DestroyImmediate(gameObject);
            } 
            else // это первая сессия, сохраняем ее
            {
                DontDestroyOnLoad(this);
            }
        }

        private void LoadUIComponents()
        {
            // UI
            _swordCount = GameObject.Find("SwordCount");
            _coinsCount = GameObject.Find("CoinsCount");
            _hpCount = GameObject.Find("HPCount");
        }

        private bool IsSessionExit()
        {
            var sessions = FindObjectsOfType<GameSession>();

            foreach(var sess in sessions)
            {
                // если не равен текущему объекту, другие сессии существуют
                if (sess != this)
                    return true;
            }

            return false;
        }

        public void Save(int scene)
        {
            string json = JsonUtility.ToJson(this._data);
            File.WriteAllText($"session_{scene}.json", json);
        }

        public void Load(int scene)
        {
            LoadUIComponents();

            if (File.Exists($"session_{scene}.json"))
            {
                var json = File.ReadAllText($"session_{scene}.json");
                var pd = JsonUtility.FromJson<PlayerData>(json);

                this._data = pd;
            }

            UpdateUI();
            //this.Data.Coins = gs.Data.Coins;
            //this.Data.Perks = gs.Data.Perks;
        }

        private void UpdateUI()
        {
            _swordCount.GetComponent<UnityEngine.UI.Text>().text = _data.Swords.ToString();

            _coinsCount.GetComponent<UnityEngine.UI.Text>().text = _data.Coins.ToString();

            _hpCount.GetComponent<UnityEngine.UI.Text>().text = _data.Hp.ToString();
        }
    }
}
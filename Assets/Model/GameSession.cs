using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Model
{
    [Serializable]
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private PlayerData _data;

        public PlayerData Data => _data;

        public void Awake()
        {
            // если сессия уже существует в сцене (приехала из предыдущей сцены), эта сессия вторая, её нужно уничтожить
            if (IsSessionExit())
            {
                DestroyImmediate(gameObject);
            } else // это первая сессия, сохраняем ее
            {
                DontDestroyOnLoad(this);
            }
        }

        private bool IsSessionExit()
        {
            var sessions = FindObjectsOfType<GameSession>();

            foreach(var sess in sessions)
            {
                if (sess != this)
                    return true;
            }

            return false;
        }

        public void Save(int scene)
        {
            string json = JsonUtility.ToJson(this.Data);
            File.WriteAllText($"session_{scene}.json", json);
        }

        public void Load(int scene)
        {
            if (File.Exists($"session_{scene}.json"))
            {
                var json = File.ReadAllText($"session_{scene}.json");
                var pd = JsonUtility.FromJson<PlayerData>(json);

                this._data = pd;
            }
            //this.Data.Coins = gs.Data.Coins;
            //this.Data.Perks = gs.Data.Perks;
        }
    }
}
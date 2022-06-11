using Model.Data;
using Model.Definitions;
using System;
using System.IO;
using UnityEngine;

namespace Model
{
    [Serializable]
    public class GameSession : MonoBehaviour
    {
        [SerializeField] public PlayerData Data;

        public void SetValue<T>(string propname, object value)
        {
            Data.GetType()
                    .GetProperty(propname)?
                    .SetValue(Data, (T)value);

            UpdateUI();
        }

        public T GetValue<T>(string propname)
        {
            return (T)Data.GetType()
                    .GetProperty(propname)?
                    .GetValue(Data, null);
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

            Data.Inventory.OnChanged += OnInventoryChanged;
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
            string json = JsonUtility.ToJson(this.Data);
            File.WriteAllText($"session_{scene}.json", json);
        }

        public void Load(int scene)
        {
            LoadUIComponents();

            if (File.Exists($"session_{scene}.json"))
            {
                var json = File.ReadAllText($"session_{scene}.json");
                var pd = JsonUtility.FromJson<PlayerData>(json);

                //this.Data = pd;

                foreach(var item in pd.Inventory.Inventory)
                {
                    Data.Inventory.Add(item.Id, item.Value);
                }

                Data.HP = pd.HP;

                /*
                foreach (var defItem in DefsFacade.I.Items.ItemsForEditor) {
                    var item = Data.Inventory.GetItem(defItem.Id);
                    if(item != null)
                        Data.Inventory.OnChanged?.Invoke(item.Id, item.Value);
                }
                */
            }

            UpdateUI();
            //this.Data.Coins = gs.Data.Coins;
            //this.Data.Perks = gs.Data.Perks;
        }

        private void OnInventoryChanged(string id, int value)
        {
            UpdateUI();
        }


        private void UpdateUI()
        {
            _swordCount.GetComponent<UnityEngine.UI.Text>().text = Data.Inventory.Count("Sword").ToString();
            _coinsCount.GetComponent<UnityEngine.UI.Text>().text = Data.Inventory.Count("Coin").ToString();
            _hpCount.GetComponent<UnityEngine.UI.Text>().text = Data.Inventory.Count("HealthPotion").ToString();
        }
    }
}
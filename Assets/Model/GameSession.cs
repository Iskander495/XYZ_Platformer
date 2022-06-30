using Model.Data;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils.Disposables;

namespace Model
{
    [Serializable]
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private PlayerData _data;
        public PlayerData Data => _data;
        private PlayerData _save;

        private readonly CompositeDisposable _trash = new CompositeDisposable();
        public QuickInventoryModel QuickInventory { get; private set; }

        public void Awake()
        {
            LoadHud();

            // если сессия уже существует в сцене (приехала из предыдущей сцены), эта сессия вторая, её нужно уничтожить
            if (IsSessionExit())
            {
                //DestroyImmediate(gameObject);
                Destroy(gameObject);
            } 
            else // это первая сессия, сохраняем ее
            {
                Save(SceneManager.GetActiveScene().buildIndex + 1);
                InitModels();
                DontDestroyOnLoad(this);
            }

            _data.Inventory.OnChanged += OnInventoryChanged;
        }

        private void InitModels()
        {
            QuickInventory = new QuickInventoryModel(_data);
            _trash.Retain(QuickInventory);
        }

        private void LoadHud()
        {
            SceneManager.LoadScene("Hud", LoadSceneMode.Additive);
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
            if (File.Exists($"session_{scene}.json"))
            {
                var json = File.ReadAllText($"session_{scene}.json");
                var pd = JsonUtility.FromJson<PlayerData>(json);

                foreach(var item in pd.Inventory.Inventory)
                {
                    _data.Inventory.Add(item.Id, item.Value);
                }

                _data.HP = pd.HP;
            }
        }

        private void OnInventoryChanged(string id, int value)
        {
            
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Model.Definitions.Localization
{
    [CreateAssetMenu(menuName = "Defs/LocaleDef", fileName = "LocaleDef")]

    public class LocaleDef : ScriptableObject
    {
        //en https://docs.google.com/spreadsheets/d/e/2PACX-1vQmQrK3lcEd9XkIpcUlpac2HSnP4OsKoLnFeF0PwV_HoobdIQHIiHalr3pkp5j1qRpb_4qMdFsJBiou/pub?gid=0&single=true&output=tsv
        //ru https://docs.google.com/spreadsheets/d/e/2PACX-1vQmQrK3lcEd9XkIpcUlpac2HSnP4OsKoLnFeF0PwV_HoobdIQHIiHalr3pkp5j1qRpb_4qMdFsJBiou/pub?gid=1482957205&single=true&output=tsv
        //es https://docs.google.com/spreadsheets/d/e/2PACX-1vQmQrK3lcEd9XkIpcUlpac2HSnP4OsKoLnFeF0PwV_HoobdIQHIiHalr3pkp5j1qRpb_4qMdFsJBiou/pub?gid=261438283&single=true&output=tsv

        [SerializeField] private string _url;
        [SerializeField] private List<LocaleItem> _localeItems;

        private UnityWebRequest _request;

        public Dictionary<string, string> GetData()
        {
            var dict = new Dictionary<string, string>();
            foreach(var item in _localeItems)
            {
                dict.Add(item.Key, item.Value);
            }

            return dict;
        }

        [ContextMenu("Update locale")]
        public void LoadLocale()
        {
            if (_request != null) return;

            Debug.Log($"Start update locale ");

            _request = UnityWebRequest.Get(_url);
            _request.SendWebRequest().completed += OnDataLoaded;
        }

        private void OnDataLoaded(AsyncOperation operation)
        {
            if(operation.isDone)
            {
                var rows = _request.downloadHandler.text.Split('\n');

                Debug.Log($"Loaded {rows.Length} rows");

                _localeItems.Clear();
                foreach (var row in rows)
                {
                    AddLocaleItem(row);
                }
            }
        }

        private void AddLocaleItem(string row)
        {
            try
            {
                var columns = row.Split('\t');
                _localeItems.Add(new LocaleItem{ Key = columns[0], Value = columns[1] });
            } 
            catch (Exception e)
            {
                Debug.LogError(row);
                Debug.LogError(e);
                throw e;
            }
        }

        [Serializable]
        private class LocaleItem
        {
            public string Key;
            public string Value;
        }
    }
}
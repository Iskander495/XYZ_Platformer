using Components.UI.Windows.Localization;
using Model.Data.Properties;
using Model.Definitions.Localization;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Localization
{
    public class LocalizationManager
    {
        public readonly static LocalizationManager I;
        public string LocaleKey => _localeKey.Value;

        private StringPersistentProperty _localeKey = new StringPersistentProperty("en", "localization/current");
        private Dictionary<string, string> _localization;

        public event Action OnLocaleChanged;

        static LocalizationManager()
        {
            I = new LocalizationManager();
        }

        public LocalizationManager()
        {
            LoadLocale(_localeKey.Value);
        }

        internal string Localize(string key)
        {
            return _localization.TryGetValue(key, out var value) ? value : $"%%%{key}%%%";
        }

        private void LoadLocale(string localeToLoad)
        {
            var def = Resources.Load<LocaleDef>($"Locales/{localeToLoad}");
            _localization = def.GetData();
            _localeKey.Value = localeToLoad;
            OnLocaleChanged?.Invoke();
        }

        internal void SetLocale(string localeKey)
        {
            LoadLocale(localeKey);
        }
    }
}
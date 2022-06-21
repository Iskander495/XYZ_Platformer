using Components.UI;
using Model;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace UI.InGameMenu
{
    public class InGameMenuWindow : AnimatedWindow
    {
        private Action _closeAction;

        private float _defaultTimeScale;

        protected override void Start()
        {
            base.Start();

            _defaultTimeScale = Time.timeScale;
            Time.timeScale = 0;
        }

        private void OnDestroy()
        {
            Time.timeScale = _defaultTimeScale;
        }

        public void OnShowSettings()
        {
            WindowUtils.CreateWindow("UI/SettingsWindow");
        }

        public void OnExit()
        {
            SceneManager.LoadScene("MainMenu");

            var session = FindObjectOfType<GameSession>();
            Destroy(session.gameObject);
            //            _closeAction = () =>
            //            {
            //                Application.Quit();

            //#if UNITY_EDITOR
            //                UnityEditor.EditorApplication.isPlaying = false;
            //#endif
            //            };
            //            Close();
        }

        public override void OnCloseAnimationComplete()
        {
            base.OnCloseAnimationComplete();

            _closeAction?.Invoke();
        }
    }
}
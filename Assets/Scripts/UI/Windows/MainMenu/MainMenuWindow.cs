using Components.UI;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

public class MainMenuWindow : AnimatedWindow
{
    private Action _closeAction;

    public void OnShowSettings()
    {
        WindowUtils.CreateWindow("UI/SettingsWindow");
    }

    public void OnShowLanguage()
    {
        WindowUtils.CreateWindow("UI/LocalizationWindow");
    }

    public void OnStartGame()
    {
        _closeAction = () => { SceneManager.LoadScene("Level1"); };
        Close();
    }

    public void OnExit()
    {
        _closeAction = () => {
            Application.Quit();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        };
        Close();
    }

    public override void OnCloseAnimationComplete()
    {
        base.OnCloseAnimationComplete();

        _closeAction?.Invoke();
    }
}

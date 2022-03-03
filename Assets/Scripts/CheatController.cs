using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CheatController : MonoBehaviour
{
    [SerializeField] private CheatItem[] _cheats;

    [SerializeField] private float _timeToLiveInput;

    private float _inputTime;

    private string _currentInput;


    private void Awake()
    {
        Keyboard.current.onTextInput += OnTextInput;
    }

    private void OnDestroy()
    {
        Keyboard.current.onTextInput -= OnTextInput;
    }

    private void OnTextInput(char inputChar)
    {
        _currentInput += inputChar;
        _inputTime = _timeToLiveInput;

        FindAnyCheats();
    }

    private void Update()
    {
        if(_inputTime < 0)
        {
            _currentInput = string.Empty;
        } 
        else
        {
            _inputTime -= Time.deltaTime;
        }
    }

    private void FindAnyCheats()
    {
        foreach(CheatItem item in _cheats)
        {
            if(_currentInput.Equals(item.Name) && item.Action != null)
            {
                item.Action.Invoke();

                _currentInput = string.Empty;
            }
        }
    }
}

[Serializable]
public class CheatItem
{
    public string Name;
    public UnityEvent Action;
}
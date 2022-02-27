using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCoinsComponent : MonoBehaviour
{
    [SerializeField] private int _score;

    [SerializeField] private ScoreController _scoreController;

    public void AddScore()
    {
        _scoreController.IncrementScore(_score);
    }
}

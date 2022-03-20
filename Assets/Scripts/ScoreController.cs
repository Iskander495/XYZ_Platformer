using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    [SerializeField] private int _score = 0;

    public void Start()
    {
        ResetScore();
    }

    public int Score
    {
        get
        {
            return _score;
        }
        set
        {
            _score = value;
            
            Debug.Log(_score);
        }
    }

    public void IncrementScore(int scoreCount)
    {
        Score += scoreCount;
    }

    public void ResetScore()
    {
        Score = 0;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartLevel : MonoBehaviour
{
    [SerializeField] private ScoreController _scoreController;

    public void Restart()
    {
        var scene = SceneManager.GetActiveScene();

        _scoreController.ResetScore();

        SceneManager.LoadScene(scene.name);
    }
}

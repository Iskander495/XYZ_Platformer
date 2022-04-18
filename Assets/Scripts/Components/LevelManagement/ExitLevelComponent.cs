using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Components.LevelManagement
{
    public class ExitLevelComponent : MonoBehaviour
    {
        [SerializeField] private string _sceneName;

        public void Exit()
        {
            var session = FindObjectOfType<GameSession>();
            session.Save(SceneManager.GetActiveScene().buildIndex + 1);

            SceneManager.LoadScene(_sceneName);
        }
    }
}
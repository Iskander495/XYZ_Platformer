using Model;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartLevel : MonoBehaviour
{
    public void Restart()
    {
        var session = FindObjectOfType<GameSession>();
        //DestroyImmediate(session);
        Destroy(session);

        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}

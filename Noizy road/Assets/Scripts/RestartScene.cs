using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScene : MonoBehaviour
{
    public void Restart(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}

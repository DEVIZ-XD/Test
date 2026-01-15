using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelRestart : MonoBehaviour
{
    [SerializeField] private float restartDelay = 1.0f;
    private bool restarting;

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        if (restarting) return;
        restarting = true;
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManagerMenu : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Fungsi untuk keluar dari game
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}

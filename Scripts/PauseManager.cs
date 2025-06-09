using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseManager : MonoBehaviour
{
    public GameObject Pause;      // Panel yang muncul saat game di-pause
    public GameObject PausePanel;
    public GameObject OpsiPanel;
    public Button continueButton;      // Tombol "Lanjutkan"
    private bool isPaused = false;     // Status apakah game sedang pause atau tida

    float elapsedTime;
    public int minutes;
    public int seconds;

    public bool isLosing = false;

    public GameObject tutorialPanel;

    public EnemyPatrol enemyPatrol;


    void Start()
    {
        Pause.SetActive(false);

        isLosing = false;
    }

    void Update()
    {
        Timer();
        // Tekan tombol escape untuk mem-pause dan melanjutkan game
       KeyControl();
       
    }

    public void KeyControl()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isLosing == false && !isPaused)
        {
            PauseGame();
        }
        else if (Input.GetKeyDown(KeyCode.Escape)&& isPaused && !OpsiPanel.activeSelf)
        {
            ResumeGame();
        }

    }

    public void Timer()
    {
        elapsedTime += Time.deltaTime;
        minutes = Mathf.FloorToInt(elapsedTime / 60);
        seconds = Mathf.FloorToInt(elapsedTime % 60);
    }

    // Fungsi untuk mem-pause game
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;  // Menghentikan waktu (pause)
        Pause.SetActive(true);  // Tampilkan panel pause

        tutorialPanel.SetActive(false);
    }

    // Fungsi untuk melanjutkan game
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;  // Mengembalikan waktu ke normal
        Pause.SetActive(false);  // Sembunyikan panel pause
    }

    // Fungsi untuk kembali ke Main Menu
    public void GoToMainMenu(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1f;  
    }


}

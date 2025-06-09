using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int currentScore = 0;
    private int highScore = 0;

    public List<TMP_Text> currentScoreText; 
    public TMP_Text highScoreText;    

    public Slider timerSlider; 

    public float startTime = 180f;     
    public float remainingTime;      

    //private bool isTimerRunning = false;

    public EnemyPatrol enemyPatrol;
    PauseManager pauseManager;

    void Start()
    {
        // Load the high score from PlayerPrefs
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        pauseManager = FindObjectOfType<PauseManager>();

        remainingTime = startTime;
        //isTimerRunning = true;

        timerSlider.maxValue = startTime;  
        timerSlider.value = remainingTime;

        UpdateUI();
    }

    void Update()
    {
        CountdownTimer();
    }

    void CountdownTimer()
    {
        if (remainingTime > 0)
        {
            if(pauseManager.minutes < 3)
            {
                remainingTime -= Time.deltaTime;
            }
            else
            {
                remainingTime -= 4*Time.deltaTime;
            }
             
            UpdateTimerUI();
        }
        else if(remainingTime < 0)
        {
            
            GameOver();
        }
    }

    public void AddTime(float timeToAdd)
    {
        remainingTime += timeToAdd;
        UpdateTimerUI();
    }

    void UpdateTimerUI()
    {
        timerSlider.value = remainingTime; 
    }

    public void AddScore(int points)
    {
        currentScore += points;
        UpdateHighScore();
        UpdateUI();
    }

    private void UpdateHighScore()
    {
        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt("HighScore", highScore);
        }
    }

    private void UpdateUI()
    {
        for (int i = 0; i < currentScoreText.Count; i++) 
        {
            currentScoreText[i].text = "" + currentScore;
        }
        
        
        highScoreText.text = "High Score: " + highScore;
    }

    void GameOver()
    {
        StartCoroutine(enemyPatrol.LosePanel());
    }

    // Fungsi opsional untuk mereset high score
    public void ResetHighScore()
    {
        highScore = 0;
        PlayerPrefs.SetInt("HighScore", highScore);
        UpdateUI();
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }
}
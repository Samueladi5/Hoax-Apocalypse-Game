using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;
using System.Linq;
using UnityEngine.UI;

public class TypingManager : MonoBehaviour
{
    [Header("Camera")]
    public CinemachineVirtualCamera virtualCamera; // Assign this in the Inspector
    public float zoomedInFOV = 30f; // Field of view when zoomed in
    public float normalFOV = 60f;   // Normal field of view
    public float zoomSpeed = 2f;    // Speed of zoom transition

    [Header("Wordlist")]
    public TextAsset jsonFile;
    public WordList wordList;
    public GameObject display; 
    public GameObject charPrefab;
    public bool isInteracting = false;

    public bool isZooming = false;
    private WordList ToRandomWord;
    private List<GameObject> charObjectList = new List<GameObject>(); 
    private int currentCharIndex = 0;
    string targetWord;

    InteractionManager interactionManager;
    PauseManager pauseManager;

    [Header("Scoring")]
    public ScoreManager scoreManager;
    private float timeToAdd = 5;




    void Start()
    {
        interactionManager = FindObjectOfType<InteractionManager>();
        pauseManager = FindObjectOfType<PauseManager>();
        wordList = JsonUtility.FromJson<WordList>(jsonFile.text);        
    }

    void DisplayPerChar()
    {

        foreach (char c in targetWord)
        {
            GameObject charGO = Instantiate(charPrefab, display.transform);

            TextMeshProUGUI charText = charGO.GetComponentInChildren<TextMeshProUGUI>();
            charText.text = c.ToString(); 

            charObjectList.Add(charGO);
        }
    }

    void Update()
    {

        
        if (interactionManager != null)
        {
            if (targetWord != null && currentCharIndex >= targetWord.Length)
            {
                
                interactionManager.EndInteraction();
                DestroyChild();
                targetWord = null;
            }
            else if (isInteracting)
            {
                DetectTyping();
            }
        }


    }

    void DetectTyping()
    {
        foreach (var charObject in charObjectList)
        {
            charObject.transform.localScale = Vector3.one; // Set the scale back to normal (1x)
        }

        // Increase the scale of the current character
        charObjectList[currentCharIndex].transform.localScale = Vector3.one * 1.5f; // Scale to 1.5x

        foreach (char c in Input.inputString)
        {
            Debug.Log(c);
            char targetChar = targetWord[currentCharIndex]; 

            TextMeshProUGUI charText = charObjectList[currentCharIndex].GetComponentInChildren<TextMeshProUGUI>();
            Image image = charObjectList[currentCharIndex].GetComponent<Image>();

            // Reset the scale of all characters


            if (char.ToLower(c) == char.ToLower(targetChar))
            {
               image.color = Color.green;
                //charText.color = Color.yellow;
                currentCharIndex++;
                SfxAudioClip.Instance.PlayAudioAtIndex(SfxAudioClip.AudioCategory.Typing, 0);
            }
            else
            {
                image.color = Color.red;
                SfxAudioClip.Instance.PlayAudioAtIndex(SfxAudioClip.AudioCategory.Typing, 1);
            }
        }
    }
    public void getRandomWord()
    {
        List<Word> filteredWords = GetFilteredWords();
        if (filteredWords.Count > 0 && targetWord ==null)
        {
            int randomIndex = Random.Range(0, filteredWords.Count);
            targetWord = filteredWords[randomIndex].word;
            DisplayPerChar();
        }
    }

    private List<Word> GetFilteredWords()
    {
        if (pauseManager.minutes < 2)
        {
            // 0-2 minutes: words with 5 characters
            return wordList.words.Where(word => word.char_count == 5).ToList();
        }
        else if (pauseManager.minutes >= 2 && pauseManager.minutes < 5)
        {
            // 2-5 minutes: words with 8 characters
            return wordList.words.Where(word => word.char_count == 8).ToList();
        }
        else
        {
            // Above 5 minutes: words with more than 8 characters
            return wordList.words.Where(word => word.char_count >= 8).ToList();
        }
    }

    public void DestroyChild()
    {
        foreach (Transform child in display.transform)
        {
            Destroy(child.gameObject);
        }
        charObjectList.Clear();
        currentCharIndex = 0;

        scoreManager.AddScore(50);
        if (!(scoreManager.remainingTime > scoreManager.startTime))
        {
            if (scoreManager.remainingTime + timeToAdd > scoreManager.startTime)
            {
                scoreManager.AddTime(scoreManager.startTime-scoreManager.remainingTime);
            }
            else
            {
                scoreManager.AddTime(timeToAdd);
            }
        }
    }



    // Coroutine to smoothly zoom in
    public IEnumerator ZoomInCamera()
    {
        isZooming=true;

        float initialFOV = virtualCamera.m_Lens.FieldOfView;
        float elapsedTime = 0f;

        while (elapsedTime < zoomSpeed)
        {
            virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(initialFOV, zoomedInFOV, elapsedTime / zoomSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        virtualCamera.m_Lens.FieldOfView = zoomedInFOV;
    }

    // Coroutine to smoothly reset zoom
    public IEnumerator ResetCameraZoom()
    {
        float initialFOV = virtualCamera.m_Lens.FieldOfView;
        float elapsedTime = 0f;

        while (elapsedTime < zoomSpeed)
        {
            virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(initialFOV, normalFOV, elapsedTime / zoomSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        virtualCamera.m_Lens.FieldOfView = normalFOV;
        isZooming = false;
    }
}

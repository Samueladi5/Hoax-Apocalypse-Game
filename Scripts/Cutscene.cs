using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class Cutscene : MonoBehaviour
{
    public AudioSource sfxAudioSource;
    public PlayableDirector playableDirector;
    public float changeTime;
    public string sceneName;
    private float musicVolume;

    private void Start()
    {
        LoadVolume();
        
        if (playableDirector != null && playableDirector.state != PlayState.Playing)
        {
            playableDirector.Play();
        }
    }

    void Update()
    {
        changeTime -= Time.deltaTime;
        if (changeTime <= 0 || Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    private void LoadVolume()
    {
        musicVolume = PlayerPrefs.GetInt("musicVolume", 10);
        sfxAudioSource.volume = Mathf.Clamp(musicVolume, 0.0f, 1.0f);
    }
}

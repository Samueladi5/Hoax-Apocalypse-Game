using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class SoundManager : MonoBehaviour
{
    public TMP_Text musicVolumeText;
    public TMP_Text sfxVolumeText;
    public Button musicButton;
    public Button sfxButton;

    public AudioSource musicSource;
    public AudioSource sfxSource;

    private int musicVolume = 10;
    private int sfxVolume = 10;

    public EventSystem eventSystem;

    private void Start()
    {
        LoadVolume();
        UpdateVolumeText();
        ApplyVolumeSettings();
    }

    private void Update()
    {
        GameObject currentSelected = eventSystem.currentSelectedGameObject;

        if (currentSelected == musicButton.gameObject || currentSelected == sfxButton.gameObject)
        {
            HandleVolumeInput(currentSelected == musicButton.gameObject);
        }
    }

    private void HandleVolumeInput(bool isMusic)
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            DecreaseVolume(isMusic);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            IncreaseVolume(isMusic);
        }

        ApplyVolumeSettings();
    }

    public void IncreaseVolume(bool isMusic)
    {
        if (isMusic && musicVolume < 10)
        {
            musicVolume++;
            PlayerPrefs.SetInt("musicVolume", musicVolume);
            musicVolumeText.text = "MUSIK : " + musicVolume;
        }
        else if (!isMusic && sfxVolume < 10)
        {
            sfxVolume++;
            PlayerPrefs.SetInt("sfxVolume", sfxVolume);
            sfxVolumeText.text = "SUARA : " + sfxVolume;
        }
    }

    public void DecreaseVolume(bool isMusic)
    {
        if (isMusic && musicVolume > 0)
        {
            musicVolume--;
            PlayerPrefs.SetInt("musicVolume", musicVolume);
            musicVolumeText.text = "MUSIK : " + musicVolume;
        }
        else if (!isMusic && sfxVolume > 0)
        {
            sfxVolume--;
            PlayerPrefs.SetInt("sfxVolume", sfxVolume);
            sfxVolumeText.text = "SUARA : " + sfxVolume;
        }
    }

    private void LoadVolume()
    {
        musicVolume = PlayerPrefs.GetInt("musicVolume", 10);
        sfxVolume = PlayerPrefs.GetInt("sfxVolume", 10);
    }

    private void ApplyVolumeSettings()
    {
        musicSource.volume = musicVolume / 10f;
        sfxSource.volume = sfxVolume / 10f;
    }

    private void UpdateVolumeText()
    {
        musicVolumeText.text = "MUSIK : " + musicVolume;
        sfxVolumeText.text = "SUARA : " + sfxVolume;
    }
}

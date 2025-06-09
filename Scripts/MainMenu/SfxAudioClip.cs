using System.Collections.Generic;
using UnityEngine;

public class SfxAudioClip : MonoBehaviour
{
    public static SfxAudioClip Instance;

    public AudioSource audioSource;

    // Enum untuk kategori audio
    public enum AudioCategory
    {
        Enemy,
        NPC,
        Walking,
        Typing,
        Lose,
    }

    // Dictionary untuk mengelola audio berdasarkan kategori
    private Dictionary<AudioCategory, AudioClip[]> audioClips;

    public AudioClip[] enemyClips;
    public AudioClip[] npcClips;
    public AudioClip[] walkingClips;
    public AudioClip[] typingClips;
    public AudioClip[] loseClips;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Menginisialisasi dictionary dengan array audio sesuai kategori
        audioClips = new Dictionary<AudioCategory, AudioClip[]>
        {
            { AudioCategory.Enemy, enemyClips },
            { AudioCategory.NPC, npcClips },
            { AudioCategory.Walking, walkingClips },
            { AudioCategory.Typing, typingClips },
            { AudioCategory.Lose, loseClips }
        };
    }

    public void PlayRandomAudio(AudioCategory category)
    {
        if (audioClips.ContainsKey(category) && audioClips[category].Length > 0)
        {
            AudioClip clip = audioClips[category][Random.Range(0, audioClips[category].Length)];
            audioSource.PlayOneShot(clip);
            Debug.Log("Playing audio: " + clip.name);
        }
        else
        {
            Debug.LogWarning("Audio clip untuk kategori " + category + " tidak ditemukan atau kosong!");
        }
    }

    public void PlayLoopingAudio(AudioCategory category)
    {
        if (category == AudioCategory.Walking && walkingClips.Length > 0)
        {
            audioSource.clip = walkingClips[Random.Range(0, walkingClips.Length)];
            audioSource.loop = true;
            audioSource.Play();
            Debug.Log("Playing looping audio: " + audioSource.clip.name);
        }
    }

    public void PlayAudioAtIndex(AudioCategory category, int index)
    {
        if (audioClips.ContainsKey(category) && index >= 0 && index < audioClips[category].Length)
        {
            AudioClip clip = audioClips[category][index];
            audioSource.PlayOneShot(clip);
            Debug.Log("Playing audio at index " + index + ": " + clip.name);
        }
        else
        {
            Debug.LogWarning("Audio clip tidak ditemukan pada kategori " + category + " di indeks " + index + "!");
        }
    }


    public void StopRandomAudio(AudioCategory category)
    {
        if (audioClips.ContainsKey(category) && audioClips[category].Length > 0)
        {
            audioSource.Stop();
        }
        else
        {
            Debug.LogWarning("Audio clip untuk kategori " + category + " tidak ditemukan atau kosong!");
        }
    }
    
}

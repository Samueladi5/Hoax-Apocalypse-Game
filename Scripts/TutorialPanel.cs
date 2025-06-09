using UnityEngine;

public class TutorialPanel : MonoBehaviour
{
    public GameObject tutorialPanel; // Panel tutorial yang akan ditampilkan
    private bool isPanelHidden = false; // Menyimpan status panel

    private void Start()
    {
        // Menampilkan panel tutorial dan mengatur timer
        tutorialPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    private void Update()
    {
        // Jika panel sudah disembunyikan, lewati Update
        if (isPanelHidden) return;

        // Mengurangi timer berdasarkan delta time

        // Jika waktu habis atau tombol spasi ditekan, sembunyikan panel
        if (Input.anyKeyDown)
        {
            tutorialPanel.SetActive(false);
            isPanelHidden = true; // Set status panel tersembunyi
            Time.timeScale = 1f;
        }
    }
}

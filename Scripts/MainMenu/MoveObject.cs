using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSelectionAnimation : MonoBehaviour
{
    public GameObject firstSelectedButton;  
    public float moveSpeed = 5f;             

    private Transform targetPosition;        
    private bool isMoving = false;           // Apakah objek sedang bergerak
    private GameObject lastSelectedButton;   // Tombol terakhir yang diseleksi

    public EventSystem eventSystem;          // Referensi ke EventSystem

    void Start()
    {
        // Set tombol pertama yang terpilih
        eventSystem.SetSelectedGameObject(firstSelectedButton);
        lastSelectedButton = firstSelectedButton;
    }

    void Update()
    {
        GameObject selectedButton = eventSystem.currentSelectedGameObject;

        // Jika tombol sedang dipilih
        if (selectedButton != null)
        {
            targetPosition = selectedButton.transform;
            isMoving = true;

            // Simpan tombol terakhir yang diseleksi
            lastSelectedButton = selectedButton;
        }
        else
        {
            // Jika tidak ada yang diseleksi, kembalikan ke tombol terakhir yang diseleksi
            eventSystem.SetSelectedGameObject(lastSelectedButton);
        }

        // Lerp ke posisi target jika sedang bergerak
        if (isMoving)
        {
            // Menggunakan Time.unscaledDeltaTime agar tetap berjalan saat pause
            transform.position = Vector3.Lerp(transform.position, targetPosition.position, moveSpeed * Time.unscaledDeltaTime);

            // Stop jika sudah dekat dengan posisi target
            if (Vector3.Distance(transform.position, targetPosition.position) < 0.01f)
            {
                isMoving = false;
            }
        }
    }

    // Fungsi untuk berpindah ke panel lain
    public void SwitchToPanel()
    {
        eventSystem.SetSelectedGameObject(firstSelectedButton);
        lastSelectedButton = firstSelectedButton;  // Reset ke tombol pertama
    }

    public void SwitchToPanel2()
    {
        eventSystem.SetSelectedGameObject(lastSelectedButton);
    }
}

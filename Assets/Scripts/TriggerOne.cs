using UnityEngine;
using TMPro; // Hapus atau ganti jika menggunakan UI Text biasa

public class TriggerText : MonoBehaviour
{
    // Tarik objek Text UI Anda ke kolom ini di Inspector
    public GameObject textObject; 

    private void Start()
    {
        // Memastikan teks mati di awal game
        if (textObject != null)
        {
            textObject.SetActive(false);
        }
    }

    // Terpicu saat objek lain (Player) masuk ke dalam Cube
    private void OnTriggerEnter(Collider other)
    {
        // Pastikan Player Anda memiliki Tag "Player"
        if (other.CompareTag("Player"))
        {
            textObject.SetActive(true);
        }
    }

    // Terpicu saat objek lain (Player) keluar dari Cube
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            textObject.SetActive(false);
        }
    }
}
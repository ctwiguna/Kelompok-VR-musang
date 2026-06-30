using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Raycast Settings")]
    public float interactDistance = 3f; // Jarak maksimum interaksi

    [Header("UI Reference")]
    public GameObject interactionUI;    // Objek Text Canvas "Press E"

    void Update()
    {
        // Membuat Ray dari posisi kamera menghadap ke depan
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // Menembakkan Raycast
        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            // Mengambil komponen secara dinamis yang menggunakan interface IInteractable
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            // TAMBAHKAN PENGECEKAN: Hanya jalan jika objek memiliki IInteractable DAN Zone sudah di-unlock
            if (interactable != null && TriggerZonePractice.IsZoneUnlocked)
            {
                // Munculkan tulisan UI karena objek bisa diinteraksi dan zone sudah terbuka
                interactionUI.SetActive(true);

                // Jika pemain menekan tombol E
                if (Input.GetKeyDown(KeyCode.E))
                {
                    // Menjalankan fungsi Interact() milik objek apa pun secara dinamis
                    interactable.Interact();
                }
            }
            else
            {
                // Jika melihat objek biasa ATAU zone belum di-unlock, sembunyikan UI
                SembunyikanUI();
            }
        }
        else
        {
            // Jika tidak melihat objek apa-apa, sembunyikan UI
            SembunyikanUI();
        }
    }

    private void SembunyikanUI()
    {
        if (interactionUI != null && interactionUI.activeSelf)
        {
            interactionUI.SetActive(false);
        }
    }
}
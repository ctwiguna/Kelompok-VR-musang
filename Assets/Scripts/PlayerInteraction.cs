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

            if (interactable != null)
            {
                // Munculkan tulisan UI karena objek bisa diinteraksi
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
                // Jika melihat objek biasa (bukan interactable), sembunyikan UI
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
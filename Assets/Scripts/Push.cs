using UnityEngine;

public class PushObject : MonoBehaviour, IInteractable
{
    public float pushForce = 500f; // Kekuatan dorongan

    public void Interact()
    {

    }

    void Update()
    {
        // Mendeteksi klik kiri mouse
        if (Input.GetMouseButtonDown(0))
        {
            // Membuat ray (sinar) dari posisi kamera menuju posisi kursor mouse
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Jika sinar mengenai objek ber-collider
            if (Physics.Raycast(ray, out hit))
            {
                // Memeriksa apakah objek yang diklik memiliki komponen Rigidbody
                Rigidbody rb = hit.collider.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    // Menghitung arah dorongan (menjauhi kamera/searah arah pandang kamera)
                    Vector3 pushDirection = Camera.main.transform.forward;

                    // Menghilangkan efek vertikal agar dorongan lebih stabil di lantai
                    pushDirection.y = 0;
                    pushDirection.Normalize();

                    // Memberikan gaya dorong ke Rigidbody
                    rb.AddForce(pushDirection * pushForce);
                }
            }
        }
    }
}
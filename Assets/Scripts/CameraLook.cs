using UnityEngine;

public class KameraLook : MonoBehaviour
{
    [Header("Sensivitas Mouse")]
    [SerializeField] private float sensitivitasMouse = 100f;

    [Header("Batasan Sudut Pandang (Vertikal)")]
    [SerializeField] private float batasAtas = -60f;
    [SerializeField] private float batasBawah = 60f;

    private float rotasiX = 0f;
    private float rotasiY = 0f;

    void Start()
    {
        // Mengunci kursor mouse di tengah layar agar tidak keluar dari jendela game
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // 1. Ambil input pergerakan mouse
        float mouseX = Input.GetAxis("Mouse X") * sensitivitasMouse * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivitasMouse * Time.deltaTime;

        // 2. Hitung rotasi untuk melihat ke atas/bawah (Sumbu X) dan kiri/kanan (Sumbu Y)
        rotasiX -= mouseY;
        rotasiY += mouseX;

        // 3. Batasi agar kamera tidak bisa menoleh ke atas/bawah sampai salto (terbalik)
        rotasiX = Mathf.Clamp(rotasiX, batasAtas, batasBawah);

        // 4. Terapkan rotasi secara lokal pada kamera
        transform.localRotation = Quaternion.Euler(rotasiX, rotasiY, 0f);

        // TIPS BONUS: Jika ingin membuka kunci kursor di tengah game (misal untuk kebutuhan Tugas 6), 
        // tekan tombol Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
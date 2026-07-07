using UnityEngine;

public class CameraLook : MonoBehaviour
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
        // Mengunci kursor mouse di tengah layar game
        Cursor.lockState = CursorLockMode.Locked;

        // AMAN: Mengambil arah hadap rotasi lokal saat ini menggunakan struktur Euler Angles bawaan
        Vector3 rotasiAwal = transform.localRotation.eulerAngles;
        
        // Memetakan sudut awal agar sinkron dengan pergerakan mouse pertama kali
        rotasiX = rotasiAwal.x;
        rotasiY = rotasiAwal.y;

        // Normalisasi sudut matematika Unity agar tidak melompat ke angka 350+ saat di-play
        if (rotasiX > 180) rotasiX -= 360;
        if (rotasiY > 180) rotasiY -= 360;
    }

    void Update()
    {
        // 1. Ambil input pergerakan mouse dari player
        float mouseX = Input.GetAxis("Mouse X") * sensitivitasMouse * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivitasMouse * Time.deltaTime;

        // 2. Akumulasikan pergerakan mouse ke variabel rotasi
        rotasiX -= mouseY; // Menoleh atas/bawah
        rotasiY += mouseX; // Menoleh kiri/kanan

        // 3. Kunci rotasi vertikal agar kamera tidak salto/terbalik ke belakang
        rotasiX = Mathf.Clamp(rotasiX, batasAtas, batasBawah);

        // 4. Terapkan rotasi secara lokal terhadap objek induk (Kereta)
        transform.localRotation = Quaternion.Euler(rotasiX, rotasiY, 0f);

        // Membuka kunci kursor jika menekan tombol Escape (untuk kebutuhan testing UI)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
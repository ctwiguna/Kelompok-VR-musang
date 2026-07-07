using UnityEngine;
using UnityEngine.Splines;

public class WahanaController : MonoBehaviour
{
    [Header("Referensi Jalur")]
    [SerializeField] private SplineContainer jalurSpline; 

    [Header("Pengaturan Kecepatan")]
    [SerializeField] private float kecepatanMaju = 2.0f;

    [Header("Pengaturan Posisi Rel")]
    [Tooltip("Gunakan ini untuk menaikkan kereta agar tidak ambles ke bawah rel")]
    [SerializeField] private float offsetY = 0.5f; // Ubah angka ini di Inspector sesuai tinggi kereta

    private float progress = 0f;

    void Update()
    {
        if (jalurSpline == null) return;

        float panjangJalur = jalurSpline.CalculateLength();
        progress += (kecepatanMaju / panjangJalur) * Time.deltaTime;

        if (progress > 1f)
        {
            progress = 0f; 
        }

        // 1. Ambil posisi asli dari titik Spline
        Vector3 posisiSpline = (Vector3)jalurSpline.EvaluatePosition(progress);

        // 2. Tambahkan posisi tinggi (Y) berdasarkan nilai Offset agar berada di atas rel
        posisiSpline.y += offsetY;

        // 3. Terapkan posisi baru ke kereta
        transform.position = posisiSpline;
        transform.forward = (Vector3)jalurSpline.EvaluateTangent(progress);
    }
}
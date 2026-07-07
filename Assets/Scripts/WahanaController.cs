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
    [SerializeField] private float offsetY = 0.5f; 

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

    // 2. Tambahkan posisi tinggi (Y) berdasarkan nilai Offset
    posisiSpline.y += offsetY;
    transform.position = posisiSpline;

    // 3. Ambil arah maju dari spline
    Vector3 arahSpline = (Vector3)jalurSpline.EvaluateTangent(progress);

    if (arahSpline != Vector3.zero)
    {
        // 4. Bikin kereta menghadap ke arah spline
        Quaternion rotasiSpline = Quaternion.LookRotation(arahSpline);

        // 5. KUNCI PERBAIKAN: Putar balik hasil rotasi spline sebesar 180 derajat di sumbu Y
        // agar mengompensasi model kamu yang aslinya kebalik.
        transform.rotation = rotasiSpline * Quaternion.Euler(0, 180, 0);
    }
}
}
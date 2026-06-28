using UnityEngine;
using UnityEngine.Splines; // Wajib diimport untuk mendeteksi fitur Spline

// "public class WahanaController : MonoBehaviour" artinya script ini adalah MonoBehaviour
public class WahanaController : MonoBehaviour
{
    [Header("Referensi Jalur")]
    [SerializeField] private SplineContainer jalurSpline; 

    [Header("Pengaturan Kecepatan")]
    [SerializeField] private float kecepatanMaju = 2.0f;

    private float progress = 0f;

    // Fungsi Update berjalan otomatis setiap frame karena script ini MonoBehaviour
    void Update()
    {
        if (jalurSpline == null) return;

        float panjangJalur = jalurSpline.CalculateLength();
        progress += (kecepatanMaju / panjangJalur) * Time.deltaTime;

        if (progress > 1f)
        {
            progress = 0f; 
        }

        // MonoBehaviour mengizinkan kita langsung mengubah posisi objek lewat 'transform'
        transform.position = (Vector3)jalurSpline.EvaluatePosition(progress);
        transform.forward = (Vector3)jalurSpline.EvaluateTangent(progress);
    }
}
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform cameraTransform;

    void Start()
    {
        // Mencari objek di Hierarchy yang bernama tepat "Camera"
        GameObject camObject = GameObject.Find("Camera");
        
        if (camObject != null)
        {
            cameraTransform = camObject.transform;
        }
        else
        {
            Debug.LogError("Waduh, objek bernama 'Camera' tidak ditemukan di Hierarchy!");
        }
    }

    void LateUpdate()
    {
        // Jika kamera ketemu, ikuti rotasinya
        if (cameraTransform != null)
        {
            transform.rotation = cameraTransform.rotation;
        }
    }
}
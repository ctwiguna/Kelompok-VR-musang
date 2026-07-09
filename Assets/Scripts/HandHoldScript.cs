using UnityEngine;

public class HandHoldSway : MonoBehaviour
{
    [Header("Sway Settings")]
    [SerializeField] private float swayAmount = 0.05f;   
    [SerializeField] private float maxSwayAmount = 0.1f; 
    [SerializeField] private float smoothAmount = 6f;    

    [Header("Bobbing Settings (Walk Effect)")]
    [SerializeField] private float bobSpeed = 10f;       
    [SerializeField] private float bobAmount = 0.02f;    

    private Vector3 initialPosition;
    private float timer = 0f;

    private void Start()
    {
        // JIKA posisi objek masih di tengah (0,0,0), beri jarak aman ke kanan bawah
        if (transform.localPosition == Vector3.zero)
        {
            transform.localPosition = new Vector3(0.3f, -0.25f, 0.5f);
        }

        initialPosition = transform.localPosition;
    }

    private void Update()
    {
        // Logika sway & bobbing tetap sama seperti sebelumnya
        float mouseX = -Input.GetAxis("Mouse X") * swayAmount;
        float mouseY = -Input.GetAxis("Mouse Y") * swayAmount;

        mouseX = Mathf.Clamp(mouseX, -maxSwayAmount, maxSwayAmount);
        mouseY = Mathf.Clamp(mouseY, -maxSwayAmount, maxSwayAmount);

        Vector3 targetSway = new Vector3(mouseX, mouseY, 0);

        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");
        Vector3 targetBob = Vector3.zero;

        if (Mathf.Abs(inputX) > 0.1f || Mathf.Abs(inputY) > 0.1f)
        {
            timer += Time.deltaTime * bobSpeed;
            targetBob.y = Mathf.Sin(timer) * bobAmount;
            targetBob.x = Mathf.Cos(timer / 2) * bobAmount;
        }
        else
        {
            timer = 0;
        }

        Vector3 finalPosition = initialPosition + targetSway + targetBob;
        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition, Time.deltaTime * smoothAmount);
    }
}
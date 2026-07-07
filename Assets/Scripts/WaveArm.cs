using UnityEngine;

public class WaveArm : MonoBehaviour
{
    [SerializeField] private float _waveAngle = 40f;
    [SerializeField] private float _waveSpeed = 4f;

    private Quaternion _startRotation;

    private void Awake()
    {
        _startRotation = transform.localRotation;
    }

    private void Update()
    {
        float angle = Mathf.Sin(Time.time * _waveSpeed) * _waveAngle;
        transform.localRotation = _startRotation * Quaternion.Euler(0, 0, angle);
    }
}
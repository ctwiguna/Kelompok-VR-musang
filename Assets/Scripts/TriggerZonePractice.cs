using TMPro;
using UnityEngine;

/// <summary>
/// Displays status text when the player enters or exits a trigger zone.
/// </summary>
public class TriggerZonePractice : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private TextMeshProUGUI _statusText;

    [Header("Boneka Controller")]
    [SerializeField] private BonekaController boneka;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (_statusText != null)
            _statusText.text = "SELAMAT DATANG DI ISTANA BONEKA";

        if (boneka != null)
            boneka.AktifkanInteraksi();

        Debug.Log("Player entered zone");
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (_statusText != null)
            _statusText.text = "LETS GO!";

        if (boneka != null)
            boneka.NonaktifkanInteraksi();

        Debug.Log("Player exited zone");
    }
}
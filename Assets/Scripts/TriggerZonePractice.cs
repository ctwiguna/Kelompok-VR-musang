using TMPro;
using UnityEngine;

/// <summary>
/// Displays status text and unlocks doll interaction when the player enters or exits a trigger zone.
/// </summary>
public class TriggerZonePractice : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private TextMeshProUGUI _statusText;

    // Properti statis agar bisa dicek dengan mudah oleh script lain tanpa perlu dragging di Inspector
    public static bool IsZoneUnlocked { get; private set; } = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Buka kunci interaksi secara global
        IsZoneUnlocked = true;

        if (_statusText != null) 
        { 
            _statusText.text = "Welcome to The Dolls Palace"; 
        }

        Debug.Log("Player entered zone - Doll interaction UNLOCKED");
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // OPSIONAL: Jika ingin mengunci kembali saat player keluar zone, aktifkan baris di bawah ini:
        // IsZoneUnlocked = false;

        if (_statusText != null) 
        { 
            _statusText.text = "LETS GO!"; 
        }

        Debug.Log("Player exited zone");
    }
}
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement; // PENTING: Wajib di-import untuk fitur berpindah Scene

public class TriggerZonePractice : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private TextMeshProUGUI _statusText;

    [Header("Barrier / Pintu Penghalang")]
    [Tooltip("Tarik GameObject Barrier / Pagar ke sini")]
    [SerializeField] private GameObject barrierObject; 

    [Header("Scene Teleport Settings")]
    [Tooltip("Ketik nama file Scene tujuan persis seperti di Build Settings")]
    [SerializeField] private string targetSceneName = "RumahBonekaMap"; 

    public static bool IsZoneUnlocked { get; private set; } = false;

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // CEK SYARAT 1: Jika belum punya tiket
        if (!TicketInteraction.HasTicket)
        {
            IsZoneUnlocked = false;

            if (_statusText != null) 
            { 
                _statusText.text = "Ambil tiket terlebih dahulu di loket!"; 
            }
            return;
        }

        // CEK SYARAT 2: Jika SUDAH punya tiket
        if (TicketInteraction.HasTicket)
        {
            IsZoneUnlocked = true;

            if (barrierObject != null && barrierObject.activeSelf)
            {
                barrierObject.SetActive(false); // Matikan barrier
            }

            if (_statusText != null) 
            { 
                _statusText.text = "Teleporting..."; 
            }

            // TELEPORT KE SCENE LAIN
            if (!string.IsNullOrEmpty(targetSceneName))
            {
                Debug.Log($"[TELEPORT] Memindahkan Player ke Scene: {targetSceneName}");
                SceneManager.LoadScene(targetSceneName);
            }
            else
            {
                Debug.LogError("[TELEPORT ERROR] Nama targetSceneName belum diisi di Inspector!");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (_statusText != null) 
        { 
            _statusText.text = "LETS GO!"; 
        }
    }
}
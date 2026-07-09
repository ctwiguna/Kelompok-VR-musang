using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTeleporter : MonoBehaviour
{
    [Header("Requirement Settings")]
    [Tooltip("Jika dicentang, player wajib mengambil tiket terlebih dahulu sebelum portal aktif.")]
    [SerializeField] private bool requireTicket = true;

    [Header("UI Reference")]
    [SerializeField] private TextMeshProUGUI statusText;

    [Header("Barrier Settings")]
    [Tooltip("Tarik GameObject Barrier / Pagar penghalang ke sini")]
    [SerializeField] private GameObject barrierObject;

    [Header("Scene Teleport Settings")]
    [Tooltip("Ketik nama file Scene tujuan persis sesuai di Build Profiles / Build Settings")]
    [SerializeField] private string targetSceneName = "RumahBonekaMap";

    private bool isTeleporting = false;

    private void OnTriggerStay(Collider other)
    {
        // 1. Validasi Tag Player dan Mencegah Eksekusi Berulang (Infinite Loop)
        if (!other.CompareTag("Player") || isTeleporting) return;

        // 2. Cek Syarat Tiket (Jika Diaktifkan)
        if (requireTicket && !TicketInteraction.HasTicket)
        {
            UpdateStatus("Ambil tiket terlebih dahulu di loket!");
            return;
        }

        // 3. Eksekusi Pembukaan Barrier & Teleportasi
        StartTeleportProcess();
    }

    private void StartTeleportProcess()
    {
        isTeleporting = true; // Kunci agar LoadScene hanya dipanggil 1x

        // Buka Barrier secara fisik jika ada
        if (barrierObject != null && barrierObject.activeSelf)
        {
            barrierObject.SetActive(false);
        }

        UpdateStatus("Teleporting...");

        // Validasi Nama Scene
        if (string.IsNullOrEmpty(targetSceneName))
        {
            Debug.LogError("[TELEPORT ERROR] Nama 'Target Scene Name' belum diisi di Inspector!", this);
            isTeleporting = false;
            return;
        }

        Debug.Log($"[TELEPORT] Memindahkan Player ke Scene: {targetSceneName}");
        
        // Pindah ke Scene Tujuan
        SceneManager.LoadScene(targetSceneName);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UpdateStatus("LETS GO!");
        }
    }

    private void UpdateStatus(string message)
    {
        if (statusText != null)
        {
            statusText.text = message;
        }
    }
}
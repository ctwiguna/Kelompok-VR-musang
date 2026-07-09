using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerZonePractice : MonoBehaviour
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
    [Tooltip("Ketik nama file Scene tujuan persis sesuai di Build Settings")]
    [SerializeField] private string targetSceneName = "RumahBonekaMap";

    [Tooltip("Centang ini jika ingin teleportasi langsung terjadi tanpa jeda teks 'LETS GO!'")]
    [SerializeField] private bool instantTeleport = false;

    [Tooltip("Berapa detik jeda waktu sebelum scene benar-benar berpindah? (Diabaikan jika Instant Teleport dicentang)")]
    [SerializeField] private float teleportDelay = 1.5f;

    public static bool IsZoneUnlocked { get; private set; } = false;
    private bool isTeleporting = false;

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player") || isTeleporting) return;

        if (requireTicket && !TicketInteraction.HasTicket)
        {
            IsZoneUnlocked = false;
            UpdateStatus("Ambil tiket terlebih dahulu di loket!");
            return;
        }

        StartTeleportProcess();
    }

    private void StartTeleportProcess()
    {
        isTeleporting = true; 
        IsZoneUnlocked = true;

        if (barrierObject != null && barrierObject.activeSelf)
        {
            if (barrierObject == this.gameObject)
            {
                if (TryGetComponent<Collider>(out Collider col)) col.enabled = false;
            }
            else
            {
                barrierObject.SetActive(false);
            }
        }

        if (instantTeleport)
        {
            ExecuteLoadScene();
        }
        else
        {
            StartCoroutine(TeleportRoutine());
        }
    }

    private IEnumerator TeleportRoutine()
    {
        UpdateStatus("LETS GO!");
        yield return new WaitForSeconds(teleportDelay);
        ExecuteLoadScene();
    }

    private void ExecuteLoadScene()
    {
        if (string.IsNullOrEmpty(targetSceneName))
        {
            Debug.LogError("[TELEPORT ERROR] Nama 'Target Scene Name' belum diisi di Inspector!", this);
            isTeleporting = false;
            return;
        }

        Debug.Log($"[TELEPORT] Memindahkan Player ke Scene: {targetSceneName}");
        SceneManager.LoadScene(targetSceneName);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !isTeleporting)
        {
            UpdateStatus("");
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
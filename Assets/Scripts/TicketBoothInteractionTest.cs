using UnityEngine;
using TMPro;

/// <summary>
/// Script pengetesan sederhana: raycast dari kamera, kalau kena objek dengan tag
/// targetTag, tampilkan "Press E", lalu ambil tiket saat E ditekan.
/// Berdiri sendiri, tidak terhubung ke sistem interaksi yang sudah ada.
/// </summary>
public class TicketBoothInteractionTest : MonoBehaviour
{
    [Header("Raycast Settings")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float interactDistance = 3f;
    [SerializeField] private string targetTag = "TicketBooth";

    [Header("UI Reference")]
    [SerializeField] private GameObject promptUI;
    [SerializeField] private TextMeshProUGUI statusText;

    [Header("Ticket Pickup (3D, held in hand)")]
    [SerializeField] private GameObject ticketModelPrefab;
    [SerializeField] private Vector3 heldLocalPosition = new Vector3(0.28f, -0.22f, 0.5f);
    [SerializeField] private Vector3 heldLocalEulerAngles = new Vector3(5f, -170f, 0f);
    [SerializeField] private Vector3 heldLocalScale = Vector3.one;

    private bool hasTicket;
    private GameObject heldTicketInstance;

    private void Awake()
    {
        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }

        SetPromptVisible(false);
    }

    private void Update()
    {
        if (cameraTransform == null)
        {
            return;
        }

        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, interactDistance)
            && hit.collider.CompareTag(targetTag))
        {
            SetPromptVisible(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                TakeTicket();
            }

            return;
        }

        SetPromptVisible(false);
    }

    private void TakeTicket()
    {
        Debug.Log("[TicketBoothInteractionTest] Tiket diambil (E ditekan).");

        if (!hasTicket)
        {
            hasTicket = true;
            SpawnHeldTicket();
        }

        SetStatus("Tiket sudah diambil");
    }

    private void SpawnHeldTicket()
    {
        if (ticketModelPrefab == null)
        {
            Debug.LogWarning("[TicketBoothInteractionTest] Ticket Model Prefab belum di-assign di Inspector, model 3D tidak bisa dimunculkan.");
            return;
        }

        if (cameraTransform == null)
        {
            Debug.LogWarning("[TicketBoothInteractionTest] cameraTransform null (Camera.main tidak ketemu), model tidak bisa di-parent ke kamera.");
            return;
        }

        if (heldTicketInstance != null)
        {
            Debug.Log("[TicketBoothInteractionTest] Model tiket sudah pernah di-spawn sebelumnya, dilewati.");
            return;
        }

        heldTicketInstance = Instantiate(ticketModelPrefab, cameraTransform);
        heldTicketInstance.transform.localPosition = heldLocalPosition;
        heldTicketInstance.transform.localEulerAngles = heldLocalEulerAngles;
        heldTicketInstance.transform.localScale = heldLocalScale;

        Debug.Log($"[TicketBoothInteractionTest] Model tiket '{heldTicketInstance.name}' berhasil di-spawn sebagai child dari '{cameraTransform.name}' " +
            $"pada localPosition={heldTicketInstance.transform.localPosition}, localEulerAngles={heldTicketInstance.transform.localEulerAngles}, localScale={heldTicketInstance.transform.localScale}. " +
            $"activeInHierarchy={heldTicketInstance.activeInHierarchy}");
    }

    private void SetPromptVisible(bool visible)
    {
        if (promptUI != null)
        {
            promptUI.SetActive(visible);
        }

        if (visible)
        {
            SetStatus("Interact");
        }
    }

    private void SetStatus(string message)
    {
        if (statusText != null)
        {
            statusText.text = message;
        }
    }
}

using UnityEngine;
using TMPro;

public class TicketInteraction : MonoBehaviour, IInteractable
{
    [Header("UI Reference")]
    [SerializeField] private GameObject promptUI;
    [SerializeField] private TextMeshProUGUI statusText;

    [Header("Ticket Setup (Auto-Adapt)")]
    [SerializeField] private GameObject ticketPrefab;
    [SerializeField] private Transform handHoldPoint; 
    [SerializeField] private Vector3 paksaSkalaTiket = new Vector3(1f, 1f, 1f);

    private bool hasTicket = false;

    private void Start()
    {
        if (promptUI != null) promptUI.SetActive(false);
        UpdateStatus("Cari Loket Tiket");
    }

    // Dipicu langsung oleh PlayerInteraction saat tombol E ditekan
    public void Interact()
    {
        if (hasTicket) return;
        TakeTicket();
    }

    // Mengizinkan script PlayerInteraction untuk menyalakan/mematikan UI bawaan tiket ini
    public void SetPromptActive(bool active)
    {
        if (hasTicket) return;
        if (promptUI != null) promptUI.SetActive(active);
    }

    private void TakeTicket()
    {
        hasTicket = true;
        if (promptUI != null) promptUI.SetActive(false);
        UpdateStatus("Tiket Berhasil Diambil!");

        if (ticketPrefab != null && handHoldPoint != null)
        {
            GameObject spawnedTicket = Instantiate(ticketPrefab, handHoldPoint);
            
            spawnedTicket.transform.localRotation = Quaternion.identity;
            spawnedTicket.transform.localScale = paksaSkalaTiket;
            spawnedTicket.transform.localPosition = Vector3.zero;

            MeshRenderer meshRenderer = spawnedTicket.GetComponentInChildren<MeshRenderer>();
            if (meshRenderer != null)
            {
                Vector3 centerOffset = spawnedTicket.transform.InverseTransformPoint(meshRenderer.bounds.center);
                spawnedTicket.transform.localPosition = -centerOffset;
            }

            if (spawnedTicket.TryGetComponent<Rigidbody>(out Rigidbody rb)) rb.isKinematic = true;
            if (spawnedTicket.TryGetComponent<Collider>(out Collider col)) col.enabled = false;
            
            Debug.Log("[SPAWN] Tiket berhasil di-spawn dengan koordinat adaptif.");
        }
    }

    private void UpdateStatus(string message)
    {
        if (statusText != null) statusText.text = message;
    }
}
using UnityEngine;
using TMPro;

public class TicketInteraction : MonoBehaviour
{
    [Header("Raycast Settings")]
    [SerializeField] private float interactDistance = 3f;
    [SerializeField] private string targetTag = "TicketBooth"; 

    [Header("UI Reference")]
    [SerializeField] private GameObject promptUI;
    [SerializeField] private TextMeshProUGUI statusText;

    [Header("Ticket Setup (Auto-Adapt)")]
    [SerializeField] private GameObject ticketPrefab;
    [SerializeField] private Transform handHoldPoint; 
    [SerializeField] private Vector3 paksaSkalaTiket = new Vector3(1f, 1f, 1f);

    private Camera mainCamera;
    private bool hasTicket = false;

    private void Start()
    {
        mainCamera = Camera.main;
        if (promptUI != null) promptUI.SetActive(false);
        UpdateStatus("Cari Loket Tiket");
    }

    private void Update()
    {
        if (hasTicket) return; 

        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            if (hit.collider.CompareTag(targetTag))
            {
                if (promptUI != null) promptUI.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    TakeTicket();
                }
                return;
            }
        }
        
        if (promptUI != null) promptUI.SetActive(false);
    }

    private void TakeTicket()
    {
        hasTicket = true;
        if (promptUI != null) promptUI.SetActive(false);
        UpdateStatus("Tiket Berhasil Diambil!");

        if (ticketPrefab != null && handHoldPoint != null)
        {
            // 1. Spawn tiket
            GameObject spawnedTicket = Instantiate(ticketPrefab, handHoldPoint);
            
            // 2. Set skala dan rotasi awal
            spawnedTicket.transform.localRotation = Quaternion.identity;
            spawnedTicket.transform.localScale = paksaSkalaTiket;
            spawnedTicket.transform.localPosition = Vector3.zero;

            // 3. LOGIKA ADAPTASI: Hitung otomatis posisi tengah berdasarkan bentuk 3D-nya
            MeshRenderer meshRenderer = spawnedTicket.GetComponentInChildren<MeshRenderer>();
            if (meshRenderer != null)
            {
                // Menghitung jarak antara titik pusat (pivot) palsu bawaan asset dengan pusat fisik aslinya
                Vector3 centerOffset = spawnedTicket.transform.InverseTransformPoint(meshRenderer.bounds.center);
                
                // Paksa posisi bergeser kebalikan dari offset tersebut agar posisinya auto-center!
                spawnedTicket.transform.localPosition = -centerOffset;
            }

            // 4. Matikan komponen fisik agar tidak jatuh
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
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Raycast Settings")]
    [SerializeField] private float interactDistance = 3f; 

    [Header("UI Reference (Untuk Boneka/Umum)")]
    [SerializeField] private GameObject interactionUI;    

    [Header("Hover Tint Settings")]
    [SerializeField] private Color hoverTargetColor = Color.green; 
    [SerializeField] [Range(0f, 1f)] private float tintStrength = 0.25f;

    private Renderer[] lastHoveredRenderers;
    private MaterialPropertyBlock propBlock;
    private TicketInteraction ticketScriptCache;

    private void Awake()
    {
        propBlock = new MaterialPropertyBlock();
        // Ambil cache script TicketInteraction yang ada di objek kamera ini sendiri
        ticketScriptCache = GetComponent<TicketInteraction>();
    }

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable == null)
            {
                interactable = hit.collider.GetComponentInParent<IInteractable>();
            }

            // DETEKSI KHUSUS LOKET TIKET BERDASARKAN TAG OBJEK YANG DITATAP
            bool isTicketBooth = hit.collider.CompareTag("TicketBooth");
            if (isTicketBooth && ticketScriptCache != null)
            {
                interactable = ticketScriptCache;
            }

            if (interactable != null)
            {
                // Khusus Boneka Karakter: Cek status zona
                if (hit.collider.TryGetComponent<RaycastTargetObject>(out var boneka) && !TriggerZonePractice.IsZoneUnlocked)
                {
                    ResetGlow();
                    SembunyikanSemuaUI();
                    return;
                }

                // Pengaturan UI Canvas
                if (isTicketBooth)
                {
                    SembunyikanUI(); // Sembunyikan UI "Press E" milik boneka
                    if (ticketScriptCache != null) ticketScriptCache.SetPromptActive(true); // Nyalakan UI bawaan tiketmu!
                }
                else if (hit.collider.GetComponent<PushObject>() != null || hit.collider.GetComponentInParent<PushObject>() != null)
                {
                    SembunyikanSemuaUI(); // Boneka dorong tidak pakai UI
                }
                else
                {
                    if (interactionUI != null) interactionUI.SetActive(true); // Boneka biasa pakai UI "Press E"
                    if (ticketScriptCache != null) ticketScriptCache.SetPromptActive(false);
                }

                // --- LOGIKA EMAS: AMBIL SEMUA GAMBER RENDERER DARI OBJEK YANG DITATAP ---
                Renderer[] currentRenderers = hit.collider.GetComponentsInChildren<Renderer>();
                if (currentRenderers.Length == 0)
                {
                    currentRenderers = hit.collider.GetComponentsInParent<Renderer>();
                }

                if (currentRenderers.Length > 0 && (lastHoveredRenderers == null || lastHoveredRenderers[0] != currentRenderers[0]))
                {
                    ResetGlow(); 
                    lastHoveredRenderers = currentRenderers;

                    Color blendedColor = Color.Lerp(Color.white, hoverTargetColor, tintStrength);

                    foreach (Renderer ren in lastHoveredRenderers)
                    {
                        if (ren != null)
                        {
                            ren.GetPropertyBlock(propBlock);
                            propBlock.SetColor("_Color", blendedColor);
                            propBlock.SetColor("_BaseColor", blendedColor); 
                            ren.SetPropertyBlock(propBlock);
                        }
                    }
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactable.Interact();
                }
            }
            else
            {
                ResetGlow();
                SembunyikanSemuaUI();
            }
        }
        else
        {
            ResetGlow();
            SembunyikanSemuaUI();
        }
    }

    private void ResetGlow()
    {
        if (lastHoveredRenderers != null)
        {
            foreach (Renderer ren in lastHoveredRenderers)
            {
                if (ren != null)
                {
                    ren.GetPropertyBlock(propBlock);
                    propBlock.SetColor("_Color", Color.white);
                    propBlock.SetColor("_BaseColor", Color.white);
                    ren.SetPropertyBlock(propBlock);
                }
            }
            lastHoveredRenderers = null;
        }
    }

    private void SembunyikanUI()
    {
        if (interactionUI != null && interactionUI.activeSelf)
        {
            interactionUI.SetActive(false);
        }
    }

    private void SembunyikanSemuaUI()
    {
        SembunyikanUI();
        if (ticketScriptCache != null) ticketScriptCache.SetPromptActive(false);
    }
}
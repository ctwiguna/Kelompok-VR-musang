using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Raycast Settings")]
    [SerializeField] private float interactDistance = 3f; 

    [Header("UI Reference (Untuk Boneka/Umum)")]
    [SerializeField] private GameObject interactionUI;    

    [Header("Hover Tint Settings (Khusus Boneka)")]
    [SerializeField] private Color hoverTargetColor = Color.green; 
    [SerializeField] [Range(0f, 1f)] private float tintStrength = 0.25f;

    [Header("Material Glow Loket (Tarik Material GlowSamar Ke Sini!)")]
    [SerializeField] private Material loketGlowMaterial;

    private Renderer[] lastHoveredRenderers;
    private Material[][] originalMaterialsMesh; 
    private TicketInteraction ticketScriptCache;

    private void Awake()
    {
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
                    SembunyikanUI(); 
                    if (ticketScriptCache != null) ticketScriptCache.SetPromptActive(true); 
                }
                else if (hit.collider.GetComponent<PushObject>() != null || hit.collider.GetComponentInParent<PushObject>() != null)
                {
                    SembunyikanSemuaUI(); 
                }
                else
                {
                    if (interactionUI != null) interactionUI.SetActive(true); 
                    if (ticketScriptCache != null) ticketScriptCache.SetPromptActive(false);
                }

                Renderer[] currentRenderers = hit.collider.GetComponentsInChildren<Renderer>();
                if (currentRenderers.Length == 0)
                {
                    currentRenderers = hit.collider.GetComponentsInParent<Renderer>();
                }

                if (currentRenderers.Length > 0 && (lastHoveredRenderers == null || lastHoveredRenderers[0] != currentRenderers[0]))
                {
                    ResetGlow(); 
                    
                    lastHoveredRenderers = currentRenderers;
                    originalMaterialsMesh = new Material[currentRenderers.Length][];

                    // JIKA TARGET ADALAH LOKET TIKET
                    if (isTicketBooth && loketGlowMaterial != null)
                    {
                        for (int i = 0; i < currentRenderers.Length; i++)
                        {
                            if (currentRenderers[i] != null)
                            {
                                originalMaterialsMesh[i] = currentRenderers[i].sharedMaterials;

                                // Tambahkan material GlowSamar kamu di atas material glTF bawaan loket
                                Material[] fakeGlowMaterials = new Material[originalMaterialsMesh[i].Length + 1];
                                for (int j = 0; j < originalMaterialsMesh[i].Length; j++)
                                {
                                    fakeGlowMaterials[j] = originalMaterialsMesh[i][j];
                                }
                                
                                // Taruh material buatanmu di lapisan terluar
                                fakeGlowMaterials[fakeGlowMaterials.Length - 1] = loketGlowMaterial;
                                currentRenderers[i].materials = fakeGlowMaterials;
                            }
                        }
                    }
                    // JIKA TARGET ADALAH BONEKA
                    else
                    {
                        for (int i = 0; i < currentRenderers.Length; i++)
                        {
                            if (currentRenderers[i] != null)
                            {
                                originalMaterialsMesh[i] = currentRenderers[i].sharedMaterials;
                                
                                Material[] dynamicMaterials = currentRenderers[i].materials;
                                for (int j = 0; j < dynamicMaterials.Length; j++)
                                {
                                    if (dynamicMaterials[j].HasProperty("_Color"))
                                    {
                                        Color origColor = dynamicMaterials[j].color;
                                        dynamicMaterials[j].color = Color.Lerp(origColor, hoverTargetColor, tintStrength);
                                    }
                                }
                                currentRenderers[i].materials = dynamicMaterials;
                            }
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
        if (lastHoveredRenderers != null && originalMaterialsMesh != null)
        {
            for (int i = 0; i < lastHoveredRenderers.Length; i++)
            {
                if (lastHoveredRenderers[i] != null && i < originalMaterialsMesh.Length && originalMaterialsMesh[i] != null)
                {
                    lastHoveredRenderers[i].materials = originalMaterialsMesh[i];
                }
            }
            lastHoveredRenderers = null;
            originalMaterialsMesh = null;
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
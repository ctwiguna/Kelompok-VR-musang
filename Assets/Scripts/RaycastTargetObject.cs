using UnityEngine;

/// <summary>
/// Represents an object that can be highlighted and interacted with by raycast, only after trigger zone is unlocked.
/// </summary>
public class RaycastTargetObject : MonoBehaviour, IInteractable
{
    [Header("Visual Reference")]
    [SerializeField] private Renderer _targetRenderer;

    [Header("Colors")]
    [SerializeField] private Color _normalColor = Color.white;
    [SerializeField] private Color _highlightColor = Color.yellow;
    [SerializeField] private Color _interactedColor = Color.green;

    [Header("Controller Hub Reference")]
    [SerializeField] private BonekaController _bonekaController;

    private bool isInteracted;

    private void Awake()
    {
        if (_targetRenderer == null)
        {
            _targetRenderer = GetComponent<Renderer>();
        }

        SetColor(_normalColor);
    }

    /// <summary>
    /// Changes the visual state when the object is looked at by raycast (Only works if zone is unlocked).
    /// </summary>
    public void SetLookedAt(bool isLookedAt)
    {
        // JIKA ZONE BELUM DILEWATI/DILETAK, JANGAN LAKUKAN HOVER EFFECT
        if (!TriggerZonePractice.IsZoneUnlocked)
        {
            SetColor(_normalColor); // Pastikan warna tetap normal
            return;
        }

        if (isInteracted)
        {
            return;
        }

        SetColor(isLookedAt ? _highlightColor : _normalColor);
    }

    /// <summary>
    /// Applies the interaction effect to this object (Only works if zone is unlocked).
    /// </summary>
    public void Interact()
    {
        // JIKA ZONE BELUM DILEWATI, TOMBOL E TIDAK AKAN BEKERJA
        if (!TriggerZonePractice.IsZoneUnlocked) return;
        if (isInteracted) return;

        isInteracted = true;
        SetColor(_interactedColor);
        Debug.Log(gameObject.name + " interacted");

        if (_bonekaController != null)
        {
            _bonekaController.AktifkanInteraksi();
        }
        else
        {
            Debug.LogError("Tolong seret objek yang punya script BonekaController ke kolom di Inspector Boneka A!");
        }
    }

    private void SetColor(Color color)
    {
        if (_targetRenderer == null)
        {
            return;
        }

        _targetRenderer.material.color = color;
    }
}
using UnityEngine;

/// <summary>
/// Represents an object that can be highlighted and interacted with by raycast.
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
    // Kita buat kolom manual agar bisa di-drag dari objek mana pun di Hierarchy
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
    /// Changes the visual state when the object is looked at by raycast.
    /// </summary>
    public void SetLookedAt(bool isLookedAt)
    {
        if (isInteracted)
        {
            return;
        }

        SetColor(isLookedAt ? _highlightColor : _normalColor);
    }

    /// <summary>
    /// Applies the interaction effect to this object.
    /// </summary>
    public void Interact()
    {
        if (isInteracted) return;

        isInteracted = true;
        SetColor(_interactedColor);
        Debug.Log(gameObject.name + " interacted");

        // Membuka gerbang interaksi pada script BonekaController pusat
        if (_bonekaController != null)
        {
            _bonekaController.AktifkanInteraksi();
            
            // Mengingat BonekaController bawaan kamu masih butuh mendeteksi tombol E di Update()-nya,
            // kita picu juga fungsi menarinya secara tidak langsung di sini agar berjalan instan saat E ditekan pada kamera.
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
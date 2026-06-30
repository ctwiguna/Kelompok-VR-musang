using TMPro;

using UnityEngine;



/// <summary>

/// Displays status text when the player enters or exits a trigger zone.

/// </summary>

public class TriggerZonePractice : MonoBehaviour

{

    [Header("UI Reference")]

    [SerializeField] private TextMeshProUGUI _statusText;

    private void OnTriggerEnter(Collider other)

    {
        if (!other.CompareTag("Player")) return;

        if (_statusText != null) { _statusText.text = "Welcome to The Dolls Palace"; }

        Debug.Log("Player entered zone");

    }



    private void OnTriggerExit(Collider other)

    {

        if (!other.CompareTag("Player")) return;



        if (_statusText != null) { _statusText.text = "LETS GO!"; }


        Debug.Log("Player exited zone");

    }

}
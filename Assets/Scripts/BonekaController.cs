using UnityEngine;

public class BonekaController : MonoBehaviour
{
    [Header("Daftar Animator Boneka")]
    public Animator[] bonekaAnimators;

    private bool bolehInteraksi = false;

    public void AktifkanInteraksi()
    {
        bolehInteraksi = true;
        
        // Panggil langsung fungsi menari di sini agar sekali tekan E di kamera, 
        // seluruh boneka di array langsung bergerak secara instan!
        PicuSemuaAnimasiDance(); 
    }

    public void NonaktifkanInteraksi()
    {
        bolehInteraksi = false;
    }

    private void Update()
    {
        if (!bolehInteraksi) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            PicuSemuaAnimasiDance();
        }
    }

    // Kita bungkus logikanya ke fungsi terpisah agar bisa diakses dari fungsi AktifkanInteraksi()
    private void PicuSemuaAnimasiDance()
    {
        foreach (Animator anim in bonekaAnimators)
        {
            if (anim != null)
            {
                anim.SetTrigger("Dance");
            }
        }

        bolehInteraksi = false;
    }
}
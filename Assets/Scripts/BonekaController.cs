using UnityEngine;

public class BonekaController : MonoBehaviour
{
    [Header("Daftar Animator Boneka")]
    public Animator[] bonekaAnimators;

    private bool bolehInteraksi = false;

    public void AktifkanInteraksi()
    {
        bolehInteraksi = true;
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
}
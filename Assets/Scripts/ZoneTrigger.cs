using UnityEngine;
using System.Collections;

public class ZoneTrigger : MonoBehaviour
{
    [Header("Pengaturan UI Teks")]
    public GameObject teksWorldSpace;
    public float durasiTeksMuncul = 3.5f;

    [Header("Pengaturan Audio")]
    public AudioSource audioSourceZona;
    // 👇 KITA TAMBAHKAN SLOT BARU DI SINI
    public AudioClip fileAudioMP3;

    private void Start()
    {
        if (teksWorldSpace != null)
            teksWorldSpace.SetActive(false);

        // Otomatis memasukkan file MP3 ke Audio Source saat game mulai
        if (audioSourceZona != null && fileAudioMP3 != null)
        {
            audioSourceZona.clip = fileAudioMP3;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Train"))
        {
            // Menggunakan FindObjectsOfType agar kompatibel dengan semua versi Unity
            AudioSource[] semuaAudio = Object.FindObjectsOfType<AudioSource>();
            
            // Mematikan suara dari zona lain satu per satu
            foreach (AudioSource audio in semuaAudio)
            {
                audio.Stop();
            }

            // Jalankan Audio Zona Baru
            if (audioSourceZona != null)
            {
                audioSourceZona.Play();
            }

            // Munculkan Teks
            if (teksWorldSpace != null)
            {
                StopAllCoroutines(); 
                StartCoroutine(MunculkanTeksSesaat());
            }
        }
    }
    
    IEnumerator MunculkanTeksSesaat()
    {
        teksWorldSpace.SetActive(true);
        yield return new WaitForSeconds(durasiTeksMuncul);
        teksWorldSpace.SetActive(false);
    }
}
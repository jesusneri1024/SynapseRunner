using UnityEngine;
using UnityEngine.SceneManagement;

public class KillOnTouchLaser : MonoBehaviour
{
    public GameObject deathUI;           // Asigna el panel de UI desde el Inspector
    public AudioClip deathSound;         // Asigna el sonido desde el Inspector
    private AudioSource audioSource;     // Se usará para reproducir el sonido
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        // Si no hay AudioSource en el objeto, lo crea
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Laser"))
        {
            // Detener movimiento del jugador
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true;
            }

            // Reproducir sonido de muerte
            if (deathSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(deathSound);
            }

            // Desactiva el generador de chunks
            GameObject generator = GameObject.Find("ChunkGenerator");
            if (generator != null)
            {
                generator.SetActive(false);
            }

            // Mostrar UI de muerte
            if (deathUI != null)
            {
                deathUI.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }


}

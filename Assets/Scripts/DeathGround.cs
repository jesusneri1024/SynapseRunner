using UnityEngine;
using UnityEngine.SceneManagement;

public class KillPlayerOnTouch : MonoBehaviour
{
    public GameObject deathUI; // Asigna el panel de UI desde el inspector
    public AudioClip deathSound;
    private AudioSource audioSource;


    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Desactiva al jugador (puedes cambiar esto por una animación de muerte)
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true;
            }

            // Reproduce el sonido
            if (deathSound != null)
            {
                audioSource.PlayOneShot(deathSound);
            }

            // Desactiva el generador de chunks
            GameObject generator = GameObject.Find("ChunkGenerator");
            if (generator != null)
            {
                generator.SetActive(false);
            }

            // Muestra la UI de muerte
            if (deathUI != null)
            {
                deathUI.SetActive(true);


                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

            }
        }
    }

    // Llamar esta función desde el botón de reiniciar
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

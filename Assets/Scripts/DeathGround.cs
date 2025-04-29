using UnityEngine;
using UnityEngine.SceneManagement;

public class KillPlayerOnTouch : MonoBehaviour
{
    public GameObject deathUI; // Asigna el panel de UI desde el inspector

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

            // Muestra la UI de muerte
            if (deathUI != null)
            {
                deathUI.SetActive(true);
            }
        }
    }

    // Llamar esta función desde el botón de reiniciar
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

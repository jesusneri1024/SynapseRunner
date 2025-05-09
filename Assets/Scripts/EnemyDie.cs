using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyDie : MonoBehaviour
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
        if (collision.collider.CompareTag("Bala"))
        {
            Destroy(this.gameObject);
        }
    }


}

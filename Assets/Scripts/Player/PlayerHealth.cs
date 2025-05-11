using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Necesario para Image UI

public class PlayerHealth : MonoBehaviour
{
    [Header("Salud del Jugador")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("UI y Sonido")]
    public GameObject deathUI;
    public Image healthBarFill; // Asigna aquí el Image "Filled"
    public AudioClip deathSound;
    private AudioSource audioSource;

    public AudioClip damageSound;

    private bool isDead = false;

    [Header("Curación")]
    public AudioClip healSound; // Sonido al recoger botiquín
    public GameObject healEffectPrefab; // Partículas al curarse

    private void Start()
    {
        currentHealth = maxHealth;
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        UpdateHealthBar();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("BalaEnemigo"))
        {
            TakeDamage(25); // Ajusta el daño según necesites
            Destroy(collision.gameObject);
            if (damageSound != null)
            {
                audioSource.PlayOneShot(damageSound);
            }

        }

        else if (collision.collider.CompareTag("Botiquin"))
        {
            Heal(50); // Curar al jugador

            // Reproduce sonido de curación
            if (healSound != null)
            {
                audioSource.PlayOneShot(healSound);
            }

            // Instancia partículas
            if (healEffectPrefab != null)
            {
                Instantiate(healEffectPrefab, transform.position, Quaternion.identity);
            }
            Destroy(collision.gameObject); // Destruir el botiquín al recogerlo
        }
    }


    void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthBar()
    {
        if (healthBarFill != null)
        {
            float fillAmount = (float)currentHealth / maxHealth;
            healthBarFill.fillAmount = fillAmount;
        }
    }

    void Die()
    {
        isDead = true;

        // Detener movimiento del jugador
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        // Reproduce sonido de muerte
        if (deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        // Desactiva generador de chunks
        GameObject generator = GameObject.Find("ChunkGenerator");
        if (generator != null)
        {
            generator.SetActive(false);
        }

        // Muestra UI de muerte
        if (deathUI != null)
        {
            deathUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void Heal(int amount)
    {
        if (isDead) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();
    }



}

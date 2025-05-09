using UnityEngine;
using TMPro;
using System.Collections;

public class Shoot : MonoBehaviour
{
    [SerializeField] GameObject sb;
    [SerializeField] GameObject gunShoot;
    [SerializeField] GameObject gunModel; // Referencia al modelo de la pistola para rotar
    [SerializeField] AudioClip pium;
    [SerializeField] AudioClip reloadSound; // Nuevo audio clip para el sonido de recarga
    [SerializeField] AudioSource sourcePium;
    [SerializeField] int maxAmmo = 6;
    [SerializeField] float reloadTime = 2f;
    [SerializeField] TextMeshProUGUI ammoText;
    [SerializeField] TextMeshProUGUI reloadingText; // Nuevo texto para mostrar "Recargando..."
    [SerializeField] float reloadRotation = -40f; // Ángulo de rotación durante la recarga
    [SerializeField] float rotationSpeed = 5f; // Velocidad de la animación de rotación

    public float intensity;

    private int currentAmmo;
    private bool isReloading = false;
    private bool isBlinking = false;
    private Coroutine blinkCoroutine;
    private Quaternion originalRotation; // Para guardar la rotación original

    GameObject virtualBullet;
    Rigidbody rb;

    void Start()
    {
        sourcePium = GetComponent<AudioSource>();
        currentAmmo = maxAmmo;
        UpdateAmmoUI();

        // Inicializamos el texto de recarga como invisible
        if (reloadingText != null)
        {
            reloadingText.gameObject.SetActive(false);
        }

        // Guardar la rotación original de la pistola
        if (gunModel != null)
        {
            originalRotation = gunModel.transform.localRotation;
        }
        else
        {
            Debug.LogWarning("No se ha asignado el modelo de la pistola al script Shoot.");
        }
    }

    void Update()
    {
        if (isReloading)
            return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetMouseButtonDown(0) && currentAmmo > 0)
        {
            ShootBullet();
        }
    }

    void ShootBullet()
    {
        virtualBullet = Instantiate(sb, gunShoot.transform.position, gunShoot.transform.rotation);
        rb = virtualBullet.GetComponent<Rigidbody>();
        rb.AddRelativeForce(Vector3.forward * 30, ForceMode.Impulse);

        sourcePium.PlayOneShot(pium);
        Destroy(virtualBullet, 2f);

        currentAmmo--;
        UpdateAmmoUI();
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Recargando...");

        // Mostrar texto de recarga en UI
        if (reloadingText != null)
        {
            reloadingText.gameObject.SetActive(true);
        }

        // Reproducir sonido de recarga
        if (reloadSound != null)
        {
            sourcePium.PlayOneShot(reloadSound);
        }

        // Crear rotación objetivo para la recarga
        if (gunModel != null)
        {
            Quaternion targetRotation = Quaternion.Euler(
                gunModel.transform.localEulerAngles.x,
                gunModel.transform.localEulerAngles.y,
                reloadRotation
            );

            // Animación de rotación suave hacia la posición de recarga
            float elapsedTime = 0f;
            float rotationDuration = 0.3f; // Tiempo que tarda en rotar
            Quaternion startRotation = gunModel.transform.localRotation;

            while (elapsedTime < rotationDuration)
            {
                gunModel.transform.localRotation = Quaternion.Slerp(
                    startRotation,
                    targetRotation,
                    elapsedTime / rotationDuration
                );
                elapsedTime += Time.deltaTime * rotationSpeed;
                yield return null;
            }

            // Asegurarnos de que llegue a la rotación exacta
            gunModel.transform.localRotation = targetRotation;
        }

        // Esperar el tiempo de recarga
        yield return new WaitForSeconds(reloadTime - 0.6f); // Ajustamos tiempo para las animaciones

        // Volver a la rotación original con animación
        if (gunModel != null)
        {
            float elapsedTime = 0f;
            float rotationDuration = 0.3f;
            Quaternion startRotation = gunModel.transform.localRotation;

            while (elapsedTime < rotationDuration)
            {
                gunModel.transform.localRotation = Quaternion.Slerp(
                    startRotation,
                    originalRotation,
                    elapsedTime / rotationDuration
                );
                elapsedTime += Time.deltaTime * rotationSpeed;
                yield return null;
            }

            // Asegurarnos de que vuelva exactamente a la rotación original
            gunModel.transform.localRotation = originalRotation;
        }

        currentAmmo = maxAmmo;
        isReloading = false;
        Debug.Log("Recarga completa.");

        // Ocultar texto de recarga
        if (reloadingText != null)
        {
            reloadingText.gameObject.SetActive(false);
        }

        UpdateAmmoUI();
    }

    void UpdateAmmoUI()
    {
        ammoText.text = "Balas: " + currentAmmo + " / " + maxAmmo;

        if (currentAmmo == 0 && !isBlinking)
        {
            blinkCoroutine = StartCoroutine(BlinkAmmoText());
            isBlinking = true;
        }
        else if (currentAmmo > 0 && isBlinking)
        {
            StopCoroutine(blinkCoroutine);
            ammoText.enabled = true;
            isBlinking = false;
        }
    }

    IEnumerator BlinkAmmoText()
    {
        while (true)
        {
            ammoText.enabled = !ammoText.enabled;
            yield return new WaitForSeconds(0.3f);
        }
    }
}
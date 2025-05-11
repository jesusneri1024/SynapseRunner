using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class Movement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 7f;
    private Rigidbody rb;
    private CapsuleCollider capsule;
    private bool isGrounded;
    private int jumpCount = 0;
    public int maxJumps = 2;

    // Agacharse
    public float crouchHeight = 1f;
    private float originalHeight;
    private Vector3 originalCenter;
    private bool isCrouching = false;

    // UI para mensaje de borde
    public GameObject borderWarningUI;
    public TextMeshProUGUI warningText;
    public TextMeshProUGUI timerText;

    // Control de tiempo en zona de borde
    private bool isNearBorder = false;
    private bool canMoveForward = false;
    private float forwardMovementTimer = 5f;
    private float currentTimer = 0f;

    private bool isTouchingObstacle = false;

    private GameObject climbableObject = null;



    void Start()
    {
        rb = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();
        originalHeight = capsule.height;
        originalCenter = capsule.center;

        // Asegúrate de que la UI de advertencia está desactivada al inicio
        if (borderWarningUI != null)
        {
            borderWarningUI.SetActive(false);
        }
    }

    void Update()
    {
        // Verificar si está en la zona de borde
        CheckBorderZone();

        // Movimiento horizontal (eje X)
        float moveX = Input.GetAxis("Horizontal");

        // Movimiento en eje Z (adelante/atrás)
        float moveZ = 0f;

        // Solo permitir movimiento en Z cuando está en la zona especial y tiene permiso
        if (isNearBorder && canMoveForward)
        {
            if (Input.GetKey(KeyCode.W))
            {
                moveZ = 1f; // Movimiento hacia adelante en Z
            }

            // Actualizar el timer
            currentTimer -= Time.deltaTime;
            if (currentTimer <= 0)
            {
                canMoveForward = false;
                if (borderWarningUI != null)
                {
                    borderWarningUI.SetActive(false);
                }
            }
            else if (timerText != null)
            {
                timerText.text = "Tiempo: " + currentTimer.ToString("F1") + "s";
            }
        }

        // Aplicar velocidad
        rb.linearVelocity = new Vector3(moveX * speed, rb.linearVelocity.y, moveZ * speed);

        // Saltar
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumps && !isCrouching)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpCount++;
        }

        // Agacharse
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isCrouching)
        {
            capsule.height = crouchHeight;
            capsule.center = new Vector3(capsule.center.x, crouchHeight / 2f, capsule.center.z);
            isCrouching = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) && isCrouching)
        {
            capsule.height = originalHeight;
            capsule.center = originalCenter;
            isCrouching = false;
        }

        if (isTouchingObstacle && climbableObject != null && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(ClimbOverObstacle());
        }

        // Caída rápida al mantener Shift en el aire
        if (Input.GetKey(KeyCode.LeftShift))
        {
            rb.AddForce(Vector3.down * 20f, ForceMode.Acceleration);
        }



    }

    private void CheckBorderZone()
    {
        float posZ = transform.position.z;

        // Comprueba si está en la zona de borde (entre Z=-15 y Z=-19)
        if (posZ <= -5f && posZ >= -19f)
        {
            // Si acaba de entrar en la zona
            if (!isNearBorder)
            {
                isNearBorder = true;
                canMoveForward = true;
                currentTimer = forwardMovementTimer;

                // Mostrar mensaje UI
                if (borderWarningUI != null)
                {
                    borderWarningUI.SetActive(true);
                    if (warningText != null)
                    {
                        warningText.text = "¡Estás cerca del borde!\nPresiona W para avanzar.";
                    }
                }
            }
        }
        else
        {
            // Si sale de la zona de borde
            if (isNearBorder)
            {
                isNearBorder = false;
                canMoveForward = false;

                // Ocultar mensaje UI
                if (borderWarningUI != null)
                {
                    borderWarningUI.SetActive(false);
                }
            }


        }
    }

    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            // Si el contacto es frontal
            if (Vector3.Dot(contact.normal, transform.forward) < -0.5f)
            {
                if (collision.gameObject.CompareTag("Climbable"))
                {
                    isTouchingObstacle = true;
                    climbableObject = collision.gameObject;
                }
            }

            // También detectamos si está en el suelo
            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;
                jumpCount = 0;
            }
        }
    }



    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == climbableObject)
        {
            isTouchingObstacle = false;
            climbableObject = null;
        }
    }


    private IEnumerator ClimbOverObstacle()
    {
        if (climbableObject == null) yield break;

        rb.isKinematic = true;

        Transform chunkParent = climbableObject.transform.parent;
        if (chunkParent != null)
        {
            transform.SetParent(chunkParent); // parent al pasillo, no al objeto pequeño
        }


        Collider climbableCollider = climbableObject.GetComponent<Collider>();
        if (climbableCollider == null)
        {
            transform.SetParent(null);
            rb.isKinematic = false;
            yield break;
        }

        Vector3 localStart = transform.localPosition;
        float topLocalY = climbableCollider.bounds.size.y + 1.0f;

        Vector3 targetLocalPos = new Vector3(
            localStart.x,
            topLocalY,
            localStart.z // no cambiamos z, escala en vertical solamente
        );

        float duration = 0.6f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            float smoothT = Mathf.SmoothStep(0f, 1f, t);
            transform.localPosition = Vector3.Lerp(localStart, targetLocalPos, smoothT);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Posición final exacta
        transform.localPosition = targetLocalPos;

        // Desparenteamos y habilitamos físicas
        transform.SetParent(null);
        rb.isKinematic = false;

        // Reset
        climbableObject = null;
        isTouchingObstacle = false;
    }








}
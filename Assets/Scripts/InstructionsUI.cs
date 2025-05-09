using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InstructionsUI : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private GameObject instructionsPanel;
    [SerializeField] private float displayTime = 10.0f;
    [SerializeField] private bool fadeOut = true;
    [SerializeField] private float fadeOutDuration = 1.0f;

    [Header("Opcional")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip closeSound;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        // Intentar obtener CanvasGroup si existe
        canvasGroup = instructionsPanel.GetComponent<CanvasGroup>();

        // Si no existe el CanvasGroup y el fadeOut está habilitado, crearlo
        if (canvasGroup == null && fadeOut)
        {
            canvasGroup = instructionsPanel.AddComponent<CanvasGroup>();
        }
    }

    private void Start()
    {
        // Asegurarse de que el panel está visible al inicio
        ShowInstructions();

        // Iniciar la cuenta regresiva para ocultar el panel
        StartCoroutine(HideInstructionsAfterDelay());
    }

    private void ShowInstructions()
    {
        instructionsPanel.SetActive(true);

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1.0f;
        }

        // Reproducir sonido si está configurado
        if (audioSource != null && openSound != null)
        {
            audioSource.PlayOneShot(openSound);
        }
    }

    private IEnumerator HideInstructionsAfterDelay()
    {
        // Esperar el tiempo configurado
        yield return new WaitForSeconds(displayTime);

        // Aplicar fadeOut si está habilitado
        if (fadeOut && canvasGroup != null)
        {
            // Reproducir sonido de cierre si está configurado
            if (audioSource != null && closeSound != null)
            {
                audioSource.PlayOneShot(closeSound);
            }

            float elapsedTime = 0;
            while (elapsedTime < fadeOutDuration)
            {
                canvasGroup.alpha = Mathf.Lerp(1.0f, 0.0f, elapsedTime / fadeOutDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            canvasGroup.alpha = 0.0f;
        }

        // Ocultar el panel
        instructionsPanel.SetActive(false);
    }

    // Método público para mostrar instrucciones de nuevo si es necesario
    public void ShowInstructionsAgain()
    {
        StopAllCoroutines();
        ShowInstructions();
        StartCoroutine(HideInstructionsAfterDelay());
    }
}
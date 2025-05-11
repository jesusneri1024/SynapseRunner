using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UISetup : MonoBehaviour
{
    public Movement playerMovement;

    [Header("UI Elements")]
    public GameObject borderWarningPanel;
    public TextMeshProUGUI warningText;
    public TextMeshProUGUI timerText;

    void Start()
    {
        if (playerMovement != null)
        {
            // Asignar referencias de UI al script de movimiento
            playerMovement.borderWarningUI = borderWarningPanel;
            playerMovement.warningText = warningText;
            playerMovement.timerText = timerText;

            // Asegurarse de que la UI esté desactivada al inicio
            if (borderWarningPanel != null)
            {
                borderWarningPanel.SetActive(false);
            }
        }
        else
        {
            Debug.LogError("No se ha asignado el componente Movement al script UISetup.");
        }
    }
}
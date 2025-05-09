using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // Esta función será llamada por el botón UI
    public void Reiniciar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void CargarJuego()
    {
        SceneManager.LoadScene("Test1");
    }

    // Carga la escena llamada "Menu"
    public void CargarEscenaMenu()
    {
        SceneManager.LoadScene("Menu");
    }


}

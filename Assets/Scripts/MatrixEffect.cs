using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class MatrixEffect : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private int columnas = 20;
    [SerializeField] private float velocidadCaida = 1f;
    [SerializeField] private float tiempoEntreCambios = 0.05f;
    [SerializeField] private Color colorTexto = Color.green;
    [SerializeField] private float opacidadMinima = 0.2f;
    [SerializeField] private float opacidadMaxima = 1f;
    [SerializeField] private int longitudMinima = 5;
    [SerializeField] private int longitudMaxima = 20;

    private List<MatrixColumna> columnasMatrix;
    private float tiempoUltimoCambio;
    private string caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789@#$%^&*()_+-=[]{}|;:,.<>?";
    private RectTransform rectTransform;
    private float anchoColumna;

    private class MatrixColumna
    {
        public TextMeshProUGUI texto;
        public float posicionY;
        public int longitud;
        public float velocidad;
        public float tiempoUltimoCambio;
    }

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        anchoColumna = rectTransform.rect.width / columnas;
        InicializarColumnas();
    }

    private void InicializarColumnas()
    {
        columnasMatrix = new List<MatrixColumna>();

        for (int i = 0; i < columnas; i++)
        {
            GameObject columnaObj = new GameObject($"MatrixColumna_{i}");
            columnaObj.transform.SetParent(transform, false);

            RectTransform columnaRect = columnaObj.AddComponent<RectTransform>();
            columnaRect.anchoredPosition = new Vector2(i * anchoColumna, 0);
            columnaRect.sizeDelta = new Vector2(anchoColumna, rectTransform.rect.height);

            TextMeshProUGUI texto = columnaObj.AddComponent<TextMeshProUGUI>();
            texto.alignment = TextAlignmentOptions.Center;
            texto.fontSize = anchoColumna * 0.8f;
            texto.color = colorTexto;

            MatrixColumna columna = new MatrixColumna
            {
                texto = texto,
                posicionY = Random.Range(0, rectTransform.rect.height),
                longitud = Random.Range(longitudMinima, longitudMaxima + 1),
                velocidad = Random.Range(velocidadCaida * 0.5f, velocidadCaida * 1.5f),
                tiempoUltimoCambio = 0
            };

            columnasMatrix.Add(columna);
            ActualizarTextoColumna(columna);
        }
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;

        foreach (MatrixColumna columna in columnasMatrix)
        {
            columna.posicionY -= columna.velocidad * deltaTime;

            if (columna.posicionY < -rectTransform.rect.height)
            {
                columna.posicionY = rectTransform.rect.height;
                columna.longitud = Random.Range(longitudMinima, longitudMaxima + 1);
                columna.velocidad = Random.Range(velocidadCaida * 0.5f, velocidadCaida * 1.5f);
            }

            if (Time.time - columna.tiempoUltimoCambio >= tiempoEntreCambios)
            {
                ActualizarTextoColumna(columna);
                columna.tiempoUltimoCambio = Time.time;
            }

            columna.texto.rectTransform.anchoredPosition = new Vector2(
                columna.texto.rectTransform.anchoredPosition.x,
                columna.posicionY
            );
        }
    }

    private void ActualizarTextoColumna(MatrixColumna columna)
    {
        string texto = "";
        for (int i = 0; i < columna.longitud; i++)
        {
            texto += caracteres[Random.Range(0, caracteres.Length)];
        }
        columna.texto.text = texto;

        // Aplicar gradiente de opacidad
        Color colorActual = colorTexto;
        colorActual.a = Mathf.Lerp(opacidadMinima, opacidadMaxima, 
            (columna.posicionY + rectTransform.rect.height) / (rectTransform.rect.height * 2));
        columna.texto.color = colorActual;
    }
} 
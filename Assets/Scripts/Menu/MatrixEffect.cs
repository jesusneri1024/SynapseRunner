using UnityEngine;
using System.Collections.Generic;

public class MatrixEffect : MonoBehaviour
{
    // Prefab para los caracteres de Matrix
    public GameObject characterPrefab;

    // Fuente para los caracteres Matrix
    public Font matrixFont;

    // Color de los caracteres (verde típico de Matrix)
    public Color characterColor = new Color(0, 255, 0);

    // Número de columnas de caracteres por efecto
    public int columnsPerEffect = 15;

    // Altura máxima de cada columna
    public int maxColumnHeight = 20;

    // Velocidad de caída
    public float fallSpeed = 5f;

    // Tiempo entre generación de caracteres
    public float spawnInterval = 0.05f;

    // Lista para almacenar todos los efectos activos
    private List<MatrixColumn[]> activeEffects = new List<MatrixColumn[]>();

    // Clase para manejar una columna de caracteres
    [System.Serializable]
    public class MatrixColumn
    {
        public List<GameObject> characters = new List<GameObject>();
        public float x;
        public float topY;
        public float speed;
        public float nextSpawnTime;
        public int maxChars;
        public bool isActive = true;
    }

    void Start()
    {
        // Asegurarse de que la cámara tenga un fondo negro
        Camera.main.backgroundColor = Color.black;

        // Crear el prefab de caracteres si no se asignó uno
        if (characterPrefab == null)
        {
            characterPrefab = new GameObject("MatrixChar");
            TextMesh textMesh = characterPrefab.AddComponent<TextMesh>();
            textMesh.font = matrixFont;
            textMesh.GetComponent<Renderer>().material = matrixFont.material;
            textMesh.color = characterColor;
            textMesh.fontSize = 14;
            textMesh.anchor = TextAnchor.MiddleCenter;
            characterPrefab.AddComponent<MatrixCharacter>();
        }
    }

    void Update()
    {
        // Detectar clic izquierdo del ratón
        if (Input.GetMouseButtonDown(0))
        {
            // Obtener la posición del clic en coordenadas del mundo
            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickPosition.z = 0;

            // Crear un nuevo efecto Matrix en la posición del clic
            CreateMatrixEffect(clickPosition);
        }

        // Actualizar todos los efectos activos
        UpdateEffects();
    }

    void CreateMatrixEffect(Vector3 position)
    {
        MatrixColumn[] columns = new MatrixColumn[columnsPerEffect];

        // Crear columnas alrededor del punto de clic
        for (int i = 0; i < columnsPerEffect; i++)
        {
            columns[i] = new MatrixColumn();
            // Distribuir columnas alrededor del punto de clic
            float angle = (i * 360f / columnsPerEffect) * Mathf.Deg2Rad;
            float radius = Random.Range(0.5f, 2f);
            columns[i].x = position.x + Mathf.Cos(angle) * radius;
            columns[i].topY = position.y + Mathf.Sin(angle) * radius;
            columns[i].speed = fallSpeed * Random.Range(0.8f, 1.2f);
            columns[i].maxChars = Random.Range(maxColumnHeight / 2, maxColumnHeight);
            columns[i].nextSpawnTime = Time.time;
        }

        // Añadir a la lista de efectos activos
        activeEffects.Add(columns);
    }

    void UpdateEffects()
    {
        List<int> effectsToRemove = new List<int>();

        // Actualizar cada efecto
        for (int e = 0; e < activeEffects.Count; e++)
        {
            MatrixColumn[] effect = activeEffects[e];
            bool allColumnsFinished = true;

            // Actualizar cada columna
            for (int c = 0; c < effect.Length; c++)
            {
                MatrixColumn column = effect[c];

                if (column.isActive)
                {
                    allColumnsFinished = false;

                    // Generar nuevos caracteres
                    if (Time.time >= column.nextSpawnTime && column.characters.Count < column.maxChars)
                    {
                        // Crear un nuevo carácter
                        GameObject newChar = Instantiate(characterPrefab);
                        TextMesh textMesh = newChar.GetComponent<TextMesh>();

                        // Seleccionar un carácter aleatorio (estilo Matrix)
                        char randomChar = GetRandomMatrixChar();
                        textMesh.text = randomChar.ToString();

                        // Posicionar el carácter
                        newChar.transform.position = new Vector3(column.x, column.topY, 0);

                        // Añadir a la columna
                        column.characters.Add(newChar);

                        // Programar el próximo carácter
                        column.nextSpawnTime = Time.time + spawnInterval;
                    }

                    // Mover los caracteres existentes hacia abajo
                    for (int i = 0; i < column.characters.Count; i++)
                    {
                        GameObject charObj = column.characters[i];
                        if (charObj != null)
                        {
                            // Mover hacia abajo
                            charObj.transform.position += Vector3.down * column.speed * Time.deltaTime;

                            // Cambiar el carácter al azar ocasionalmente
                            if (Random.value < 0.05f)
                            {
                                TextMesh textMesh = charObj.GetComponent<TextMesh>();
                                textMesh.text = GetRandomMatrixChar().ToString();
                            }

                            // Ajustar el brillo según la posición (los más nuevos más brillantes)
                            TextMesh tm = charObj.GetComponent<TextMesh>();
                            float brightness = 1.0f - (float)i / column.characters.Count;
                            tm.color = new Color(characterColor.r, characterColor.g, characterColor.b, brightness);
                        }
                    }

                    // Dejar de generar si se alcanzó el máximo
                    if (column.characters.Count >= column.maxChars)
                    {
                        column.isActive = false;
                    }
                }
                else
                {
                    // Esta columna ya no está activa, verificar si todos sus caracteres salieron de la pantalla
                    bool columnFinished = true;
                    for (int i = 0; i < column.characters.Count; i++)
                    {
                        if (column.characters[i] != null)
                        {
                            // Si algún carácter está todavía visible, la columna no ha terminado
                            columnFinished = false;
                            break;
                        }
                    }

                    if (!columnFinished)
                    {
                        allColumnsFinished = false;
                    }
                }
            }

            // Si todas las columnas de este efecto han terminado, marcar para eliminación
            if (allColumnsFinished)
            {
                effectsToRemove.Add(e);
            }
        }

        // Eliminar los efectos terminados (en orden inverso para no afectar los índices)
        for (int i = effectsToRemove.Count - 1; i >= 0; i--)
        {
            activeEffects.RemoveAt(effectsToRemove[i]);
        }
    }

    char GetRandomMatrixChar()
    {
        // Caracteres típicos de Matrix (incluye katakana japonés y símbolos)
        string matrixChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+-=[]{}|;:,./<>?ｱｲｳｴｵｶｷｸｹｺｻｼｽｾｿﾀﾁﾂﾃﾄﾅﾆﾇﾈﾉﾊﾋﾌﾍﾎﾏﾐﾑﾒﾓﾔﾕﾖﾗﾘﾙﾚﾛﾜﾝ";
        return matrixChars[Random.Range(0, matrixChars.Length)];
    }
}

// Componente para manejar el comportamiento individual de cada carácter
public class MatrixCharacter : MonoBehaviour
{
    private float lifetime;
    private float maxLifetime;

    void Start()
    {
        maxLifetime = Random.Range(3f, 8f);
        lifetime = 0;
    }

    void Update()
    {
        lifetime += Time.deltaTime;

        // Desvanecer y destruir después de cierto tiempo
        if (lifetime > maxLifetime)
        {
            TextMesh tm = GetComponent<TextMesh>();
            Color c = tm.color;
            c.a -= Time.deltaTime;
            tm.color = c;

            if (c.a <= 0)
            {
                Destroy(gameObject);
            }
        }

        // Cambiar de carácter ocasionalmente
        if (Random.value < 0.01f)
        {
            TextMesh tm = GetComponent<TextMesh>();
            string matrixChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+-=[]{}|;:,./<>?ｱｲｳｴｵｶｷｸｹｺｻｼｽｾｿﾀﾁﾂﾃﾄﾅﾆﾇﾈﾉﾊﾋﾌﾍﾎﾏﾐﾑﾒﾓﾔﾕﾖﾗﾘﾙﾚﾛﾜﾝ";
            tm.text = matrixChars[Random.Range(0, matrixChars.Length)].ToString();
        }
    }
}
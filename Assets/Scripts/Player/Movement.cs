using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        
        // Movimiento estrictamente horizontal en el eje mundial
        Vector3 move = new Vector3(moveX, 0f, 0f) * speed * Time.deltaTime;
        transform.position += move;
    }
}

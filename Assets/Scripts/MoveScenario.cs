using UnityEngine;

public class MoveScenario : MonoBehaviour
{
    private int movZ;
    public int vel;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = new Vector3(0f, 0f, -vel) * Time.deltaTime;
        transform.position += move;
    }
}

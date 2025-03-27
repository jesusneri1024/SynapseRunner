using UnityEngine;

public class Shoot : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject sb;
    [SerializeField] GameObject gunShoot;
    GameObject virtualBullet;
    Rigidbody playerRB;
    Rigidbody rb;
    public int jump;
    [SerializeField] AudioClip pium;
    [SerializeField] AudioSource sourcePium;
    public GameObject camera;
    public float intensity;


    void Start()
    {

        sourcePium = GetComponent<AudioSource>();
        playerRB = GetComponent<Rigidbody>();

    }

public float MapRange(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return outMin + (value - inMin) * (outMax - outMin) / (inMax - inMin);
    }
    void Update()
    {



        if (Input.GetMouseButtonDown(0))
        {
            virtualBullet = Instantiate(sb, gunShoot.transform.position, gunShoot.transform.rotation);
            rb = virtualBullet.GetComponent<Rigidbody>();
            rb.AddRelativeForce(Vector3.forward * 3, ForceMode.Impulse);

            //salto

            float newValue = MapRange(10, 0f, 90f, 0f, 6f);
            print(camera.transform.localRotation.x);
            playerRB.AddRelativeForce(Vector3.up * camera.transform.localRotation.eulerAngles.magnitude * .3f, ForceMode.Impulse);


            sourcePium.PlayOneShot(pium);
        }
    }

    
}

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
    public GameObject camara;
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
            // Dispara la bala
            virtualBullet = Instantiate(sb, gunShoot.transform.position, gunShoot.transform.rotation);
            rb = virtualBullet.GetComponent<Rigidbody>();
            rb.AddRelativeForce(Vector3.forward * 3, ForceMode.Impulse);

            // Dirección de disparo (hacia donde apunta el arma)
            Vector3 shootDir = gunShoot.transform.forward;

            // Eliminar el eje Z para que no empuje hacia adelante o atrás
            shootDir.z = 0f;

            // Si el vector no es cero, invertimos y normalizamos
            if (shootDir != Vector3.zero)
            {
                Vector3 pushDir = -shootDir.normalized; // Dirección opuesta a donde se dispara (solo X y Y)
                playerRB.AddForce(pushDir * intensity, ForceMode.Impulse);
            }

            // Sonido
            sourcePium.PlayOneShot(pium);

            //Destruir bala
            Destroy(virtualBullet, 2f);

        }
    }





}

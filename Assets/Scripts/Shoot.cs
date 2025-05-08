using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] GameObject sb;
    [SerializeField] GameObject gunShoot;
    GameObject virtualBullet;
    Rigidbody rb;
    [SerializeField] AudioClip pium;
    [SerializeField] AudioSource sourcePium;
    public float intensity;

    void Start()
    {
        sourcePium = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Dispara la bala
            virtualBullet = Instantiate(sb, gunShoot.transform.position, gunShoot.transform.rotation);
            rb = virtualBullet.GetComponent<Rigidbody>();
            rb.AddRelativeForce(Vector3.forward * 30, ForceMode.Impulse);

            // Sonido
            sourcePium.PlayOneShot(pium);

            // Destruir bala después de 2 segundos
            Destroy(virtualBullet, 2f);
        }
    }
}

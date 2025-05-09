using UnityEngine;

public class DroneAI : MonoBehaviour
{
    public float detectionRange = 20f;
    public float fireCooldown = 2f;
    public Transform firePoint; // Desde dónde dispara
    public GameObject projectilePrefab;
    public Transform cannonTransform; // Parte visual que se “anima”

    private Transform player;
    private float fireTimer;
    private bool isRecoiling = false;
    private Vector3 originalCannonPosition;
    private float recoilTime = 0.1f;
    private float recoilTimer = 0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (cannonTransform)
        {
            originalCannonPosition = cannonTransform.localPosition;
        }
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            // Mira hacia el jugador
            // Dentro de Update(), reemplaza la rotación actual por esta línea:
            Vector3 direction = (player.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);


            // Dispara
            if (fireTimer <= 0f)
            {
                Shoot();
                fireTimer = fireCooldown;
            }
        }

        fireTimer -= Time.deltaTime;

        // Simula retroceso visual
        if (isRecoiling && cannonTransform)
        {
            recoilTimer += Time.deltaTime;
            if (recoilTimer < recoilTime)
            {
                cannonTransform.localPosition = originalCannonPosition + new Vector3(0, 0, -0.1f);
            }
            else
            {
                cannonTransform.localPosition = originalCannonPosition;
                isRecoiling = false;
                recoilTimer = 0f;
            }
        }
    }

    void Shoot()
    {
        if (projectilePrefab && firePoint)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb && player)
            {
                Vector3 toPlayer = player.position - firePoint.position;

                float timeToReachTarget = 0.3f; // tiempo deseado para que llegue (puedes ajustar esto)
                Vector3 velocity = toPlayer / timeToReachTarget;

                rb.linearVelocity = velocity; // ¡velocidad directa!
            }
        }

        // Simula animación de retroceso
        if (cannonTransform)
        {
            isRecoiling = true;
        }
    }


}

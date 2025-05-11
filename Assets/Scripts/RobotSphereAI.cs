using UnityEngine;
using System.Collections;


public class RobotSphereAI : MonoBehaviour
{
    public Transform player;
    public float speed = 2f;
    public float rotationSpeed = 5f;
    public float attackRange = 2f;
    public float retreatDistance = 3f;
    public float fixedYHeight = -3.15f; // Altura a la que el robot debe flotar

    private Animator anim;
    private enum State { Idle, Chasing, Attacking, Retreating }
    private State currentState;

    private Vector3 retreatTarget;
    private float attackCooldown = 2f;
    private float lastAttackTime = -999f;

    private float damageCooldown = 10f; // Tiempo de inmunidad tras ser golpeado
    private float lastDamageTime = -999f;

    public int health = 30;

    public GameObject deathParticlesPrefab;

    public GameObject visualObject; // Asigna este en el Inspector (el hijo visual del robot)


    private Renderer[] renderers;
    private Color originalColor;
    private bool isImmune = false;
    public float immuneDuration = 1f;



    void Start()
    {
        anim = GetComponent<Animator>();
        currentState = State.Idle;

        renderers = GetComponentsInChildren<Renderer>();

        foreach (var r in renderers)
        {
            r.material = new Material(r.material); // Clona para evitar afectar a otros
        }

        if (renderers.Length > 0)
        {
            originalColor = renderers[0].material.color;
        }
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);




        switch (currentState)
        {
            case State.Idle:
                if (distanceToPlayer < 10f)
                {
                    anim.SetBool("Open_Anim", true); // Se abre al detectar
                    ChangeState(State.Chasing);
                }
                break;

            case State.Chasing:
                if (distanceToPlayer <= attackRange)
                {
                    ChangeState(State.Attacking);
                }
                else
                {
                    ChasePlayer();
                }
                break;

            case State.Attacking:
                AttackPlayer(distanceToPlayer);
                break;


            case State.Retreating:
                Retreat();
                break;
        }

        // Mantener rotación hacia el jugador
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }

        // Mantener altura fija
        Vector3 pos = transform.position;
        pos.y = fixedYHeight;
        transform.position = pos;
    }

    void ChangeState(State newState)
    {
        Debug.Log("Cambiando de estado: " + currentState + " → " + newState);
        currentState = newState;

        switch (newState)
        {
            case State.Idle:
                anim.SetBool("Walk_Anim", false);
                anim.SetBool("Roll_Anim", false);
                break;

            case State.Chasing:
                anim.SetBool("Walk_Anim", true);
                anim.SetBool("Roll_Anim", false);
                break;

            case State.Attacking:
                Debug.Log("Entrando a estado ATTACKING");
                anim.SetBool("Walk_Anim", false);
                anim.SetBool("Roll_Anim", true);
                AttackPlayer(Vector3.Distance(transform.position, player.position)); // Ejecuta ataque inmediatamente
                Invoke("StartRetreat", 1.2f);
                break;

            case State.Retreating:
                anim.SetBool("Walk_Anim", true);
                anim.SetBool("Roll_Anim", false);
                retreatTarget = transform.position - transform.forward * retreatDistance;
                break;
        }
    }


    void ChasePlayer()
    {
        Vector3 targetPos = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
    }

    void AttackPlayer(float distanceToPlayer)
    {
        Debug.Log("Intentando atacar. Distancia: " + distanceToPlayer + " / Rango: " + attackRange);

        if (distanceToPlayer <= attackRange && Time.time - lastAttackTime > attackCooldown)
        {
            Debug.Log("Dentro del rango y cooldown cumplido");

            PlayerHealth ph = player.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.TakeDamage(10);
                Debug.Log("¡DAÑO INFLIGIDO al jugador!");
                lastAttackTime = Time.time;
            }
            else
            {
                Debug.LogWarning("No se encontró el componente PlayerHealth en el jugador");
            }
        }
    }



    void StartRetreat()
    {
        if (currentState == State.Attacking)
            ChangeState(State.Retreating);
    }

    void Retreat()
    {
        Vector3 retreatPos = new Vector3(retreatTarget.x, transform.position.y, retreatTarget.z);
        transform.position = Vector3.MoveTowards(transform.position, retreatPos, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, retreatPos) < 0.5f)
        {
            ChangeState(State.Idle);
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bala"))
        {
            Debug.Log("Robot golpeado por una BALA (Collision)");
            TakeDamage(10);
            Destroy(collision.gameObject); // Destruye la bala tras impactar
        }
    }

    void TakeDamage(int damage)
    {
        if (isImmune) return;

        health -= damage;
        Debug.Log("Vida del robot: " + health);

        anim.SetBool("Open_Anim", false);

        if (health <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(ImmuneFlash());
        }
    }



    void Die()
    {
        Debug.Log("¡Robot inicia muerte!");

        anim.SetBool("Open_Anim", false); // Se cierra
        StartCoroutine(DeathSequence());  // Ejecuta secuencia de muerte
    }


    IEnumerator DeathSequence()
    {
        // Espera a que termine animación de cierre
        yield return new WaitForSeconds(1f);

        // Ocultar parte visual del robot
        if (transform.childCount > 0)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }

        // Instanciar partículas
        if (deathParticlesPrefab != null)
        {
            Instantiate(deathParticlesPrefab, transform.position, Quaternion.identity);
        }

        // Espera a que partículas se vean
        yield return new WaitForSeconds(1.5f);

        this.enabled = false; // Detener la lógica justo antes de destruir
        Destroy(gameObject);
    }

    IEnumerator ImmuneFlash()
    {
        isImmune = true;

        Color tintedColor = Color.Lerp(originalColor, Color.red, 0.6f); // mezcla entre original y rojo

        foreach (var r in renderers)
        {
            r.material.color = tintedColor;
        }

        yield return new WaitForSeconds(immuneDuration);

        foreach (var r in renderers)
        {
            r.material.color = originalColor;
        }

        isImmune = false;
    }






}

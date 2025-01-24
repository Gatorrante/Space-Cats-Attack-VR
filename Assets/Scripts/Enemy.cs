using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 2; 
    public float speed = 2f; 
    public float detectionRadius = 350f; 
    public ParticleSystem deathParticle; 

    private Transform player; 

    void Start()
    {
        Debug.Log($"Enemigo respawneado con {health} de vida.");
    }

    void Update()
    {
        DetectPlayer();
        if (player != null)
        {
            MoveTowardsPlayer();
        }
    }

    /// <summary>
    /// Detecta al jugador
    /// </summary>
    void DetectPlayer()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                player = hitCollider.transform;
                break;
            }
        }
    }

    /// <summary>
    /// Busca al jugador
    /// </summary>
    void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }

    /// <summary>
    /// Aplica daño 
    /// </summary>
    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log($"Vida restante del enemigo: {health}");

        if (health <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Muerte del enemigo
    /// </summary>
    void Die()
    {
        Debug.Log($"Enemigo {gameObject.name} muerto!");

        if (deathParticle != null)
        {
            ParticleSystem particle = Instantiate(deathParticle, transform.position, Quaternion.identity);
            particle.Play();
            Destroy(particle.gameObject, particle.main.duration);
        }

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
using UnityEngine;

public class FinalBoss : MonoBehaviour
{
    [Header("Vida")]
    public int maxLives = 3;
    private int currentLives;

    [Header("Movimiento")]
    public float speed = 3f;
    public float stoppingDistance = 2f;

    [Header("Ataque")]
    public float attackCooldown = 1.2f;
    private float lastAttackTime = 0f;

    [Header("Referencias")]
    public Transform player;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sr;

    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        currentLives = maxLives;

        // Busca al jugador automáticamente si no lo has asignado
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    void Update()
    {
        if (isDead || player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // --- MOVIMIENTO ---
        if (distance > stoppingDistance)
        {
            MoveTowardsPlayer();
            animator.SetBool("Walking", true);
        }
        else
        {
            // --- ATAQUE ---
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            animator.SetBool("Walking", false);
            TryAttack();
        }
    }

    void MoveTowardsPlayer()
    {
        float dir = Mathf.Sign(player.position.x - transform.position.x);
        rb.linearVelocity = new Vector2(dir * speed, rb.linearVelocity.y);

        // Flip visual
        if (dir > 0)
            sr.flipX = false;
        else
            sr.flipX = true;
    }

    void TryAttack()
    {
        if (Time.time > lastAttackTime + attackCooldown)
        {
            animator.SetTrigger("Attack");
            lastAttackTime = Time.time;
        }
    }

    // El jugador golpea al boss
    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentLives -= amount;
        animator.SetTrigger("Hit");

        if (currentLives <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        animator.SetTrigger("Die");

        // Desactivar colisión cuando muere
        foreach (var col in GetComponents<Collider2D>())
            col.enabled = false;

        Destroy(gameObject, 1.5f); // esperar animación
    }

    // Detectar golpes del jugador
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack"))
        {
            TakeDamage(1);
        }
    }
}

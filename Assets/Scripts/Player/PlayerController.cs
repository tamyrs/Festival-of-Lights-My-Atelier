using UnityEngine;

/// <summary>
/// Controla o movimento e as animações do player (gato do ateliê).
///
/// Requisitos no GameObject do Player:
///   - SpriteRenderer
///   - Rigidbody2D (Gravity Scale = 0)
///   - Collider2D (BoxCollider2D ou CircleCollider2D)
///   - Animator com o Controller "PlayerAnimatorController"
///
/// As animações devem seguir a nomenclatura:
///   - IDLE / idle_down, idle_left, idle_right, idle_up
///   - RUN  / run_down,  run_left,  run_right,  run_up
///   - ATTACK 1 / attack1_down, attack1_left, attack1_right, attack1_up
///
/// O Animator deve usar parâmetros:
///   - "Horizontal" (float), "Vertical" (float), "Speed" (float), "Attack" (bool)
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Movimento")]
    [Tooltip("Velocidade de movimento do gato em unidades por segundo.")]
    public float moveSpeed = 4f;

    [Header("Referências")]
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator animator;

    private Vector2 moveInput;
    private Vector2 lastMoveDirection = Vector2.down; // olha para baixo por padrão
    private bool isAttacking = false;
    private float attackCooldown = 0.5f;
    private float attackTimer = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        // Garante que a física não puxe o player para baixo (jogo 2D top-down)
        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }
    }

    private void Update()
    {
        // Lê input (WASD / setas / gamepad)
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        moveInput = new Vector2(h, v).normalized;

        // Atualiza a direção para onde o player está olhando (útil para ataques e idle)
        if (moveInput != Vector2.zero)
        {
            lastMoveDirection = moveInput;
        }

        // Ataca com espaço ou clique esquerdo (com cooldown)
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            if (attackTimer <= 0f && !isAttacking)
            {
                StartAttack();
            }
        }

        // Atualiza o timer do ataque
        if (attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;
        }

        // Atualiza parâmetros do Animator
        UpdateAnimator();
    }

    private void FixedUpdate()
    {
        // Move usando Rigidbody para respeitar colisões
        if (!isAttacking && rb != null)
        {
            rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
        }
    }

    private void StartAttack()
    {
        isAttacking = true;
        attackTimer = attackCooldown;

        if (animator != null)
        {
            animator.SetBool("Attack", true);
        }

        // Desativa o ataque após um curto período (sincronizado com a animação)
        Invoke(nameof(EndAttack), 0.3f);
    }

    private void EndAttack()
    {
        isAttacking = false;
        if (animator != null)
        {
            animator.SetBool("Attack", false);
        }
    }

    private void UpdateAnimator()
    {
        if (animator == null) return;

        bool isMoving = moveInput != Vector2.zero;
        animator.SetFloat("Horizontal", moveInput.x);
        animator.SetFloat("Vertical", moveInput.y);
        animator.SetFloat("Speed", isMoving ? 1f : 0f);

        // Para animações de idle direcionais quando parado, usa a última direção
        if (!isMoving)
        {
            animator.SetFloat("Horizontal", lastMoveDirection.x);
            animator.SetFloat("Vertical", lastMoveDirection.y);
        }
    }

    /// <summary>
    /// Retorna a direção atual para onde o player está olhando.
    /// Útil para outros sistemas (ex.: lançar pintura, interagir com objetos).
    /// </summary>
    public Vector2 GetLookDirection()
    {
        return lastMoveDirection;
    }

    /// <summary>
    /// Retorna se o player está se movendo no momento.
    /// </summary>
    public bool IsMoving()
    {
        return moveInput != Vector2.zero;
    }
}

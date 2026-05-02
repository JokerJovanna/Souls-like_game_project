using UnityEngine;

public class Player : MonoBehaviour
{
    private MovementComponent movement;
    private JumpComponent jump;
    private DodgeComponent dodge;
    private StaminaComponent stamina;
    private PlayerHealthComponent health;
    private BlockComponent block;

    public float CurrentHealth => health != null ? health.CurrentHealth : 0;
    public float MaxHealth => health != null ? health.MaxHealth : 0;
    public float CurrentStamina => stamina != null ? stamina.CurrentStamina : 0;
    public float MaxStamina => stamina != null ? stamina.MaxStamina : 0;
    public bool IsDodging => dodge != null && dodge.IsDodging;
    public bool IsBlocking => block != null && block.IsBlocking;

    public event System.Action<float, float> OnHealthChanged;
    public event System.Action<float, float> OnStaminaChanged;
    public event System.Action OnDie;
    public event System.Action OnHealPotionCountChanged;
    private Animator animator;

    void Awake()
    {
        movement = GetComponent<MovementComponent>();
        jump = GetComponent<JumpComponent>();
        dodge = GetComponent<DodgeComponent>();
        stamina = GetComponent<StaminaComponent>();
        health = GetComponent<PlayerHealthComponent>();
        block = GetComponent<BlockComponent>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        gameObject.SetActive(false);

        if (health != null)
        {
            health.OnHealthChanged += (cur, max) => OnHealthChanged?.Invoke(cur, max);
            health.OnDie += () => OnDie?.Invoke();
        }
        if (stamina != null)
        {
            stamina.OnStaminaChanged += (cur, max) => OnStaminaChanged?.Invoke(cur, max);
        }
        if (jump != null)
        {
            jump.OnJump += () => animator?.SetTrigger("JumpTrigger");
        }
    }

    void Update()
    {
        if (animator == null) return;

        float speedValue = movement != null ? movement.HorizontalSpeed : 0f;
        animator.SetFloat("Speed", speedValue);
        animator.SetBool("IsGrounded", jump != null && jump.IsGrounded);
        animator.SetBool("IsDodging", IsDodging);

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("AttackTrigger");
        }
    }
}
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 8f;
    public float jumpImpulse = 9f;
    public float airSpeed = 5f;

    Vector2 moveInput;
    Rigidbody2D rb;
    Animator animator;
    TouchingDirections touchingDirections;
    private Collider2D playerCollider;

    
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;
    [SerializeField] private HealthBarUI healthBar;
    [SerializeField] private GameObject attackHitbox;
    
    private bool isDead = false;
    
    [SerializeField] private float invincibilityDuration = 0.5f;
    private float lastTimeDamaged = -Mathf.Infinity;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private SanityBarUI sanityBar;
    [SerializeField] private float maxSanity = 100;
    [SerializeField] private float currentSanity;

    public float CurrentMoveSpeed { get
    {
        if(touchingDirections.IsGrounded)
        {
            if(IsMoving && !touchingDirections.IsOnWall) 
            {
                return moveSpeed; //Implement air movement speed here :p
            } else {
                return 0;
            } 
        } else {
            return airSpeed;
        }
    }}
    
    
    [SerializeField]
    private bool _isMoving = false;
    public bool IsMoving { get
        {
            return _isMoving;
        } 
        private set 
        {
            _isMoving = value;
            animator.SetBool("isMoving", value);
        }
    }
   

    public bool _isFacingRight = true;
    public bool isFacingRight { get { return _isFacingRight; } private set {
            if(isFacingRight != value) 
            {
                transform.localScale *= new Vector2(-1, 1);
            }
            _isFacingRight = value;

        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        playerCollider = GetComponent<Collider2D>();
        
        currentHealth = maxHealth;
        if (healthBar != null)
            healthBar.SetHealth(currentHealth, maxHealth);

        currentSanity = maxSanity;
        if (sanityBar != null)
            sanityBar.SetSanity(Mathf.RoundToInt(currentSanity), Mathf.RoundToInt(maxSanity));
    }

    [System.Obsolete]
    private void FixedUpdate()
    {
        if (!isDead)
        {
            rb.linearVelocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    //Temp
    private void Update()
    {
        if (Keyboard.current.hKey.wasPressedThisFrame)
        {
            Heal(10);
        }
        if (Keyboard.current.jKey.wasPressedThisFrame)
        {
            TakeDamage(10);
        }
        if (Keyboard.current.kKey.wasPressedThisFrame)
            LoseSanity(10);

        if (Keyboard.current.lKey.wasPressedThisFrame)
            RestoreSanity(10);
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        if (isDead) return;

        moveInput = context.ReadValue<Vector2>();
        IsMoving = moveInput != Vector2.zero;
        SetFacingDirection(moveInput);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (isDead) return;
        if(context.started && touchingDirections.IsGrounded)
        {
            animator.SetTrigger(AnimationStrings.jumpTrigger);
            rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpImpulse);
        }
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if(moveInput.x > 0 && !isFacingRight) 
        {
            isFacingRight = true;
        } else if (moveInput.x < 0 && isFacingRight) 
        {
            isFacingRight = false;
        }
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (isDead || !context.started) return;

        if (!touchingDirections.IsGrounded && rb.linearVelocity.y < 0 && Keyboard.current.sKey.isPressed)
        {
            animator.SetTrigger("downAirAttack");
        }
        else
        {
            animator.SetTrigger(AnimationStrings.attackTrigger);
        }

        SFXManager.Instance.PlayAttackSound();
    }


    public void TakeDamage(int damage)
    {
        if (isDead) return;

        if (Time.time - lastTimeDamaged < invincibilityDuration)
            return;

        lastTimeDamaged = Time.time;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        SFXManager.Instance.PlayPlayerHurt();

        if (healthBar != null)
            healthBar.SetHealth(currentHealth, maxHealth);
        
        StartCoroutine(FlashRed());
        IgnoreEnemyCollision(true);
        Invoke(nameof(EnableEnemyCollision), invincibilityDuration);

        if (currentHealth <= 0)
            Die();
    }

    private void IgnoreEnemyCollision(bool ignore)
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Collider2D enemyCol = enemy.GetComponent<Collider2D>();
            if (enemyCol != null)
            {
                Physics2D.IgnoreCollision(playerCollider, enemyCol, ignore);
            }
        }
    }

    private void EnableEnemyCollision()
    {
        IgnoreEnemyCollision(false);
    }

    public void Heal(int amount)
    {
        if (isDead) return;
            int prev = currentHealth;
            currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        if (healthBar != null)
            healthBar.SetHealth(currentHealth, maxHealth);
    }
    
    private void Die()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        animator.SetBool(AnimationStrings.isDead, true);

        Invoke(nameof(Respawn), 1.0f);
    }
    public void LoseSanity(float amount)
    {
        currentSanity = Mathf.Clamp(currentSanity - amount, 0, maxSanity);
        if (sanityBar != null)
            sanityBar.SetSanity(Mathf.RoundToInt(currentSanity), Mathf.RoundToInt(maxSanity));
    }
    public void RestoreSanity(float amount)
    {
        currentSanity = Mathf.Clamp(currentSanity + amount, 0, maxSanity);
        if (sanityBar != null)
            sanityBar.SetSanity(Mathf.RoundToInt(currentSanity), Mathf.RoundToInt(maxSanity));
    }
    private IEnumerator FlashRed()
    {
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = originalColor;
    }

    public void EnableAttackHitbox()
    {
        attackHitbox.SetActive(true);
        CancelInvoke(nameof(DisableAttackHitbox));
        Invoke(nameof(DisableAttackHitbox), 0.3f);
    }

    public void DisableAttackHitbox()
    {
        attackHitbox.SetActive(false);
    }
    public bool CanPogo()
    {
        return currentSanity >= 25f;
    }

    public void PogoBounce()
    {
        if (currentSanity >= 12.5f)
        {
            currentSanity -= 25f;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpImpulse);

            if (sanityBar != null)
                sanityBar.SetSanity(Mathf.RoundToInt(currentSanity), Mathf.RoundToInt(maxSanity));
        }
        else
        {
            Debug.Log("Not enough sanity to pogoo :(");
        }
    }

    public void Respawn()
    {
        isDead = false;
        animator.SetBool(AnimationStrings.isDead, false);

        RespawnManager.Respawn(gameObject);

        currentHealth = maxHealth;
        if (healthBar != null)
            healthBar.SetHealth(currentHealth, maxHealth);
    }

}

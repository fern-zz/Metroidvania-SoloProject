using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    [SerializeField] private SpriteRenderer spriteRenderer;
    private bool isDead = false;

    private Animator animator;

    private void Awake()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;

        if (spriteRenderer != null)
            StartCoroutine(FlashGray());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator FlashGray()
    {
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = Color.gray;
        yield return new WaitForSeconds(0.15f);
        spriteRenderer.color = originalColor;
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        if (animator != null)
        {
            animator.SetBool("isDead", true); 
        }

        if (SFXManager.Instance != null)
        {
            SFXManager.Instance.PlayEnemyDeath(); 
        }

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
        }

    }
    public void OnEnemyDeathFinished()
    {
        Destroy(gameObject);
    }

}

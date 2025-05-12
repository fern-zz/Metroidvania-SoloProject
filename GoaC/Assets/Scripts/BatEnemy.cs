using UnityEngine;
using System.Collections;

public class BatEnemy : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2.75f;
    public float detectionRadius = 8f;
    public float hoverAmplitude = 0.5f;
    public float hoverFrequency = 2f;
    private float timeSinceLastSeen = 0f;
    [SerializeField] private float returnDelay = 3f;
    [SerializeField] private float returnSpeed = 2f;
    private bool returningHome = false;


    [Header("Combat")]
    public int contactDamage = 10;
    public Color flashColor = Color.red;
    public float flashDuration = 0.2f;
    [SerializeField] private int maxHealth = 25;
    private int currentHealth;

    [Header("References")]
    public SpriteRenderer spriteRenderer;

    private Transform player;
    private Animator animator;
    private Rigidbody2D rb;
    private Vector3 startPos;
    private Color originalColor;
    private bool isDead = false;
    private bool isFluttering = false;

    void Start()
    {
        foreach (var otherBat in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (otherBat != this.gameObject)
            {
                Collider2D otherCol = otherBat.GetComponent<Collider2D>();
                Collider2D myCol = GetComponent<Collider2D>();
                if (otherCol && myCol)
                    Physics2D.IgnoreCollision(myCol, otherCol);
            }
        }
        currentHealth = maxHealth;

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;
        originalColor = spriteRenderer.color;
    }

    void Update()
    {
        if (isDead || player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance < detectionRadius)
        {
            timeSinceLastSeen = 0f;
            returningHome = false;

            Vector2 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = direction * moveSpeed;

            if (!isFluttering)
            {
                SFXManager.Instance?.PlayBatFlutter();
                isFluttering = true;
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            timeSinceLastSeen += Time.deltaTime;

            if (timeSinceLastSeen >= returnDelay)
            {
                returningHome = true;
            }

            if (returningHome)
            {
                Vector3 directionToHome = (startPos - transform.position);
                float distanceToHome = directionToHome.magnitude;

                if (distanceToHome > 0.1f)
                {
                    Vector3 move = directionToHome.normalized * returnSpeed * Time.deltaTime;
                    transform.position += move;
                }
                else
                {
                    returningHome = false;
                }
            }
            else
            {

                transform.position += Vector3.up * Mathf.Sin(Time.time * hoverFrequency) * hoverAmplitude * Time.deltaTime;
            }

            isFluttering = false;
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;

        if (other.CompareTag("Player"))
        {
            PlayerController pc = other.GetComponent<PlayerController>();
            if (pc != null)
            {
                pc.TakeDamage(contactDamage);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

            if (currentHealth <= 0)
        {
            isDead = true;

            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.simulated = false;

            Collider2D col = GetComponent<Collider2D>();
            if (col != null) col.enabled = false;

            animator.SetBool("isDead", true);

            SFXManager.Instance?.PlayBatDeath();

            StartCoroutine(FlashAndFade());
        }
    }


    private IEnumerator FlashAndFade()
    {
        spriteRenderer.color = flashColor;
        yield return new WaitForSeconds(flashDuration);

        float duration = 0.5f;
        float timer = 0f;
        Color original = spriteRenderer.color;

        while (timer < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, timer / duration);
            spriteRenderer.color = new Color(original.r, original.g, original.b, alpha);
            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}

using UnityEngine;

public class PlayerAttackHitbox : MonoBehaviour
{
    [SerializeField] private int damageAmount = 25;
    [SerializeField] private PlayerController playerController;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!playerController.CanPogo()) return;

        if (other.TryGetComponent<EnemyHealth>(out var enemy))
        {
            enemy.TakeDamage(damageAmount);
            playerController.PogoBounce();
        }
        else if (other.TryGetComponent<BatEnemy>(out var bat))
        {
            bat.TakeDamage(damageAmount);
            playerController.PogoBounce();
        }
    }


    private void CheckForPogoBounce()
    {
        var rb = playerController.GetComponent<Rigidbody2D>();
        if (rb.linearVelocity.y < 0)
        {
            playerController.PogoBounce();
        }
    }
}

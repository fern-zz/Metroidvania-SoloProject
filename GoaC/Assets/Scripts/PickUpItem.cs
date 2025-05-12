using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    public enum PickupType { Health, Sanity }
    public PickupType type;
    public int amount = 10;
    public GameObject visualToDisable;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                if (type == PickupType.Health)
                {
                    player.Heal(amount);
                    SFXManager.Instance.PlayHealSound();
                }
                else if (type == PickupType.Sanity)
                {
                    player.RestoreSanity(amount);
                    SFXManager.Instance.PlaySanityNote();
                }

                if (visualToDisable != null)
                    visualToDisable.SetActive(false);

                Destroy(gameObject, 0.1f);
            }
        }
    }
}

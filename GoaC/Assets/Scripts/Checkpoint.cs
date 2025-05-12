using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private ParticleSystem checkpointParticles;
    private bool activated = false;

    private void Awake()
    {
        if (checkpointParticles != null)
            checkpointParticles.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            RespawnManager.SetCheckpoint(this);
        }
    }

    public void Activate()
    {
        if (checkpointParticles != null)
        {
            checkpointParticles.gameObject.SetActive(true);
        }
    }

    public void Deactivate()
    {
        if (checkpointParticles != null)
        {
            checkpointParticles.gameObject.SetActive(false);
        }
    }
}

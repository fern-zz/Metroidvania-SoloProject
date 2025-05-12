using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public static Checkpoint currentCheckpoint;

    private void Awake()
    {
        if (currentCheckpoint == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                currentCheckpoint = null;
        }
    }

    public static void SetCheckpoint(Checkpoint newCheckpoint)
    {
        if (currentCheckpoint != null)
            currentCheckpoint.Deactivate();

        currentCheckpoint = newCheckpoint;
        currentCheckpoint.Activate();
    }

    public static void Respawn(GameObject player)
    {
        if (currentCheckpoint != null)
        {
            player.transform.position = currentCheckpoint.transform.position;
        }
    }
}

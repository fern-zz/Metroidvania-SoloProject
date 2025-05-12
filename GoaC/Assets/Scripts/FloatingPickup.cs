using UnityEngine;

public class FloatingPickup : MonoBehaviour
{
    public float floatStrength = 0.25f;
    public float floatSpeed = 2f;

    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatStrength;
        transform.position = startPos + new Vector3(0, yOffset, 0);
    }
}

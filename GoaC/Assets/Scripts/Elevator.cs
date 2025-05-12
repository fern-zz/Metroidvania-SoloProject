using UnityEngine;

public class ElevatorTriggerUpDown : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 3f;

    private bool isPlayerOn = false;
    private Vector3 targetPosition;

    void Start()
    {
        transform.position = pointA.position;
        targetPosition = pointA.position;
    }

    void Update()
    {
        if (isPlayerOn && targetPosition != pointB.position && AtPosition(pointA.position))
        {
            targetPosition = pointB.position;
        }
        else if (!isPlayerOn && targetPosition != pointA.position && AtPosition(pointB.position))
        {
            targetPosition = pointA.position;
        }

        if (!AtPosition(targetPosition))
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
    }

    private bool AtPosition(Vector3 pos)
    {
        return Vector3.Distance(transform.position, pos) < 0.01f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            isPlayerOn = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            isPlayerOn = false;
    }
}

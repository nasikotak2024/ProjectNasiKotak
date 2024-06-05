using UnityEngine;
using System.Collections;

public class AdvancedAIChaseAndReturn2D : MonoBehaviour
{
    public Transform player; // Referensi ke pemain
    public float detectionRadius = 10f; // Jarak deteksi AI
    public float moveSpeed = 2f; // Kecepatan gerak AI
    public float returnSpeed = 2f; // Kecepatan kembali ke posisi awal
    public LayerMask obstacleLayer; // Layer untuk mendeteksi halangan

    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 startPosition;
    private bool isReturning = false;
    private Coroutine returnCoroutine;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Mengambil komponen Rigidbody2D
        startPosition = rb.position; // Mengingat posisi awal AI
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius)
        {
            if (isReturning && returnCoroutine != null)
            {
                StopCoroutine(returnCoroutine); // Menghentikan proses kembali ke posisi semula
                isReturning = false;
            }

            Vector2 direction = (player.position - transform.position).normalized;
            direction = AvoidObstacles(direction);

            movement = direction * moveSpeed;
        }
        else if (!isReturning)
        {
            movement = Vector2.zero; // AI berhenti bergerak
            returnCoroutine = StartCoroutine(ReturnToStartPosition());
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * Time.fixedDeltaTime);
    }

    private IEnumerator ReturnToStartPosition()
    {
        isReturning = true;
        while (Vector2.Distance(rb.position, startPosition) > 0.1f)
        {
            Vector2 direction = (startPosition - rb.position).normalized;
            direction = AvoidObstacles(direction);

            movement = direction * returnSpeed;

            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            if (distanceToPlayer <= detectionRadius)
            {
                isReturning = false;
                yield break; // Keluar dari coroutine jika pemain memasuki zona deteksi
            }

            yield return null;
        }
        movement = Vector2.zero;
        rb.position = startPosition;
        isReturning = false;
    }

    private Vector2 AvoidObstacles(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1f, obstacleLayer);
        if (hit.collider != null)
        {
            // Hit an obstacle, try to steer around it
            Vector2 perpDirection = Vector2.Perpendicular(direction);
            Vector2 newDirection1 = direction + perpDirection;
            Vector2 newDirection2 = direction - perpDirection;

            if (!IsObstacleInPath(newDirection1))
            {
                return newDirection1.normalized;
            }
            else if (!IsObstacleInPath(newDirection2))
            {
                return newDirection2.normalized;
            }
        }

        return direction; // No obstacles, continue in the original direction
    }

    private bool IsObstacleInPath(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1f, obstacleLayer);
        return hit.collider != null;
    }

    // Visualisasi jangkauan deteksi di Scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}

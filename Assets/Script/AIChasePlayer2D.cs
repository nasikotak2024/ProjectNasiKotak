using UnityEngine;
using System.Collections;

public class AIChaseAndReturn2D : MonoBehaviour
{
    public Transform player; // Referensi ke pemain
    public float detectionRadius = 10f; // Jarak deteksi AI
    public float moveSpeed = 2f; // Kecepatan gerak AI
    public float returnSpeed = 2f; // Kecepatan kembali ke posisi awal

    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 startPosition;
    private bool isReturning = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Mengambil komponen Rigidbody2D
        startPosition = rb.position; // Mengingat posisi awal AI
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius && !isReturning)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            movement = direction * moveSpeed;
        }
        else if (!isReturning)
        {
            movement = Vector2.zero; // AI berhenti bergerak
            StartCoroutine(ReturnToStartPosition());
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
            movement = direction * returnSpeed;
            yield return null;
        }
        movement = Vector2.zero;
        rb.position = startPosition;
        isReturning = false;
    }

    // Visualisasi jangkauan deteksi di Scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}

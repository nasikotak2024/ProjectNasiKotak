using UnityEngine;

public class Topdown : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movement;

    private const string LVertical = "LastVertical";
    private const string LHorizontal = "LastHorizontal";

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Reset vektor pergerakan
        movement = Vector2.zero;

        // Mendapatkan input horizontal (kanan/kiri)
        float moveInputX = Input.GetAxisRaw("Horizontal");
        // Mendapatkan input vertikal (atas/bawah)
        float moveInputY = Input.GetAxisRaw("Vertical");


        // Memperbarui vektor pergerakan sesuai input
        if (moveInputX != 0)
        {
            movement.x = moveInputX;
        }
        else if (moveInputY != 0)
        {
            movement.y = moveInputY;
        }

        // Set parameters to control the animations
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);

        if (movement != Vector2.zero)
        {
            animator.SetFloat(LHorizontal, movement.x);
            animator.SetFloat(LVertical, movement.y);
        }
    }

    void FixedUpdate()
    {
        // Menggerakkan karakter menggunakan Rigidbody
        Vector2 targetPosition = rb.position + movement * moveSpeed * Time.fixedDeltaTime;

        // Menggerakkan karakter
        rb.MovePosition(Vector2.Lerp(rb.position, targetPosition, 0.5f));        
    }
}

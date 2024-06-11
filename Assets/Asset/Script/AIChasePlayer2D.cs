using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Aoiti.Pathfinding; // Import the pathfinding library

public class AdvancedAIChaseAndReturn2D : MonoBehaviour
{
    public Transform player; // Reference to the player
    public float detectionRadius = 10f; // Detection radius for the AI
    public float moveSpeed = 2f; // Movement speed of the AI

    public Image gameOverImage; // Reference to UI Image for game over
    public AudioSource gameOverAudio; // Reference to AudioSource for game over sound

    private Rigidbody2D rb;
    private Vector2 startPosition;
    private bool isReturning = false;
    private Coroutine returnCoroutine;

    [Header("Pathfinding")]
    [SerializeField] float gridSize = 0.5f; // Increase patience or gridSize for larger maps
    private Pathfinder<Vector2> pathfinder;
    private List<Vector2> pathLeftToGo = new List<Vector2>();
    [SerializeField] LayerMask obstacles;
    [SerializeField] bool searchShortcut = false;
    [SerializeField] bool drawDebugLines;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
        startPosition = rb.position; // Remember the AI's starting position
        pathfinder = new Pathfinder<Vector2>(GetDistance, GetNeighbourNodes, 1000); // Initialize pathfinder

        if (gameOverImage != null)
        {
            gameOverImage.gameObject.SetActive(false); // Disable game over image at start
        }
        else
        {
            Debug.LogError("GameOver Image is not assigned in the Inspector.");
        }

        if (gameOverAudio == null)
        {
            Debug.LogError("GameOver Audio is not assigned in the Inspector.");
        }
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius)
        {
            if (isReturning && returnCoroutine != null)
            {
                StopCoroutine(returnCoroutine); // Stop returning to start position
                isReturning = false;
            }

            GetMoveCommand(player.position);
        }
        else if (!isReturning)
        {
            returnCoroutine = StartCoroutine(ReturnToStartPosition());
        }

        if (pathLeftToGo.Count > 0) // If the target is not yet reached
        {
            Vector3 dir = (Vector3)pathLeftToGo[0] - transform.position;
            transform.position += dir.normalized * moveSpeed * Time.deltaTime;
            if (((Vector2)transform.position - pathLeftToGo[0]).sqrMagnitude < (moveSpeed * Time.deltaTime) * (moveSpeed * Time.deltaTime))
            {
                transform.position = pathLeftToGo[0];
                pathLeftToGo.RemoveAt(0);
            }
        }

        if (drawDebugLines)
        {
            for (int i = 0; i < pathLeftToGo.Count - 1; i++) // Visualize your path in the scene view
            {
                Debug.DrawLine(pathLeftToGo[i], pathLeftToGo[i + 1], Color.red);
            }
        }
    }

    private IEnumerator ReturnToStartPosition()
    {
        isReturning = true;
        while (Vector2.Distance(rb.position, startPosition) > 0.1f)
        {
            GetMoveCommand(startPosition);
            yield return new WaitForSeconds(0.5f); // Wait for a short time before recalculating path
        }
        isReturning = false;
    }

    void GetMoveCommand(Vector2 target)
    {
        Vector2 closestNode = GetClosestNode(transform.position);
        List<Vector2> path;
        if (pathfinder.GenerateAstarPath(closestNode, GetClosestNode(target), out path)) // Generate path between two points on grid that are close to the transform position and the assigned target
        {
            if (searchShortcut && path.Count > 0)
                pathLeftToGo = ShortenPath(path);
            else
            {
                pathLeftToGo = new List<Vector2>(path);
                pathLeftToGo.Add(target);
            }
        }
    }

    Vector2 GetClosestNode(Vector2 target)
    {
        return new Vector2(Mathf.Round(target.x / gridSize) * gridSize, Mathf.Round(target.y / gridSize) * gridSize);
    }

    float GetDistance(Vector2 A, Vector2 B)
    {
        return (A - B).sqrMagnitude; // Uses square magnitude to lessen the CPU time
    }

    Dictionary<Vector2, float> GetNeighbourNodes(Vector2 pos)
    {
        Dictionary<Vector2, float> neighbours = new Dictionary<Vector2, float>();
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (i == 0 && j == 0) continue;

                Vector2 dir = new Vector2(i, j) * gridSize;
                if (!Physics2D.Linecast(pos, pos + dir, obstacles))
                {
                    neighbours.Add(GetClosestNode(pos + dir), dir.magnitude);
                }
            }
        }
        return neighbours;
    }

    List<Vector2> ShortenPath(List<Vector2> path)
    {
        List<Vector2> newPath = new List<Vector2>();

        for (int i = 0; i < path.Count; i++)
        {
            newPath.Add(path[i]);
            for (int j = path.Count - 1; j > i; j--)
            {
                if (!Physics2D.Linecast(path[i], path[j], obstacles))
                {
                    i = j;
                    break;
                }
            }
            newPath.Add(path[i]);
        }
        newPath.Add(path[path.Count - 1]);
        return newPath;
    }

    // Handle collision with player
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameOver();
        }
    }

    // Game over function
    private void GameOver()
    {
        if (gameOverImage != null)
        {
            gameOverImage.gameObject.SetActive(true);
        }

        if (gameOverAudio != null)
        {
            gameOverAudio.Play();
        }

        Time.timeScale = 0f; // Stop the game
    }

    // Draw detection radius in the scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}

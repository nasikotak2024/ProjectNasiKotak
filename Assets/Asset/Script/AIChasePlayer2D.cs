using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Aoiti.Pathfinding;

public class AdvancedAIChaseAndReturn2D : MonoBehaviour
{
    private PlayerHide playerHideScript;
    public Transform player;
    public float detectionRadius = 10f;
    public float moveSpeed = 2f;


    private Rigidbody2D rb;
    private Vector2 startPosition;
    private bool isReturning = false;
    private Coroutine returnCoroutine;
    private int patrolIndex = 0;
    private int currentPatrolIndex = 0;
    public float patrolPointDistance = 0.2f;
    public float patrolWaitTime = 3f;
    public float chaseStopDistance = 15f;

    [Header("Pathfinding")]
    [SerializeField] float gridSize = 0.5f;
    private Pathfinder<Vector2> pathfinder;
    private List<Vector2> pathLeftToGo = new List<Vector2>();
    [SerializeField] LayerMask obstacles;
    [SerializeField] bool searchShortcut = false;
    [SerializeField] bool drawDebugLines;

    [Header("Patrol Points")]
    public List<Transform> patrolPoints;

    private CapsuleCollider2D capsuleCollider;

    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        playerHideScript = player.GetComponent<PlayerHide>();

        rb = GetComponent<Rigidbody2D>();
        startPosition = rb.position;
        pathfinder = new Pathfinder<Vector2>(GetDistance, GetNeighbourNodes, 1000);

       
    }

    void Update()
    {

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius && !playerHideScript.IsHidden())
        {
            if (isReturning && returnCoroutine != null)
            {
                StopCoroutine(returnCoroutine);
                isReturning = false;
            }

            GetMoveCommand(player.position);
        }
        else if (!isReturning)
        {
            if (patrolPoints.Count > 0)
            {
                Patrol();
            }
            else
            {
                returnCoroutine = StartCoroutine(ReturnToStartPosition());
            }
        }

        if (pathLeftToGo.Count > 0)
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
            for (int i = 0; i < pathLeftToGo.Count - 1; i++)
            {
                Debug.DrawLine(pathLeftToGo[i], pathLeftToGo[i + 1], Color.red);
            }
        }
    }

    private void Patrol()
    {
        if (pathLeftToGo.Count == 0)
        {
            GetMoveCommand(patrolPoints[patrolIndex].position);
            patrolIndex = (patrolIndex + 1) % patrolPoints.Count;
        }
    }

    private IEnumerator ReturnToStartPosition()
    {
        isReturning = true;
        Transform targetPatrolPoint = patrolPoints[currentPatrolIndex];

        while (Vector2.Distance(transform.position, targetPatrolPoint.position) > patrolPointDistance)
        {
            Vector2 direction = ((Vector2)targetPatrolPoint.position - (Vector2)transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, targetPatrolPoint.position, moveSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(patrolWaitTime);

        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
        isReturning = false;
    }

    private IEnumerator ReturnToPatrolStart()
    {
        isReturning = true;
        Vector2 patrolStart = patrolPoints[0].position;
        while (Vector2.Distance(rb.position, patrolStart) > 0.1f)
        {
            GetMoveCommand(patrolStart);
            yield return new WaitForSeconds(0.5f);
        }
        isReturning = false;
    }

    void GetMoveCommand(Vector2 target)
    {
        Vector2 closestNode = GetClosestNode(transform.position);
        List<Vector2> path;
        if (pathfinder.GenerateAstarPath(closestNode, GetClosestNode(target), out path))
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
        return (A - B).sqrMagnitude;
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
    
    public void PlayerHiding(bool isHiding)
    {
        if (isHiding)
        {
            if (returnCoroutine != null)
            {
                StopCoroutine(returnCoroutine);
                returnCoroutine = StartCoroutine(ReturnToPatrolStart());
            }
        }
    }

    // Draw detection radius in the scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}

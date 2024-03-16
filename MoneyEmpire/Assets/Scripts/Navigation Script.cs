using UnityEngine;
using UnityEngine.AI;

public class PatrolScript : MonoBehaviour
{
    private NavMeshAgent agent;
    private int currentWaypoint = 0;
    private Vector3[] waypoints;

    public int minWaypoints = 3;
    public int maxWaypoints = 10;
    public float wanderRadius = 20f;
    public float minDistanceFromObstacles = 2f; // Minimum distance from box colliders

    public GameObject[] sidewalkObjects; // Assign the sidewalk objects here
    public LayerMask obstacleMask; // Layer mask for obstacles

    private float timeAtCurrentPosition = 0f;
    private float timeThreshold = 0.2f; // Time threshold for changing position

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GenerateWaypoints();
        agent.SetDestination(waypoints[currentWaypoint]);
    }

    private void Update()
{
    if (agent.remainingDistance <= agent.stoppingDistance + 1f) // Adding tolerance of +-1
    {
        timeAtCurrentPosition += Time.deltaTime;
        if (timeAtCurrentPosition >= timeThreshold)
        {
            ChangePosition();
        }
    }
    else
    {
        timeAtCurrentPosition = 0f;
    }
}


    private void ChangePosition()
    {
        Vector3 newPosition = FindNewPosition();
        if (newPosition != Vector3.zero)
        {
            agent.SetDestination(newPosition);
            timeAtCurrentPosition = 0f;
        }
    }

    private Vector3 FindNewPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += waypoints[currentWaypoint];

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, NavMesh.AllAreas))
        {
            if (IsPositionOnSidewalk(hit.position) && !IsPositionNearObstacle(hit.position))
            {
                return hit.position;
            }
        }
        return Vector3.zero;
    }

    private bool IsPositionOnSidewalk(Vector3 position)
    {
        foreach (GameObject sidewalkObject in sidewalkObjects)
        {
            Collider[] colliders = sidewalkObject.GetComponentsInChildren<Collider>();
            foreach (Collider collider in colliders)
            {
                if (collider.bounds.Contains(position))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool IsPositionNearObstacle(Vector3 position)
    {
        Collider[] obstacles = Physics.OverlapBox(position, Vector3.one * minDistanceFromObstacles, Quaternion.identity, obstacleMask);
        return obstacles.Length > 0;
    }

    private void GenerateWaypoints()
    {
        int numWaypoints = Random.Range(minWaypoints, maxWaypoints + 1);
        waypoints = new Vector3[numWaypoints];

        Vector3 initialPosition = agent.transform.position;
        waypoints[0] = initialPosition;

        for (int i = 1; i < numWaypoints; i++)
        {
            Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
            randomDirection += waypoints[i - 1];

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, NavMesh.AllAreas))
            {
                if (IsPositionOnSidewalk(hit.position) && !IsPositionNearObstacle(hit.position))
                {
                    waypoints[i] = hit.position;
                }
                else
                {
                    waypoints[i] = waypoints[i - 1];
                }
            }
            else
            {
                waypoints[i] = waypoints[i - 1];
            }
        }
    }
}

using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent))]
public class AdvancedRaceLogic : MonoBehaviour
{
    [SerializeField] private float currentSpeed;
    
    private int currentWaypointIndex = 0;
    private NavMeshAgent agent;
    private AIRacer racerStats;
    private PositionTracker tracker;
    private Vector3 waypointOffset;
    
    public CustomGraph<Transform> waypoints;
    private Transform currentWaypoint;
    private void Awake()
    {
        tracker = GetComponent<PositionTracker>();
        agent = GetComponent<NavMeshAgent>();
        racerStats = GetComponent<AIRacer>();
        
        if (racerStats == null)
        {
            Debug.LogError(gameObject.name + " has no AIRacer script attached!");
        }
    }
    
    private void Update()
    {
        RotateTowardsMovementDirection();
        HandleCorneringSlowdown();
        currentSpeed = agent.velocity.magnitude;
    }

    public void BeginRace(Transform startWaypoint)
    {
        agent.enabled = true;
        if (waypoints == null || startWaypoint == null || !waypoints.ContainsVertex(startWaypoint))
        {
            Debug.LogError(name + ": Invalid or missing start waypoint.");
            return;
        }

        currentWaypoint = startWaypoint;
        
        tracker.CurrentWaypoint = currentWaypoint;

        AdvancedRaceManager.Instance.RegisterRacer(tracker);
        ApplyRacerStats();
        SetDestination(currentWaypoint);
    }
    private void ApplyRacerStats()
    {
        agent.speed = racerStats.TopSpeed;
        agent.acceleration = racerStats.Acceleration;
        agent.angularSpeed = racerStats.TurnRate;
    }
    
    private void SetDestination(Transform target)
    {
        Vector2 randomOffset = Random.insideUnitCircle * 3f;
        waypointOffset = new Vector3(randomOffset.x, 0f, randomOffset.y);

        Vector3 destination = target.position + waypointOffset;
        agent.SetDestination(destination);

        tracker.CurrentWaypoint = target;
    }

    private void AdvanceWaypoint(GameObject racer)
    {
        if (racer != gameObject || currentWaypoint == null)
            return;
        
        tracker.TotalWaypointsPassed++;

        List<Transform> nextOptions = waypoints.GetConnectedVertex(currentWaypoint);

        if (nextOptions == null || nextOptions.Count == 0)
        {
            Debug.LogWarning(name + "No next waypoints from {currentWaypoint.name}");
            return;
        }

        currentWaypoint = nextOptions[Random.Range(0, nextOptions.Count)];
        SetDestination(currentWaypoint);
    }

    
    private void RotateTowardsMovementDirection()
    {
        if (agent.hasPath)
        {
            Vector3 direction = (agent.steeringTarget - transform.position).normalized;

            if (direction.sqrMagnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                float smoothTurnRate = 5f;
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * smoothTurnRate);
            }
        }
    }

    
    private void HandleCorneringSlowdown()
    {
        if (agent.hasPath)
        {
            Vector3 desiredDirection = agent.desiredVelocity.normalized;
            float angle = Vector3.Angle(transform.forward, desiredDirection);

            float slowFactor = Mathf.InverseLerp(0f, 60f, angle);

            float targetSpeed = Mathf.Lerp(racerStats.TopSpeed, racerStats.TopSpeed * racerStats.CorneringSlowdownMultiplier, slowFactor);
            
            agent.speed = Mathf.Lerp(agent.speed, targetSpeed, Time.deltaTime * 5f);
        }
    }
    
    private void OnEnable()
    {
        Waypoint.OnWaypointPassed += AdvanceWaypoint;
    }

    private void OnDisable()
    {
        Waypoint.OnWaypointPassed -= AdvanceWaypoint;
    }
}
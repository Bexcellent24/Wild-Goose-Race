using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent))]
public class BeginnerRaceLogic : MonoBehaviour
{
    [SerializeField] private float currentSpeed;
    
    private int currentWaypointIndex = 0;
    private NavMeshAgent agent;
    private AIRacer racerStats;
    private PositionTracker tracker;
    private Vector3 waypointOffset;
    
    public CustomLinkedList<Transform> waypoints; //assigned by the BeginnerRaceManager when they are spawaned
    
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

    public void BeginRace()
    {
        agent.enabled = true;
        if (waypoints == null || waypoints.IsEmpty)
        {
            Debug.LogError(gameObject.name + " has no waypoints assigned!");
            return;
        }
        
        // tells the BeginnerRaceManager that this racer is all set up and should be added to the list to track their position
        BeginnerRaceManager.Instance.RegisterRacer(tracker); 
        
        //applies the stats of the type of racer to this nav mesh agent
        ApplyRacerStats();
        
        //starts the going 
        SetDestinationToCurrentWaypoint();
    }
    
    private void ApplyRacerStats()
    {
        if (racerStats != null)
        {
            agent.speed = racerStats.TopSpeed;
            agent.acceleration = racerStats.Acceleration;
            agent.angularSpeed = racerStats.TurnRate;
        }
    }
    private void SetDestinationToCurrentWaypoint()
    {
        if (!waypoints.IsEmpty && currentWaypointIndex < waypoints.Size)
        {
            Vector3 target = waypoints[currentWaypointIndex].position;
            agent.SetDestination(target + waypointOffset); //small offset so they dont line up like ducks.... they are geese. not ducks
            tracker.CurrentWaypoint = waypoints[currentWaypointIndex]; //for position purposes
        }
        else
        {
            Debug.LogWarning(gameObject.name + ": Invalid waypoint index " + currentWaypointIndex);
        }
    }

    private void AdvanceWaypoint(GameObject racer)
    {
        if (racer == this.gameObject)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex == waypoints.Size)
            {
                Debug.Log("something funky going on " + currentWaypointIndex);
                currentWaypointIndex = 0;
            }
            tracker.TotalWaypointsPassed++; // for position purposes
            
            Vector2 randomOffset = Random.insideUnitCircle * 3f; //new offset
            waypointOffset = new Vector3(randomOffset.x, 0, randomOffset.y);
            
            SetDestinationToCurrentWaypoint(); //next point! GO!
        }
    }

    private void Update()
    {
        RotateTowardsMovementDirection(); //makes them look where they are going.... somewhat smoothly
        HandleCorneringSlowdown(); //my attempt at some custom controlling corner smoothing
        
        currentSpeed = agent.velocity.magnitude; //to see if stuff was working in the editor
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
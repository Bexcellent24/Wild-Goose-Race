using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRaceLogic : MonoBehaviour
{
    private enum RaceMode { None, Beginner, Advanced }
    private RaceMode raceMode = RaceMode.None;

    private CustomLinkedList<Transform> waypointsList;
    private CustomGraph<Transform> waypointsGraph;
    [SerializeField] private int currentWaypointIndex = 0;

    private Transform currentGraphVertex;
    private PositionTracker tracker;

    private void Awake()
    {
        tracker = GetComponent<PositionTracker>();
    }

    private void AdvanceWaypoint(GameObject racer)
    {
        if (racer != this.gameObject) return;

        switch (raceMode)
        {
            case RaceMode.Beginner:
                
                currentWaypointIndex++;

                if (currentWaypointIndex >= waypointsList.Size)
                {
                    Debug.Log("Resetting Index for player");
                    currentWaypointIndex = 0;
                }

                tracker.CurrentWaypoint = waypointsList[currentWaypointIndex];
                tracker.TotalWaypointsPassed++;
                break;

            case RaceMode.Advanced:
                
                currentWaypointIndex++;
                
                List<Transform> nextOptions = waypointsGraph.GetConnectedVertex(currentGraphVertex);
        
                if (nextOptions == null || nextOptions.Count == 0)
                {
                    Debug.LogWarning("No neighbors to move to in graph.");
                    return;
                }
                
                tracker.CurrentWaypoint = nextOptions[0];
                currentGraphVertex = nextOptions[0];
                Debug.Log("Players Transfrom : " + tracker.CurrentWaypoint);
                tracker.TotalWaypointsPassed++;
                break;

            default:
                Debug.LogWarning("Race mode not set.");
                break;
        }
    }


    private void SetupBeginnerWaypoints(CustomLinkedList<Transform> waypoints)
    {
        waypointsList = waypoints;
        raceMode = RaceMode.Beginner;
        currentWaypointIndex = 0;

        if (waypointsList != null && !waypointsList.IsEmpty)
        {
            tracker.CurrentWaypoint = waypointsList[currentWaypointIndex];
        }

        BeginnerRaceManager.Instance.RegisterRacer(tracker);
    }

    private void SetupAdvancedWaypoints(CustomGraph<Transform> waypoints, Transform firstVertex)
    {
        waypointsGraph = waypoints;
        currentGraphVertex = firstVertex;
        raceMode = RaceMode.Advanced;

        tracker.CurrentWaypoint = currentGraphVertex;
        AdvancedRaceManager.Instance.RegisterRacer(tracker);
    }
    
    
    private void HandleBranchRedirect(Transform newWaypoint)
    {
        tracker.CurrentWaypoint = newWaypoint;
        currentGraphVertex = newWaypoint;

        Debug.Log("Player redirected to:" + newWaypoint.name);
    }

    private void OnEnable()
    {
        BeginnerRaceManager.OnWaypointSetup += SetupBeginnerWaypoints;
        AdvancedRaceManager.OnWaypointSetup += SetupAdvancedWaypoints;
        Waypoint.OnWaypointPassed += AdvanceWaypoint;
        BranchRedirector.OnRedirectTriggered += HandleBranchRedirect;
        
    }

    private void OnDisable()
    {
        BeginnerRaceManager.OnWaypointSetup -= SetupBeginnerWaypoints;
        AdvancedRaceManager.OnWaypointSetup -= SetupAdvancedWaypoints;
        Waypoint.OnWaypointPassed -= AdvanceWaypoint;
        BranchRedirector.OnRedirectTriggered -= HandleBranchRedirect;
    }
}

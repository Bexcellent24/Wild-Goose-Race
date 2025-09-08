using System;
using UnityEngine;

public class PositionTracker : MonoBehaviour
{
    [SerializeField] private int totalWaypointsPassed;
    [SerializeField] private Transform currentWaypoint;
    
    public int TotalWaypointsPassed { get;  set; }
    public Transform CurrentWaypoint { get; set; }
    
    private void FixedUpdate()
    {
        //just for keeping track of things in the editor 
        totalWaypointsPassed = TotalWaypointsPassed;
        currentWaypoint = CurrentWaypoint;
    }
}

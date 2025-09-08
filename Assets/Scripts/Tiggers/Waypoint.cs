using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public delegate void WaypointPassedAction(GameObject racer);
    public static event WaypointPassedAction OnWaypointPassed;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Racer") || other.CompareTag("Player"))
        {
            PositionTracker tracker = other.GetComponent<PositionTracker>();

            if (tracker.CurrentWaypoint == this.gameObject.transform)
            {
                OnWaypointPassed?.Invoke(other.gameObject);
            }
            
        }
    }
}

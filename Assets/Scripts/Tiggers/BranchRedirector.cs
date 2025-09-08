using UnityEngine;

public class BranchRedirector : MonoBehaviour
{
    public delegate void RedirectTriggerAction(Transform branch);
    public static event RedirectTriggerAction OnRedirectTriggered;
    [SerializeField] private Transform redirectWaypoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnRedirectTriggered?.Invoke(redirectWaypoint);
        }
    }
}


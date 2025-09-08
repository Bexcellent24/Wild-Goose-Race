using UnityEngine;

public class LapCounter : MonoBehaviour
{
    public delegate void LapTriggeredAction();
    public static event LapTriggeredAction OnLapTriggered;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnLapTriggered?.Invoke(); 
        }
    }
}

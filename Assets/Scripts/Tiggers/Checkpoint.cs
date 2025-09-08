using UnityEngine;
using System;
using Unity.VisualScripting;

public class Checkpoint : MonoBehaviour
{
    public delegate void CheckpointPassedAction(float time, GameObject checkpoint);
    public static event CheckpointPassedAction OnCheckpointPassed;
    
    [SerializeField] private float timeBonus = 5f; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnCheckpointPassed?.Invoke(timeBonus, this.gameObject); 
            SFXManager.Instance?.PlayGlobalSound("Checkpoint", .9f);
            Debug.Log("Checkpoint Passed");
        }
    }
    
}
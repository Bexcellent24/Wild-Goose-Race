using System;
using UnityEngine;

public class GooseStateTrigger : MonoBehaviour
{
    public static event Action<IGooseState> OnGooseTrigger;
    public static event Action<GameObject, GooseStateType> OnAIRacerTrigger;
    //public enum GooseStateType { Running, Swimming, Flying}
    
    [SerializeField] private GooseStateType stateType;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IGooseState newState = GetStateFromEnum(stateType);
            OnGooseTrigger?.Invoke(newState);
        }
        else if (other.CompareTag("Racer"))
        {
            OnAIRacerTrigger?.Invoke(other.gameObject, stateType);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (stateType == GooseStateType.Swimming)
            {
                IGooseState newState = GetStateFromEnum(GooseStateType.Running);
                OnGooseTrigger?.Invoke(newState);
            }
        }
        else if (other.CompareTag("Racer"))
        {
            if (stateType == GooseStateType.Swimming)
            {
                OnAIRacerTrigger?.Invoke(other.gameObject, GooseStateType.Running);
            }
        }
        
    }

    private IGooseState GetStateFromEnum(GooseStateType type)
    {
        return type switch
        {
            GooseStateType.Running => new RunningState(),
            GooseStateType.Swimming => new SwimmingState(),
            GooseStateType.Flying => new FlyingState(),
            _ => null
        };
    }

    
}

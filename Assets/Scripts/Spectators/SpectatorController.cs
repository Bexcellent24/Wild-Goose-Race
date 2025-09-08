using UnityEngine;

public class Spectator : MonoBehaviour
{
    public Animator Animator;
    private SpectatorState currentState;
    private float stateTimer;
    
    void Start()
    {
        //Starts off all spectators in the idle anim but offsets.
        Animator.Play("Wobble", 0, Random.value); 
        SwitchState(new IdleState());
        
        SFXManager.Instance.PlaySoundOnObject("Chirping", this.gameObject, 1.5f);
    }

    void Update()
    {
        if (currentState != null)
        {
            //Runs the Update abstract method in SpectatorState if currentState is not null.
            currentState.Update(this);
        }    
    }

    public void SwitchState(SpectatorState newState)
    {
        //Exits the current State.
        if (currentState != null)
        {
            currentState.Exit(this);
        }
        
        //Updates the currentState to the new state (Idle, Jump, Cheer).
        currentState = newState;
        
        //Runs the enter logic.
        currentState.Enter(this);
    }

    public void SetRandomStateDuration()
    {
        //Randomises the stateTimer between 1 and 3 second for de-synchronisation amongst other spectators.
        stateTimer = Random.Range(1f, 3f);
    }

    public bool IsStateTimeOver()
    {
        //Decreases the stateTimer every frame.
        stateTimer -= Time.deltaTime;
        //Returns true when the timer hits 0.
        return stateTimer <= 0f;
    }

    public void SwitchToRandomState()
    {
        //Picks a random int between 0 and 2.
        int random = Random.Range(0, 3);
        //The number picked will determine which state the spectator will transition to.
        switch (random)
        {
            case 0: SwitchState(new IdleState()); break;
            case 1: SwitchState(new CheerState()); break;
            case 2: SwitchState(new JumpState()); break;
        }
    }
}




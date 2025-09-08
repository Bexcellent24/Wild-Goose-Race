using UnityEngine;

public class IdleState : SpectatorState
{
    //This class inherits the abstract class SpectatorState, meaning it must contain the state 3 methods
    //Each variation uses an override to implement their own implementations respectively 
    public override void Enter(Spectator spectator)
    {
        //Case 0 = Idle.
        //Setting the animation controller CurrentState int variable to 0 so that it plays the Idle case animation.
        spectator.Animator.SetInteger("CurrentState", 0); 
        //Runs the SetRandomStateDuration method to displace how long each the spectator will remain in this state.
        spectator.SetRandomStateDuration();
    }

    public override void Update(Spectator spectator)
    {
        //If statement checks to see if the duration of the stateTimer is over, if true then run the switch state method 
        if (spectator.IsStateTimeOver())
            spectator.SwitchToRandomState();
    }

    public override void Exit(Spectator spectator)
    {
        //Exit method has no current cleanup logic, but thought it good practice to leave it here.
    }
}


using UnityEngine;

public class JumpState : SpectatorState
{
    public override void Enter(Spectator spectator)
    {
        // 2 = Jump
        spectator.Animator.SetInteger("CurrentState", 2); 
        spectator.SetRandomStateDuration();
    }

    public override void Update(Spectator spectator)
    {
        if (spectator.IsStateTimeOver())
            spectator.SwitchToRandomState();
    }

    public override void Exit(Spectator spectator)
    {
        
    }
}


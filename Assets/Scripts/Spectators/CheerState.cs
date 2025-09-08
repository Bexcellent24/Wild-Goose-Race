using UnityEngine;

public class CheerState : SpectatorState
{
    public override void Enter(Spectator spectator)
    {
        // 1 = Cheer
        spectator.Animator.SetInteger("CurrentState", 1); 
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


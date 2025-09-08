using UnityEngine;

public class RacerTurner : AIRacer
{
    public override void Initialise()
    {
        //Assigns Custom behaviours for this AIRacer subclass
        TopSpeed = 30f;
        Acceleration = 10f;
        TurnRate = 20f;
        CorneringSlowdownMultiplier = 0.2f;
    }
}

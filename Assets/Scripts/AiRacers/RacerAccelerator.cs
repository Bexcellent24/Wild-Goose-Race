using UnityEngine;

public class RacerAccelerator : AIRacer
{
    public override void Initialise()
    {
        //Assigns Custom behaviours for this AIRacer subclass
        TopSpeed = 30f;
        Acceleration = 12f;
        TurnRate = 100f;
        CorneringSlowdownMultiplier = 0.5f;
    }
}

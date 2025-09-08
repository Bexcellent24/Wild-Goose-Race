using UnityEngine;

public class RacerTopSpeeder : AIRacer
{
   public override void Initialise()
   {
      //Assigns Custom behaviours for this AIRacer subclass
      TopSpeed = 34f;
      Acceleration = 8f;
      TurnRate = 100f;
      CorneringSlowdownMultiplier = 0.5f;
   }
}

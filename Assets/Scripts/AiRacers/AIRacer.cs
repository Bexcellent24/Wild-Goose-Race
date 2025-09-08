using UnityEngine;

public abstract class AIRacer : MonoBehaviour
{
   //Each Racer subclass will have to implement there own variations of these variables
   //This allows us to allocate different attributes to each racer subclass
   public float TopSpeed { get; protected set; }
   public float Acceleration { get; protected set; }
   public float TurnRate { get; protected set; }
   public float CorneringSlowdownMultiplier { get; protected set; }

   //Each subclass will implement this method and will set their specific attributes accordingly
   public abstract void Initialise();

   
}

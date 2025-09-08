using UnityEngine;

public abstract class SpectatorState
{
   //Three methods must be implemented for any class that inherits this class
   
   //Three states are needed to implement the spectators states
   public abstract void Enter(Spectator spectator);
   public abstract void Update(Spectator spectator);
   public abstract void Exit(Spectator spectator);
}




using UnityEngine;

public interface IGooseState
{
    void EnterState(GooseController goose);
    void HandleInput(GooseController goose);
    void UpdatePhysics(GooseController goose);
    void ExitState(GooseController goose);
    
}


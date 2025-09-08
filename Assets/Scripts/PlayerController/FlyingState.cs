using UnityEngine;

public class FlyingState : IGooseState
{
    
    private float verticalInput;
    
    public void EnterState(GooseController goose)
    {
        Debug.Log("Entered Flying State");
        
        goose.rb.angularVelocity = Vector3.zero;
        goose.rb.useGravity = false; 
        
        goose.animator.SetBool("flying", true);
        
        SFXManager.Instance?.PlayLoopedSoundOnObject("Flying", goose.gameObject, 0.9f); // Or any desired volume
    }

    public void HandleInput(GooseController goose)
    {
        goose.turnInput = Input.GetAxis("Horizontal"); 
        goose.forwardInput = Mathf.Max(0, Input.GetAxis("Vertical")); 
        verticalInput = (Input.GetKey(KeyCode.Space) ? 1 : 0) + (Input.GetKey(KeyCode.LeftShift) ? -1 : 0);
    }

    public void UpdatePhysics(GooseController goose)
    {
        ApplyFlightMovement(goose);
        EnforceHeightLimit(goose);
        KeepUpright(goose);
    }
    
    public void ExitState(GooseController goose)
    {
        Debug.Log("Exiting Flying State");
        goose.currentHeading = goose.transform.eulerAngles.y;
        goose.rb.useGravity = true;
        goose.animator.SetBool("flying", false);
        
        SFXManager.Instance?.StopSoundOnObject(goose.gameObject);
    }

    private void ApplyFlightMovement(GooseController goose)
    {
        Vector3 velocity = goose.rb.linearVelocity;
        Vector3 forwardVelocity = Vector3.Project(velocity, goose.transform.forward);
        goose.rb.linearVelocity = forwardVelocity;
        
        if (goose.forwardInput > 0f)
        {
            if (forwardVelocity.magnitude < goose.gooseStats.maxFlightSpeed)
            {
                goose.rb.AddForce(goose.transform.forward * goose.gooseStats.flyingAcceleration, ForceMode.Acceleration);
            }
        }
        else if (goose.forwardInput < 0f) 
        {
            goose.rb.AddForce(-goose.transform.forward * goose.gooseStats.flyingBrakeForce, ForceMode.Acceleration);
        }
        else
        {
            // Apply natural slow down
            goose.rb.linearVelocity *= 1f - (goose.gooseStats.flyingCoastDrag * Time.deltaTime);
        }

        // VERTICAL MOVEMENT: Simple up/down
        if (verticalInput != 0)
        {
            goose.rb.AddForce(Vector3.up * (verticalInput * goose.gooseStats.verticalFlightSpeed), ForceMode.Acceleration);
        }
    }




    private void EnforceHeightLimit(GooseController goose)
    {
        float dynamicMinHeight = GetGroundHeight(goose) + 2f; // 2m above the ground

        // Prevent flying too high
        if (goose.transform.position.y > goose.gooseStats.maxFlightHeight)
        {
            Vector3 position = goose.transform.position;
            position.y = goose.gooseStats.maxFlightHeight;
            goose.transform.position = position;
            goose.rb.linearVelocity = new Vector3(goose.rb.linearVelocity.x, 0, goose.rb.linearVelocity.z);
        }
        // Prevent flying lower than dynamic min height
        else if (goose.transform.position.y < dynamicMinHeight)
        {
            Vector3 position = goose.transform.position;
            position.y = dynamicMinHeight;
            goose.transform.position = position;
            goose.rb.linearVelocity = new Vector3(goose.rb.linearVelocity.x, 0, goose.rb.linearVelocity.z);
        }
    }
    
    private float GetGroundHeight(GooseController goose)
    {
        RaycastHit hit;
        if (Physics.Raycast(goose.transform.position, Vector3.down, out hit, Mathf.Infinity))
        {
            return hit.point.y; // Return ground height
        }
        return 0f;
    }


    private void KeepUpright(GooseController goose)
    {
        // Base upright rotation to stop the goose from going belly up
        Quaternion uprightRotation = Quaternion.Euler(0, goose.transform.eulerAngles.y, 0);
    
        // ROLL
        float rollAngle = goose.turnInput * goose.gooseStats.turnTiltAmount; 
        Quaternion rollRotation = Quaternion.Euler(0, 0, -rollAngle);

        //PITCH
        float pitchAngle = verticalInput * goose.gooseStats.verticalTiltAmount; 
        Quaternion pitchRotation = Quaternion.Euler(-pitchAngle, 0, 0);

        // Combine upright, roll, and pitch
        Quaternion targetRotation = uprightRotation * rollRotation * pitchRotation;

        // Smooth rotation change
        goose.transform.rotation = Quaternion.Lerp(goose.transform.rotation, targetRotation, goose.gooseStats.tiltSpeed * Time.deltaTime);

        // Apply turning movement
        goose.transform.Rotate(Vector3.up * (goose.turnInput * goose.gooseStats.flyingTurnSpeed * Time.deltaTime));
    }

}

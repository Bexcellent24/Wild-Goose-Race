using System.Collections;
using UnityEngine;
public class RunningState : IGooseState
{
    private bool isRunningSoundPlaying = false;
    private int movementState = 0; 
    
    public void EnterState(GooseController goose)
    {
        Debug.Log("Entered Running State");

        goose.animator.SetBool("running", true);
    }
    
    public void HandleInput(GooseController goose)
    {
        goose.turnInput = Input.GetAxis("Horizontal");
        goose.forwardInput = Input.GetAxis("Vertical");

        // Update the rotation
        goose.currentHeading += goose.turnInput * goose.gooseStats.turnSpeed * Time.deltaTime;
        goose.gooseStats.uprightJointTargetRot = Quaternion.Euler(0f, goose.currentHeading, 0f);

        
        float speed = goose.rb.linearVelocity.magnitude;
        
        if(speed <= 0.3f)
            movementState = 0;
        else if (speed > 0.3f && speed <= 15f)
            movementState = 1; // Walk
        else if (speed > 15f && speed <= 30f)
            movementState = 2; // Jog
        else if (speed > 30f)
            movementState = 3; // Run
        
        if (!goose.animator.GetBool("swimming") && !goose.animator.GetBool("flying"))
        {
            goose.animator.SetInteger("movementState", movementState);
        }

        if (speed >= 0.3f)
        {
            if (!isRunningSoundPlaying)
            {
                SFXManager.Instance.PlayLoopedSoundOnObject("Running", goose.gameObject, 1f);
                isRunningSoundPlaying = true;
            }
        }
        else
        {
            if (isRunningSoundPlaying)
            {
                SFXManager.Instance.StopSoundOnObject(goose.gameObject);
                isRunningSoundPlaying = false;
            }
        }
        
    }

    public void UpdatePhysics(GooseController goose)
    {
        ApplyHoverForce(goose);
        UpdateUprightForce(goose);
        ApplyForwardMovement(goose);
    }

    public void ExitState(GooseController goose)
    {
        Debug.Log("Exiting Running State");
        goose.animator.SetBool("running", false);
        
    }

    private void ApplyHoverForce(GooseController goose)
    {
        Vector3 downDir = Vector3.down;
        RaycastHit hit;
        
        if (Physics.Raycast(goose.transform.position, downDir, out hit, goose.gooseStats.rideHeight * 2))
        {
            Vector3 vel = goose.rb.linearVelocity;
            Vector3 rayDir = downDir;

            Vector3 otherVel = hit.rigidbody != null ? hit.rigidbody.linearVelocity : Vector3.zero;
            float relVel = Vector3.Dot(rayDir, vel - otherVel);

            float displacement = hit.distance - goose.gooseStats.rideHeight;
            float springForce = (displacement * goose.gooseStats.rideSpringStrength) - (relVel * goose.gooseStats.rideSpringDamper);

            goose.rb.AddForce(rayDir * springForce, ForceMode.Acceleration);
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForceAtPosition(rayDir * -springForce, hit.point, ForceMode.Acceleration);
            }
            
            goose.gooseStats.fallSpeed = 0;
        }
        else
        {
           // goose.gooseStats.fallSpeed += goose.gooseStats.fallAcceleration * Time.deltaTime;
            goose.rb.AddForce(Vector3.down * 1000, ForceMode.Acceleration);
        }
    }
    
    private void UpdateUprightForce(GooseController goose)
    {
        Quaternion toGoal = goose.gooseStats.uprightJointTargetRot * Quaternion.Inverse(goose.transform.rotation);
        toGoal.ToAngleAxis(out float rotDegrees, out Vector3 rotAxis);
        
        if (rotAxis != Vector3.zero)
        {
            rotAxis.Normalize();
            float rotRadians = rotDegrees * Mathf.Deg2Rad;
            Vector3 torque = (rotAxis * (rotRadians * goose.gooseStats.uprightJointSpringStrength)) - (goose.rb.angularVelocity * goose.gooseStats.uprightJointSpringDampener);
            goose.rb.AddTorque(torque, ForceMode.Acceleration);
        }
    }
    
    private void ApplyForwardMovement(GooseController goose)
    {
        Vector3 currentVelocity = goose.rb.linearVelocity;
        float currentForwardSpeed = Vector3.Dot(currentVelocity, goose.transform.forward);
        
        if (goose.forwardInput > 0f)
        {
            if (currentForwardSpeed < goose.gooseStats.maxForwardSpeed)
            {
                goose.rb.AddForce(goose.transform.forward * goose.gooseStats.acceleration, ForceMode.Acceleration);
            }
        }
        else if (goose.forwardInput < 0f)
        {
            if (currentForwardSpeed > 0.1f)
            {
                goose.rb.AddForce(-goose.transform.forward * goose.gooseStats.brakeForce, ForceMode.Acceleration);
            }
            else if (currentForwardSpeed > -goose.gooseStats.maxReverseSpeed)
            {
                goose.rb.AddForce(goose.transform.forward * (goose.gooseStats.acceleration * goose.forwardInput), ForceMode.Acceleration);
            }
        }
        else
        {
            // Prevent drag from affecting the goose if it's nearly stationary
            if (currentVelocity.magnitude > 0.1f) 
            {
                Vector3 dragForce = -currentVelocity * goose.gooseStats.coastDrag;
                goose.rb.AddForce(dragForce, ForceMode.Acceleration);
            }
        }

        // Drift Reduction
        Vector3 forwardVelocity = Vector3.Project(goose.rb.linearVelocity, goose.transform.forward); // Keep only forward motion
        Vector3 sidewaysVelocity = goose.rb.linearVelocity - forwardVelocity; 
        goose.rb.linearVelocity = forwardVelocity + (sidewaysVelocity * 0.2f);
    }
    
}

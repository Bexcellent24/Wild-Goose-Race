    using UnityEngine;

    public class SwimmingState : IGooseState
    {
        
        private float stateTimer;
        public void EnterState(GooseController goose)
        {
            Debug.Log("Entered Swimming State");
            stateTimer = 0f;
            
            goose.rb.angularVelocity = Vector3.zero;
            
            goose.rb.linearDamping = 2f;
            goose.animator.SetBool("swimming", true);
            
            SFXManager.Instance.PlayLoopedSoundOnObject("Swimming", goose.gameObject, .9f); // Or any desired volume
        }

        public void HandleInput(GooseController goose)
        {
            goose.turnInput = Input.GetAxis("Horizontal");
            goose.forwardInput = Input.GetAxis("Vertical");
            
            goose.currentHeading += goose.turnInput * goose.gooseStats.turnSpeed * Time.deltaTime;
            goose.gooseStats.uprightJointTargetRot = Quaternion.Euler(0f, goose.currentHeading, 0f);
        }

        public void UpdatePhysics(GooseController goose)
        {
            ApplyBuoyancy(goose);
            ApplyWaterResistance(goose);
            ApplySwimmingMovement(goose);
            stateTimer += Time.fixedDeltaTime;
            if (stateTimer > 0.2f)
            {
                UpdateUprightForce(goose);
            }
        }

        public void ExitState(GooseController goose)
        {
            Debug.Log("Exiting Swimming State");
            
            //goose.rb.linearVelocity = Vector3.zero;
            goose.rb.angularVelocity = Vector3.zero;
            
            goose.rb.linearDamping = 0f;
            goose.animator.SetBool("swimming", false);
            
            SFXManager.Instance.StopSoundOnObject(goose.gameObject);
        }

        private void ApplyBuoyancy(GooseController goose)
        {
            float depth = goose.gooseStats.waterLevel - goose.transform.position.y;
            if (depth > 0)
            {
                float buoyancyForce = goose.gooseStats.buoyancyStrength * depth;
                goose.rb.AddForce(Vector3.up * buoyancyForce, ForceMode.Acceleration);
            }
            
            
        }

        private void ApplyWaterResistance(GooseController goose)
        {
            goose.rb.linearVelocity *= 1f - (goose.gooseStats.waterDrag * Time.fixedDeltaTime);
            goose.rb.angularVelocity *= 1f - (goose.gooseStats.turnDrag * Time.fixedDeltaTime);
            
        }

        private void ApplySwimmingMovement(GooseController goose)
        {
            if (goose.forwardInput != 0)
            {
                goose.rb.AddForce(goose.transform.forward * (goose.forwardInput * goose.gooseStats.swimAcceleration), ForceMode.Acceleration);
            }
        }

        private void UpdateUprightForce(GooseController goose)
        {
            Quaternion toGoal = goose.gooseStats.uprightJointTargetRot * Quaternion.Inverse(goose.transform.rotation);
            toGoal.ToAngleAxis(out float rotDegrees, out Vector3 rotAxis);
            rotAxis.Normalize();
            float rotRadians = rotDegrees * Mathf.Deg2Rad;

            Vector3 torque = (rotAxis * (rotRadians * goose.gooseStats.uprightJointSpringStrength)) -
                             (goose.rb.angularVelocity * goose.gooseStats.uprightJointSpringDampener);

            goose.rb.AddTorque(torque, ForceMode.Acceleration);
        }
    }
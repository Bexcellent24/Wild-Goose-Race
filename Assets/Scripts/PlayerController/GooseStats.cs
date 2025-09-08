using UnityEngine;

[CreateAssetMenu(fileName = "GooseStats", menuName = "Goose/GooseStats", order = 1)]
public class GooseStats : ScriptableObject
{
    [Header("Running State Variables")]
    [Header("Acceleration/Breaking Settings")]
    public float maxForwardSpeed = 50f;
    public float maxReverseSpeed = 10f;
    public float acceleration = 5f;
    public float brakeForce = 20f;
    public float coastDrag = 2f;
    
    [Header("Turning Settings")]
    public float turnSpeed = 90f;
    //public float currentHeading = 0f;

    [Header("Hover Settings")]
    public float rideHeight = 2.4f;
    public float rideSpringStrength = 800f;
    public float rideSpringDamper = 20f;
    public float fallSpeed = 100f;  
    public float fallAcceleration = 200f;

    [Header("Upright Settings")]
    public Quaternion uprightJointTargetRot;
    public float uprightJointSpringStrength = 25f;
    public float uprightJointSpringDampener = 4f;

    [Header("Swimming State Variables")]
    public float waterLevel = 4.2f;
    public float buoyancyStrength = 800f;
    public float waterDrag = 0.5f;
    public float turnDrag = 0.5f;
    public float swimAcceleration = 80f;

    [Header("Flying State Variables")]
    public float maxFlightSpeed = 25f;
    public float verticalFlightSpeed = 15f;
    public float flyingAcceleration = 8f;
    public float flyingBrakeForce = 5f;
    public float flyingCoastDrag = 1.5f;
    public float flyingTurnSpeed = 60f;
    public float maxFlightHeight = 40f;
    public float turnTiltAmount = 20f;
    public float verticalTiltAmount = 20f;
    public float tiltSpeed = 5f;
}


using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GooseController : MonoBehaviour
{
    public Rigidbody rb;
    public Animator animator;
    
    private IGooseState currentState;
    
    [Header("Goose Stats")]
    public GooseStats gooseStats; // This one stays public and accessible
    [SerializeField] private GooseStats gooseStatsTemplate; // Only used internally
    public float forwardInput;
    public float turnInput;
    public float currentHeading;

    
    [Header("Material Variables")]
    public GameObject playerPrefab;
    public Material[] gooseMaterials; 
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        gooseStats = Instantiate(gooseStatsTemplate);
        currentHeading = transform.eulerAngles.y;
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "CheckpointRace")
        {
            gooseStats.maxFlightHeight = 55f;
            gooseStats.waterLevel = 20.5f;
            gooseStats.maxForwardSpeed = 30;
            gooseStats.maxFlightSpeed = 25;
            
        }
        else if (sceneName == "BeginnerRace")
        {
            gooseStats.maxFlightHeight = 85f;
            gooseStats.waterLevel = 53.8f;
            gooseStats.maxForwardSpeed = 30;
            gooseStats.maxFlightSpeed = 20;
        }
        else if (sceneName == "AdvancedRace")
        {
            gooseStats.maxFlightHeight = 81f;
            gooseStats.waterLevel = 47f;
            gooseStats.maxForwardSpeed = 31;
            gooseStats.maxFlightSpeed = 23;
        }
        
        // Get the selected goose index
        int selectedGooseIndex = PlayerPrefs.GetInt("SelectedGooseIndex", 0);

        // Apply the correct material
        if (playerPrefab != null && gooseMaterials.Length > selectedGooseIndex)
        {
            Renderer playerRenderer = playerPrefab.GetComponent<Renderer>();
            if (playerRenderer != null)
            {
                playerRenderer.material = gooseMaterials[selectedGooseIndex];
            }
            else
            {
                Debug.LogError("Player prefab is missing a Renderer component!");
            }
        }
        else
        {
            Debug.LogError("Invalid goose index or missing materials!");
        }
        
        ChangeState(new RunningState());
        
    }

    private void Update()
    {
        currentState?.HandleInput(this);
    }

    private void FixedUpdate()
    {
        currentState?.UpdatePhysics(this);
    }

    private void ChangeState(IGooseState newState)
    {
        if (currentState != null)
        {
            currentState.ExitState(this);
        }
        
        currentState = newState;
        currentState.EnterState(this);
    }
    
    private void OnEnable()
    {
        GooseStateTrigger.OnGooseTrigger += ChangeState; 
    }

    private void OnDisable()
    {
        GooseStateTrigger.OnGooseTrigger -= ChangeState; 
    }
    
    
}
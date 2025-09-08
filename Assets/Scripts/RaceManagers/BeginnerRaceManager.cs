using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BeginnerRaceManager : MonoBehaviour
{
    public delegate void LappedAction(int lap);
    public static event LappedAction OnLapped;
    
    public delegate void GamePausedAction();
    public static event GamePausedAction OnGamePaused;
    
    public delegate void PositionUpdatedAction(int position);
    public static event PositionUpdatedAction OnPositionUpdated;
    
    public delegate void WaypointSetupAction(CustomLinkedList<Transform> waypoints);
    public static event WaypointSetupAction OnWaypointSetup;
    
    public delegate void RaceEndAction();
    public static event RaceEndAction OnRaceEnd;
    
    
    [SerializeField] private ConcreteAIRacerFactory factory;
    [SerializeField] private AISpawner spawner;
    [SerializeField] private Transform[] waypointTransforms;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private PositionTracker playerTracker;
    
    
    private List<PositionTracker> racers = new List<PositionTracker>(); //list of PositionTracker to track positions
    private CustomLinkedList<Transform> waypointsList; //custom list for waypoints
    private int LapCount = 1;
    
    public static BeginnerRaceManager Instance { get; private set; }

    private void Awake()
    {
        //singleton stuff
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    
    private void Start()
    {
        //put all the waypoints in the list
        SetupWaypoints();
        StartRace();
        SFXManager.Instance?.PlayBackgroundMusic("RaceBackground");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnGamePaused?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            SFXManager.Instance?.PlayGlobalSound("Quack");
        }
    }
    
    private void FixedUpdate()
    {
        //sorting the list of players based first off how many waypoints they have passed and how close they are to their check points. works 90% of the time
        racers = racers
            .Where(r => r != null && r.CurrentWaypoint != null)
            .OrderByDescending(r => CalculateRaceProgress(r))
            .ToList();

        int playerPosition = racers.IndexOf(playerTracker) + 1;
        OnPositionUpdated?.Invoke(playerPosition);

      //  DisplayRacerOrderDebug();

    }
    
    private void SetupWaypoints()
    {
        //loads em up in order that they are put in the editor. 
        waypointsList = new CustomLinkedList<Transform>();

        foreach (Transform wp in waypointTransforms)
        {
            waypointsList.Insert(wp);
            Debug.Log(wp.name);
        }
        
        //lets poeple know that we are goose to go with the waypoints
        OnWaypointSetup?.Invoke(waypointsList);
    }
    public void RegisterRacer(PositionTracker tracker)
    {
        //adds things with trackers to the list to track position
        if (tracker != null && !racers.Contains(tracker))
        {
            racers.Add(tracker);
            Debug.Log("Registered racer: " + tracker.gameObject.name);
        }
        else
        {
            Debug.Log("Failed to add " + tracker.gameObject.name);
        }
    }
    private void StartRace()
    {
        //tells the spawners to get spawning, giving it the factory it should use, the spawn points, and the waypoint list to give to the racers
        spawner.SpawnRacers(factory, spawnPoints, waypointsList);
    }
    private float CalculateRaceProgress(PositionTracker racer)
    {
        float baseProgress = racer.TotalWaypointsPassed;
        float proximity = WaypointProximityBonus(racer);

        return baseProgress + proximity;
    }
    private float WaypointProximityBonus(PositionTracker racer)
    {
        if (racer.CurrentWaypoint == null)
            return 0f;

        float distance = Vector3.Distance(racer.transform.position, racer.CurrentWaypoint.position);
        return Mathf.Clamp(1f - (distance / 100f), 0f, 1f);
    }
    
    private void DisplayRacerOrderDebug()
    {
        //helping me figure out what the heck is going on
        
        System.Text.StringBuilder debugText = new System.Text.StringBuilder();
        debugText.AppendLine("=== Racer Order ===");

        for (int i = 0; i < racers.Count; i++)
        {
            var racer = racers[i];
            debugText.AppendLine(
                $"#{i + 1}: {racer.gameObject.name} | Waypoints: {racer.TotalWaypointsPassed} | " +
                $"DistanceToNext: {Vector3.Distance(racer.transform.position, racer.CurrentWaypoint.position):F2}"
            );
        }

        debugText.AppendLine("====================");
        Debug.Log(debugText.ToString());
    }
    private void CheckLap()
    {
        LapCount++;

        if (LapCount >= 4)
        {
            EndRace();
        }
        else
        {
            // tells the ui that the player has lapped so keep up
            OnLapped?.Invoke(LapCount);
        }
    }
    private void EndRace()
    {
        //tells everyone the race is over. go home.
        OnRaceEnd?.Invoke();
    }
    private void OnEnable()
    {
        LapCounter.OnLapTriggered += CheckLap;
    }
    private void OnDisable()
    {
        LapCounter.OnLapTriggered -= CheckLap;
    }
}
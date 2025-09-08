using System;
using TMPro;
using UnityEngine;

public class CheckpointRaceManager : MonoBehaviour
{
    
    public delegate void GameOverAction(bool raceWon);
    public static event GameOverAction OnGameOver;
    
    public delegate void GamePausedAction();
    public static event GamePausedAction OnGamePaused;
    
    [SerializeField] private GameObject[] checkpoints; 
    [SerializeField] private TextMeshProUGUI timerText; 
    [SerializeField] private float startTime = 30f;
    [SerializeField] private Material matActive;
    [SerializeField] private Renderer portalRenderer;
    
    private float raceTime;
    private CustomStack<GameObject> checkpointStack = new CustomStack<GameObject>();
    private bool raceActive = false;

    private void Start()
    {
        raceTime = startTime;
        InitializeCheckpoints();
        StartRace();
        SFXManager.Instance?.PlayBackgroundMusic("RaceBackground");
    }

    private void Update()
    {
        if (raceActive)
        {
            raceTime -= Time.deltaTime;
            TimeSpan timeSpan = TimeSpan.FromSeconds(raceTime);
            timerText.text = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);

            if (raceTime <= 0)
            {
                raceTime = 0;
                EndRace(false);
            }
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                SFXManager.Instance?.PlayGlobalSound("Quack");
            }
        }
        
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnGamePaused?.Invoke();
        }
        
    }

    private void InitializeCheckpoints()
    {
        checkpointStack.Clear();

        // Push checkpoints in reverse order so the first one is at the top
        for (int i = checkpoints.Length - 1; i >= 0; i--)
        {
            checkpointStack.Push(checkpoints[i]);
        }
        
        // Set the first active checkpoint
        if (!checkpointStack.IsEmpty())
        {
            SetCheckpointMaterial(checkpointStack.Peek(), matActive);
        }

        Debug.Log("Checkpoints loaded in order.");
    }

    private void StartRace()
    {
        raceActive = true;
    }
    

    private GameObject GetNextCheckpoint()
    {
        return checkpointStack.Peek();
    }

    private void HandleCheckpointPassed(float timeBonus, GameObject checkpoint)
    {
        if (!raceActive) return;

        if (checkpoint == GetNextCheckpoint())
        {
            raceTime += timeBonus;
            checkpointStack.Pop();
            
            // Set the next checkpoint to active material
            if (!checkpointStack.IsEmpty())
            {
                SetCheckpointMaterial(checkpointStack.Peek(), matActive);
            }

            if (checkpointStack.IsEmpty())
            {
                EndRace(true);
            }
            
            Debug.Log("Checkpoint passed! +" + timeBonus + " seconds");
            if (checkpoint.transform.childCount > 0)
            {
                Transform firstChild = checkpoint.transform.GetChild(0);
                Destroy(firstChild.gameObject);
            }
            else
            {
                Debug.LogWarning("Checkpoint has no children to destroy: " + checkpoint.name);
            }
        }
        
        
       
    }

    private void EndRace(bool raceWon)
    {
        raceActive = false;
        OnGameOver?.Invoke(raceWon);
        Debug.Log("Race Over!");
    }
    
    private void SetCheckpointMaterial(GameObject checkpoint, Material material)
    {
        if (checkpoint.transform.childCount > 0)
        {
            Transform firstChild = checkpoint.transform.GetChild(0);
            Renderer renderer = firstChild.GetComponent<Renderer>();

            if (renderer != null)
            {
                renderer.material = material;
            }
            else
            {
                Debug.LogWarning("First child has no renderer: " + firstChild.name);
            }
        }
        else
        {
            Debug.LogWarning("Checkpoint has no children: " + checkpoint.name);
        }
    }

    private void OnEnable()
    {
        Checkpoint.OnCheckpointPassed += HandleCheckpointPassed; 
    }

    private void OnDisable()
    {
        Checkpoint.OnCheckpointPassed -= HandleCheckpointPassed; 
    }
}


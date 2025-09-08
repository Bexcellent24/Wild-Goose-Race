using System.Collections;
using TMPro;
using UnityEngine;

public class BRUIManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private TextMeshProUGUI gameOverMessage;
    [SerializeField] private TextMeshProUGUI lapCounterText;
    [SerializeField] private TextMeshProUGUI positionText;
    [SerializeField] private GameObject pausedMenuScreen;
    
    
    void Start()
    {
        Time.timeScale = 1;
        gameOverScreen.SetActive(false);
        
    }
    private void OnEnable()
    {
        BeginnerRaceManager.OnLapped += UpdateLap;
        BeginnerRaceManager.OnRaceEnd += GameOver;
        BeginnerRaceManager.OnPositionUpdated += UpdatePosition;
        BeginnerRaceManager.OnGamePaused += PauseRace;
        
        AdvancedRaceManager.OnLapped += UpdateLap;
        AdvancedRaceManager.OnRaceEnd += GameOver;
        AdvancedRaceManager.OnPositionUpdated += UpdatePosition;
        AdvancedRaceManager.OnGamePaused += PauseRace;
        
        SceneNavigation.OnGameResumed += ResumeRace;
    }
    private void OnDisable()
    {
        BeginnerRaceManager.OnLapped -= UpdateLap;
        BeginnerRaceManager.OnRaceEnd -= GameOver;
        BeginnerRaceManager.OnPositionUpdated -= UpdatePosition;
        BeginnerRaceManager.OnGamePaused -= PauseRace;
        
        AdvancedRaceManager.OnLapped -= UpdateLap;
        AdvancedRaceManager.OnRaceEnd -= GameOver;
        AdvancedRaceManager.OnPositionUpdated -= UpdatePosition;
        AdvancedRaceManager.OnGamePaused -= PauseRace;
        
        SceneNavigation.OnGameResumed -= ResumeRace;
    }

    private void UpdateLap(int lap)
    {
        lapCounterText.text = "LAP: " + lap + "/3";
    }
    private void PauseRace()
    {
        Time.timeScale = 0;
        pausedMenuScreen.SetActive(true);
        SFXManager.Instance?.PauseAllSFX();
    }

    private void ResumeRace()
    {
        Time.timeScale = 1;
        pausedMenuScreen.SetActive(false);
        SFXManager.Instance?.ResumeAllSFX();
    }
    
    private void UpdatePosition(int position)
    {
        string suffix = GetPositionSuffix(position);
        positionText.text = position + suffix;
    }
    
    private string GetPositionSuffix(int position)
    {
        if (position == 1) return "st";
        if (position == 2) return "nd";
        if (position == 3) return "rd";
        return "th";
    }
    
    private void GameOver()
    {
        gameOverScreen.SetActive(true);
        StartCoroutine(StopTimeDelay());
        gameOverMessage.text = "YOU FINISHED " + positionText.text;
        SFXManager.Instance?.StopAllSFX();
    }
    private IEnumerator StopTimeDelay()
    {
        yield return new WaitForSeconds(2f);
        Time.timeScale = 0;
        
    }
}

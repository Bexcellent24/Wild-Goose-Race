using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class CPRUIManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private TextMeshProUGUI gameOverMessage;
    [SerializeField] private GameObject pausedMenuScreen;
    void Start()
    {
        Time.timeScale = 1;
        gameOverScreen.SetActive(false);
        
    }

    private void OnEnable()
    {
        CheckpointRaceManager.OnGameOver += GameOver;
        CheckpointRaceManager.OnGamePaused += PauseRace;
        
        SceneNavigation.OnGameResumed += ResumeRace;
    }
    private void OnDisable()
    {
        CheckpointRaceManager.OnGameOver -= GameOver;
        CheckpointRaceManager.OnGamePaused -= PauseRace;
        
        SceneNavigation.OnGameResumed -= ResumeRace;
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
    private void GameOver(bool raceWon)
    {
        gameOverScreen.SetActive(true);
        StartCoroutine(StopTimeDelay());
        SFXManager.Instance?.StopAllSFX();

        if (raceWon)
        {
            gameOverMessage.text = "Congratulations! \n You Did It!";
        }
        else
        {
            gameOverMessage.text = "Game Over!";
        }
    }

    private IEnumerator StopTimeDelay()
    {
        yield return new WaitForSeconds(2f);
        Time.timeScale = 0;
        
    }
}

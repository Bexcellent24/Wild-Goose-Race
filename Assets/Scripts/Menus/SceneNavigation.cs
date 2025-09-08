using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneNavigation : MonoBehaviour
{
    
    public delegate void GameResumedAction();
    public static event GameResumedAction OnGameResumed;
    
    [SerializeField] private string sceneName; 
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Image LoadingBarFull;

    private void Start()
    {
        loadingScreen.SetActive(false);
    }
    
    public void LoadScene()
    {
        if (IsValidScene(sceneName))
        {
            Time.timeScale = 1;
            StartCoroutine(LoadingScene());
        }
        else
        {
            Debug.LogWarning($"Scene '{sceneName}' does not exist or is not added to the build settings.");
        }
    }

    public void ResumeRace()
    {
        OnGameResumed?.Invoke();
    }
    
    IEnumerator LoadingScene()
    {
        loadingScreen.SetActive(true);
        float loadTime = 1.5f;
        float timer = 0f;

        // Simulate loading bar filling up over `loadTime` seconds
        while (timer < loadTime)
        {
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(timer / loadTime);
            LoadingBarFull.fillAmount = progress;
            yield return null;
        }

        // Optional: small delay at 100% to feel smooth
        yield return new WaitForSeconds(0.3f);

        SceneManager.LoadScene(sceneName);
    }
    
    
    
    

    public void ExitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; 
#endif
    }

    private bool IsValidScene(string scene)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneNameInBuild = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            
            if (sceneNameInBuild == scene)
            {
                return true;
            }
        }
        return false;
    }
}
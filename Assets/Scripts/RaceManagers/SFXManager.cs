using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }

    private HashMap<string, AudioClip> sfxMap = new HashMap<string, AudioClip>();

    [SerializeField] private string[] soundNames;

    [Range(0f, 1f)]
    [SerializeField] private float globalVolume = 0.5f;
    
    private List<AudioSource> activeAudioSources = new List<AudioSource>();
    private AudioSource backgroundAudioSource;
    private AudioSource globalAudioSource;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Load sounds once
        foreach (string soundName in soundNames)
        {
            AddSound(soundName, Resources.Load<AudioClip>("Sounds/" + soundName));
        }
    }

     public void PlayBackgroundMusic(string key)
    {
        if (sfxMap.ContainsKey(key))
        {
            AudioClip clip = sfxMap.Get(key);

            if (backgroundAudioSource == null)
            {
                backgroundAudioSource = gameObject.AddComponent<AudioSource>();
            }

            if (backgroundAudioSource.isPlaying)
            {
                backgroundAudioSource.Stop();
            }

            backgroundAudioSource.clip = clip;
            backgroundAudioSource.loop = true;
            backgroundAudioSource.volume = 0.1f;
            backgroundAudioSource.Play();

            if (!activeAudioSources.Contains(backgroundAudioSource))
            {
                activeAudioSources.Add(backgroundAudioSource);
            }
        }
    }

    public void StopBackgroundMusic()
    {
        if (backgroundAudioSource != null && backgroundAudioSource.isPlaying)
        {
            backgroundAudioSource.Stop();
        }
    }

    public void AddSound(string key, AudioClip clip)
    {
        if (clip != null)
        {
            sfxMap.Put(key, clip);
        }
        else
        {
            Debug.LogError("Sound not found: " + key);
        }
    }

    public void PlaySoundAtPosition(string key, Vector3 position, float volume = 1.0f)
    {
        if (sfxMap.ContainsKey(key))
        {
            AudioClip clip = sfxMap.Get(key);
            float adjustedVolume = Mathf.Clamp(volume * globalVolume, 0.0f, 1.0f);
            AudioSource.PlayClipAtPoint(clip, position, adjustedVolume);
        }
        else
        {
            Debug.LogError("Sound not found: " + key);
        }
    }

    public void PlaySoundOnObject(string key, GameObject obj, float volume = 1.0f)
    {
        if (sfxMap.ContainsKey(key))
        {
            AudioClip clip = sfxMap.Get(key);
            AudioSource source = obj.GetComponent<AudioSource>();
            if (source == null) source = obj.AddComponent<AudioSource>();

            source.clip = clip;
            source.volume = Mathf.Clamp(volume * globalVolume, 0.0f, 1.0f);
            source.spatialBlend = 1.0f;
            source.Play();

            if (!activeAudioSources.Contains(source))
            {
                activeAudioSources.Add(source);
            }
        }
        else
        {
            Debug.LogError("Sound not found: " + key);
        }
    }

    public void PlayGlobalSound(string key, float volume = 1.0f)
    {
        if (sfxMap.ContainsKey(key))
        {
            if (globalAudioSource == null)
            {
                globalAudioSource = gameObject.AddComponent<AudioSource>();
                globalAudioSource.spatialBlend = 0f; // 2D sound
                globalAudioSource.playOnAwake = false;
            }

            AudioClip clip = sfxMap.Get(key);
            globalAudioSource.volume = Mathf.Clamp(volume * globalVolume, 0f, 1f);
            globalAudioSource.PlayOneShot(clip);

            if (!activeAudioSources.Contains(globalAudioSource))
            {
                activeAudioSources.Add(globalAudioSource);
            }
        }
        else
        {
            Debug.LogError("Global sound not found: " + key);
        }
    }

    public void PlayLoopedSoundOnObject(string key, GameObject obj, float volume = 1.0f)
    {
        if (sfxMap.ContainsKey(key))
        {
            AudioClip clip = sfxMap.Get(key);
            AudioSource source = obj.GetComponent<AudioSource>();
            if (source == null)
            {
                source = obj.AddComponent<AudioSource>();
            }

            source.clip = clip;
            source.loop = true;
            source.volume = Mathf.Clamp(volume * globalVolume, 0f, 1f);
            source.spatialBlend = 1.0f;

            if (!source.isPlaying)
            {
                source.Play();
            }

            if (!activeAudioSources.Contains(source))
            {
                activeAudioSources.Add(source);
            }
        }
        else
        {
            Debug.LogError("Sound not found: " + key);
        }
    }

    public void StopSoundOnObject(GameObject obj)
    {
        AudioSource source = obj.GetComponent<AudioSource>();
        if (source != null && source.isPlaying)
        {
            source.Stop();
        }
    }

    public void PauseAllSFX()
    {
        foreach (AudioSource source in activeAudioSources)
        {
            if (source != null && source.isPlaying)
            {
                source.Pause();
            }
        }

        if (backgroundAudioSource != null && backgroundAudioSource.isPlaying)
        {
            backgroundAudioSource.Pause();
        }

        if (globalAudioSource != null && globalAudioSource.isPlaying)
        {
            globalAudioSource.Pause();
        }
    }
    
    public void ResumeAllSFX()
    {
        foreach (AudioSource source in activeAudioSources)
        {
            if (source != null)
            {
                source.UnPause();
            }
        }

        if (backgroundAudioSource != null)
        {
            backgroundAudioSource.UnPause();
        }

        if (globalAudioSource != null)
        {
            globalAudioSource.UnPause();
        }
    }

    public void StopAllSFX()
    {
        foreach (AudioSource source in activeAudioSources)
        {
            if (source != null && source.isPlaying)
            {
                source.Stop();
            }
        }

        StopBackgroundMusic();

        if (globalAudioSource != null && globalAudioSource.isPlaying)
        {
            globalAudioSource.Stop();
        }

    }


    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StopBackgroundMusic();
        PlayBackgroundMusic("Background");  // your menu background music key
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header("Music Clips")]
    public AudioClip menuMusic;
    public AudioClip gameMusic;
    public AudioClip battleMusic;

    [Header("Mixer Group")]
    public AudioMixerGroup musicMixerGroup;

    private AudioSource audioSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.outputAudioMixerGroup = musicMixerGroup;
            audioSource.loop = true;
            audioSource.playOnAwake = false;

            SceneManager.sceneLoaded += OnSceneLoaded;
            PlayMusicForScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForScene(scene.name);
    }

    void PlayMusicForScene(string sceneName)
    {
        if (sceneName == "Menu")
            SwitchToMusic(menuMusic);
        else if (sceneName == "L1")
            SwitchToMusic(gameMusic);
    }

    public void SwitchToMusic(AudioClip newClip)
    {
        if (newClip == null) return;
        if (audioSource.clip == newClip) return;

        audioSource.clip = newClip;
        audioSource.Play();
    }

    public void StopMusic()
    {
        if (audioSource.isPlaying)
            audioSource.Stop();
    }

    public bool IsPlaying(AudioClip clip)
    {
        return audioSource.clip == clip && audioSource.isPlaying;
    }
}

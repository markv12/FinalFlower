using UnityEngine;

public class AudioManager : MonoBehaviour {

    private const string  AUDIO_MANAGER_PATH = "AudioManager";
    public static AudioManager instance;
    public static AudioManager Instance {
        get {
            if (instance == null) {
                GameObject gameOverScreenObject = (GameObject)Resources.Load(AUDIO_MANAGER_PATH);
                GameObject instantiated = Instantiate(gameOverScreenObject);
                DontDestroyOnLoad(instantiated);
                instance = instantiated.GetComponent<AudioManager>();
            }
            return instance;
        }
    }

    public AudioSource[] audioSources;

    public AudioSource backgroundMusic;
    public AudioClip shootSound;
    public AudioClip hitSound;
    
    private int audioSourceIndex = 0;


    void Awake() {
        instance = this;
        backgroundMusic.volume = 0;
        backgroundMusic.Play();
        this.CreateAnimationRoutine(
            1,
            delegate (float progress) {
                backgroundMusic.volume = progress;
            }
        );
    }

    public void PlayGunSound() {
        AudioSource source = GetNextAudioSource();
        source.volume = 1f;
        source.PlayOneShot(shootSound);
    }

    public void PlayHitSound() {
        AudioSource source = GetNextAudioSource();
        source.volume = 0.6f;
        source.PlayOneShot(hitSound);
    }

    private AudioSource GetNextAudioSource()
    {
        AudioSource result = audioSources[audioSourceIndex];
        audioSourceIndex = (audioSourceIndex + 1) % audioSources.Length;
        return result;
    }
}

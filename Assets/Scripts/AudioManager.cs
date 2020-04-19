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

    public AudioClip shootSound;
    
    private int audioSourceIndex = 0;


    void Awake() {
        instance = this;
    }

    public void PlayGunSound() {
        GetNextAudioSource().PlayOneShot(shootSound);
    }

    private AudioSource GetNextAudioSource()
    {
        AudioSource result = audioSources[audioSourceIndex];
        audioSourceIndex = (audioSourceIndex + 1) % audioSources.Length;
        return result;
    }
}

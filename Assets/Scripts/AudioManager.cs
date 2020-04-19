using UnityEngine;

public class AudioManager : MonoBehaviour {

    private const string  AUDIO_MANAGER_PATH = "AudioManager";
    private static AudioManager instance;
    public static AudioManager Instance {
        get {
            if (instance == null) {
                GameObject gameOverScreenObject = (GameObject)Resources.Load(AUDIO_MANAGER_PATH);
                GameObject instantiated = Instantiate(gameOverScreenObject);
                DontDestroyOnLoad(instantiated);
                instance = instantiated.GetComponent<AudioManager>();

                instance.warningMusic.volume = 0;
                instance.warningMusic.Play();

                instance.backgroundMusic.volume = 0;
                instance.backgroundMusic.Play();
                instance.CreateAnimationRoutine(
                    1,
                    delegate (float progress) {
                        instance.backgroundMusic.volume = progress;
                    }
                );
            }
            return instance;
        }
    }

    public AudioSource[] audioSources;

    public AudioSource backgroundMusic;
    public AudioSource warningMusic;
    public AudioClip shootSound;
    public AudioClip hitSound;
    
    private int audioSourceIndex = 0;

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

    private Coroutine warningMusicFadeRoutine = null;
    public void EnableWarningMusic(bool enabled) {
        this.EnsureCoroutineStopped(ref warningMusicFadeRoutine);
        float startVolume = warningMusic.volume;
        float endVolume = enabled ? 0.75f : 0;
        warningMusicFadeRoutine = this.CreateAnimationRoutine(
            0.5f,
            delegate (float progress) {
                warningMusic.volume = Mathf.Lerp(startVolume, endVolume, progress);
            }
        );
    }

    private AudioSource GetNextAudioSource()
    {
        AudioSource result = audioSources[audioSourceIndex];
        audioSourceIndex = (audioSourceIndex + 1) % audioSources.Length;
        return result;
    }
}

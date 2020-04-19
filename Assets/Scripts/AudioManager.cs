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

    public AudioSource menuMusic;
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

    public void PlayMenuMusic() {
        this.EnsureCoroutineStopped(ref musicFadeRoutine);
        float warningStartVolume = warningMusic.volume;
        float backgroundStartVolume = backgroundMusic.volume;
        float endVolume = 0;
        musicFadeRoutine = this.CreateAnimationRoutine(
            0.5f,
            delegate (float progress) {
                warningMusic.volume = Mathf.Lerp(warningStartVolume, endVolume, progress);
                backgroundMusic.volume = Mathf.Lerp(backgroundStartVolume, endVolume, progress);
            },
            delegate {
                warningMusic.Stop();
                backgroundMusic.Stop();
                menuMusic.Play();
                musicFadeRoutine = this.CreateAnimationRoutine(
                    1f,
                    delegate (float progress) {
                        menuMusic.volume = Mathf.Lerp(0, 1, progress);
                    }
                );
            }
        );
    }

    public void PlayGameMusic() {
        this.EnsureCoroutineStopped(ref musicFadeRoutine);
        float menuStartVolume = menuMusic.volume;
        float endVolume = 0;
        musicFadeRoutine = this.CreateAnimationRoutine(
            0.5f,
            delegate (float progress) {
                menuMusic.volume = Mathf.Lerp(menuStartVolume, endVolume, progress);
            },
            delegate {
                menuMusic.Stop();
                backgroundMusic.volume = 0;
                warningMusic.volume = 0;
                backgroundMusic.Play();
                warningMusic.Play();

                musicFadeRoutine = this.CreateAnimationRoutine(
                    1f,
                    delegate (float progress) {
                        backgroundMusic.volume = Mathf.Lerp(0, 1, progress);
                    }
                );
            }
        );
    }

    private Coroutine musicFadeRoutine = null;
    public void EnableWarningMusic(bool enabled) {
        this.EnsureCoroutineStopped(ref musicFadeRoutine);
        float startVolume = warningMusic.volume;
        float endVolume = enabled ? 0.6f : 0;
        musicFadeRoutine = this.CreateAnimationRoutine(
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

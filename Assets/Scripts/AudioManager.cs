using UnityEngine;

public class AudioManager : MonoBehaviour {

    public AudioSource[] audioSources;

    public AudioClip shootSound;
    
    private int audioSourceIndex = 0;

    public static AudioManager instance;

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

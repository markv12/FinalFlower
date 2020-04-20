using UnityEngine;

public class AudioManager : MonoBehaviour
{

	private const string AUDIO_MANAGER_PATH = "AudioManager";
	private static AudioManager instance;
	public static AudioManager Instance
	{
		get
		{
			if (instance == null)
			{
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
						delegate (float progress)
						{
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
	public AudioClip hitWallSound;
	public AudioClip hitPlayerSound;
	public AudioClip hitEnemySound;
	public AudioClip jumpSound;
	public AudioClip winSound;
	public AudioClip capsuleThrowSound;
	public AudioClip capsuleCatchSound;
	public AudioClip capsuleBreakSound;

	private int audioSourceIndex = 0;

	public void PlayGunSound()
	{
		PlaySFX(shootSound, .7f);
	}

	public void PlayHitWallSound()
	{
		PlaySFX(hitWallSound, 0.3f);
	}
	public void PlayHitPlayerSound()
	{
		PlaySFX(hitPlayerSound, 1.3f);
	}
	public void PlayHitEnemySound()
	{
		PlaySFX(hitEnemySound, 1.3f);
	}

	public void PlayJumpSound()
	{
		PlaySFX(jumpSound, 1f);
	}

	public void PlayWinSound()
	{
		PlaySFX(winSound, 0.5f);
	}

	public void PlayCapsuleThrow()
	{
		PlaySFX(capsuleThrowSound, 1.2f);
	}

	public void PlayCapsuleCatch()
	{
		PlaySFX(capsuleCatchSound, 0.8f);
	}

	public void PlayCapsuleBreak()
	{
		PlaySFX(capsuleBreakSound, 0.7f);
	}

	public void PlaySFX(AudioClip clip, float volume)
	{
		AudioSource source = GetNextAudioSource();
		source.volume = volume;
		source.PlayOneShot(clip);
	}

	public void PlayMenuMusic()
	{
		this.EnsureCoroutineStopped(ref musicFadeRoutine);
		float warningStartVolume = warningMusic.volume;
		float backgroundStartVolume = backgroundMusic.volume;
		float endVolume = 0;
		musicFadeRoutine = this.CreateAnimationRoutine(
				0.5f,
				delegate (float progress)
				{
					warningMusic.volume = Mathf.Lerp(warningStartVolume, endVolume, progress);
					backgroundMusic.volume = Mathf.Lerp(backgroundStartVolume, endVolume, progress);
				},
				delegate
				{
					warningMusic.Stop();
					backgroundMusic.Stop();
					menuMusic.Play();
					musicFadeRoutine = this.CreateAnimationRoutine(
									1f,
									delegate (float progress)
								{
									menuMusic.volume = Mathf.Lerp(0, 1, progress);
								}
							);
				}
		);
	}

	public void PlayGameMusic()
	{
		this.EnsureCoroutineStopped(ref musicFadeRoutine);
		float menuStartVolume = menuMusic.volume;
		float endVolume = 0;
		musicFadeRoutine = this.CreateAnimationRoutine(
				0.5f,
				delegate (float progress)
				{
					menuMusic.volume = Mathf.Lerp(menuStartVolume, endVolume, progress);
				},
				delegate
				{
					menuMusic.Stop();
					backgroundMusic.volume = 0;
					warningMusic.volume = 0;
					backgroundMusic.Play();
					warningMusic.Play();

					musicFadeRoutine = this.CreateAnimationRoutine(
									1f,
									delegate (float progress)
								{
									backgroundMusic.volume = Mathf.Lerp(0, 1, progress);
								}
							);
				}
		);
	}

	private Coroutine musicFadeRoutine = null;
	public void EnableWarningMusic(bool enabled)
	{
		this.EnsureCoroutineStopped(ref musicFadeRoutine);
		float startVolume = warningMusic.volume;
		float endVolume = enabled ? 0.2f : 0;
		musicFadeRoutine = this.CreateAnimationRoutine(
				0.5f,
				delegate (float progress)
				{
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

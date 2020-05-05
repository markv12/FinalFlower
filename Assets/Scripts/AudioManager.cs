using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
	private const string AUDIO_MANAGER_PATH = "AudioManager";
	private const float BG_MUSIC_VOLUME = 0.3f;
	private const float MENU_MUSIC_VOLUME = 0.6f;
	private static AudioManager instance;
	public static AudioManager Instance
	{
		get
		{
			if (instance == null) {
				GameObject gameOverScreenObject = (GameObject)Resources.Load(AUDIO_MANAGER_PATH);
				GameObject instantiated = Instantiate(gameOverScreenObject);
				DontDestroyOnLoad(instantiated);
				instance = instantiated.GetComponent<AudioManager>();
			}
			return instance;
		}
	}

	private void SetBGMClips(bool useClip1) {
		AudioClip bgmClip = useClip1 ? bgm1 : bgm2;
		AudioClip bgmWarningClip = useClip1 ? bgmWarning1 : bgmWarning2;
		backgroundMusicSource.clip = bgmClip;
		warningMusicSource.clip = bgmWarningClip;
	}

	[Header("Background Music")]
	public AudioSource menuMusic;
	public AudioSource backgroundMusicSource;
	public AudioSource warningMusicSource;
	public AudioSource endMusic;

	public AudioClip bgm1;
	public AudioClip bgmWarning1;
	public AudioClip bgm2;
	public AudioClip bgmWarning2;

	[Header("Sound Effects")]
	public AudioSource[] audioSources;
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
		PlaySFX(shootSound, .4f);
	}

	public void PlayHitWallSound()
	{
		PlaySFX(hitWallSound, 0.3f);
	}
	public void PlayHitPlayerSound()
	{
		PlaySFX(hitPlayerSound, 0.8f);
	}
	public void PlayHitEnemySound()
	{
		PlaySFX(hitEnemySound, 0.8f);
	}

	public void PlayJumpSound()
	{
		PlaySFX(jumpSound, 0.75f);
	}

	public void PlayWinSound()
	{
		PlaySFX(winSound, 0.5f);
	}

	public void PlayCapsuleThrow()
	{
		PlaySFX(capsuleThrowSound, 0.8f);
	}

	public void PlayCapsuleCatch()
	{
		PlaySFX(capsuleCatchSound, 0.8f);
	}

	public void PlayCapsuleBreak()
	{
		PlaySFX(capsuleBreakSound, 0.4f);
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
		float warningStartVolume = warningMusicSource.volume;
		float backgroundStartVolume = backgroundMusicSource.volume;
		float endMusicStartVolume = endMusic.volume;
		float endVolume = 0;
		musicFadeRoutine = this.CreateAnimationRoutine(
				0.5f,
				delegate (float progress)
				{
					warningMusicSource.volume = Mathf.Lerp(warningStartVolume, endVolume, progress);
					backgroundMusicSource.volume = Mathf.Lerp(backgroundStartVolume, endVolume, progress);
					endMusic.volume = Mathf.Lerp(endMusicStartVolume, endVolume, progress);
				},
				delegate
				{
					warningMusicSource.Stop();
					backgroundMusicSource.Stop();
					endMusic.Stop();
					menuMusic.Play();
					musicFadeRoutine = this.CreateAnimationRoutine(
							1f,
							delegate (float progress)
						{
							menuMusic.volume = Mathf.Lerp(0, MENU_MUSIC_VOLUME, progress);
						}
					);
				}
		);
	}

	private bool currentlyClip1 = false;
	public void PlayGameMusic(bool useClip1) {
		if (useClip1 != currentlyClip1 || backgroundMusicSource.volume <= 0 || !backgroundMusicSource.isPlaying) {
			this.EnsureCoroutineStopped(ref musicFadeRoutine);
			float menuStartVolume = menuMusic.volume;
			float bgmStartVolume = backgroundMusicSource.volume;
			float warningStartVolume = warningMusicSource.volume;
			float endVolume = 0;
			musicFadeRoutine = this.CreateAnimationRoutine(
					0.5f,
					delegate (float progress) {
						menuMusic.volume = Mathf.Lerp(menuStartVolume, endVolume, progress);
						backgroundMusicSource.volume = Mathf.Lerp(bgmStartVolume, endVolume, progress);
						warningMusicSource.volume = Mathf.Lerp(warningStartVolume, endVolume, progress);
					},
					delegate {
						menuMusic.Stop();
						SetBGMClips(useClip1);
						currentlyClip1 = useClip1;
						backgroundMusicSource.volume = 0;
						warningMusicSource.volume = 0;
						backgroundMusicSource.Play();
						warningMusicSource.Play();

						musicFadeRoutine = this.CreateAnimationRoutine(
										1f,
										delegate (float progress) {
											backgroundMusicSource.volume = Mathf.Lerp(0, BG_MUSIC_VOLUME, progress);
										}
								);
					}
			);
		}
	}

	private Coroutine musicFadeRoutine = null;
	public void EnableWarningMusic(bool enabled)
	{
		this.EnsureCoroutineStopped(ref musicFadeRoutine);
		float startVolume = warningMusicSource.volume;
		float endVolume = enabled ? 0.07f : 0;
		musicFadeRoutine = this.CreateAnimationRoutine(
				0.5f,
				delegate (float progress)
				{
					warningMusicSource.volume = Mathf.Lerp(startVolume, endVolume, progress);
				}
		);
	}

	public void PlayEndMusic()
	{
		this.EnsureCoroutineStopped(ref musicFadeRoutine);
		float warningStartVolume = warningMusicSource.volume;
		float backgroundStartVolume = backgroundMusicSource.volume;
		float endVolume = 0;
		musicFadeRoutine = this.CreateAnimationRoutine(
				0.5f,
				delegate (float progress)
				{
					warningMusicSource.volume = Mathf.Lerp(warningStartVolume, endVolume, progress);
					backgroundMusicSource.volume = Mathf.Lerp(backgroundStartVolume, endVolume, progress);
				},
				delegate
				{
					warningMusicSource.Stop();
					backgroundMusicSource.Stop();
					endMusic.Play();
					musicFadeRoutine = this.CreateAnimationRoutine(
									1f,
									delegate (float progress)
									{
										endMusic.volume = Mathf.Lerp(0, BG_MUSIC_VOLUME, progress);
									}
							);
				}
		);
	}

	private AudioSource GetNextAudioSource()
	{
		AudioSource result = audioSources[audioSourceIndex];
		audioSourceIndex = (audioSourceIndex + 1) % audioSources.Length;
		return result;
	}

	public static bool UseClip1ForSceneName(string sceneName) {
		return sceneName == Scenes.LEVEL_1.Name()
			|| sceneName == Scenes.LEVEL_2.Name()
			|| sceneName == Scenes.LEVEL_3.Name()
			|| sceneName == Scenes.LEVEL_4.Name()
			|| sceneName == Scenes.LEVEL_5.Name();
	}
}

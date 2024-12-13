using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Audio EnviromentEnviroment")]
    public AudioClip BackgroundMusic, MusicBoss, DoorOpen, DoorClose, Laser, Acid, Touch;

    [Header("Audio Player")]
    public AudioClip PJump, PHurt, PDie, PGunBlast;

    [Header("Acid Alien")]
    public AudioClip AARoar, AASpit, AAttack, AADie, AAHurt;

    [Header("Acid Ske")]
    public AudioClip ASRoar, ASDie;

    [Header("Boss")]
    public AudioClip BSkill1, BSkill2, BAttack, BDie, BHurt;

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (BackgroundMusic != null)
        {
            audioSource.clip = BackgroundMusic;
            audioSource.loop = true;
            audioSource.volume = 0.5f;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("BackgroundMusic is not assigned!");
        }
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            if (audioSource != null)
            {
                audioSource.PlayOneShot(clip);
            }
            else
            {
                Debug.LogError("AudioSource is missing on SoundManager!");
            }
        }
    }

    public void PlaySoundAtPosition(AudioClip clip, Vector3 position)
    {
        if (clip == null)
        {
            Debug.LogWarning("Audio clip is null!");
            return;
        }

        AudioSource.PlayClipAtPoint(clip, position);
    }

    public void MuteAudio(bool isMuted)
    {
        if (audioSource != null)
        {
            audioSource.mute = isMuted;
        }
        else
        {
            Debug.LogError("AudioSource is missing on SoundManager!");
        }
    }
    public void StopSound(AudioClip clip)
    {
        if (audioSource != null && audioSource.isPlaying && audioSource.clip == clip)
        {
            audioSource.Stop();
        }
        else
        {
            Debug.LogWarning("The specified audio clip is not currently playing or AudioSource is missing!");
        }
    }

}

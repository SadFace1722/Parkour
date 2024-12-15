using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    public AudioSource BackgroundMusic, MusicBoss;

    [Header("Audio EnviromentEnviroment")]
    public AudioClip DoorOpen, DoorClose, Laser, Acid, Touch;

    [Header("Audio Player")]
    public AudioClip PJump, PHurt, PDie, PGunBlast;

    [Header("Acid Alien")]
    public AudioClip AARoar, AASpit, AAttack, AADie, AAHurt;

    [Header("Acid Ske")]
    public AudioClip ASDie, ASHurt, ASAttack;

    [Header("Boss")]
    public AudioClip BSkill1, BSkill2, BAttack, BDie, BHurt;

    public AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
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


    public void PlayBackgroundMusic()
    {
        if (BackgroundMusic != null && !BackgroundMusic.isPlaying)
        {
            // Dừng nhạc Boss nếu đang phát
            if (MusicBoss != null && MusicBoss.isPlaying)
            {
                MusicBoss.Stop();
            }

            // Phát nhạc nền
            BackgroundMusic.Play();
        }
    }

    public void PlayMusicBoss()
    {
        if (MusicBoss != null && !MusicBoss.isPlaying)
        {
            // Dừng nhạc nền nếu đang phát
            if (BackgroundMusic != null && BackgroundMusic.isPlaying)
            {
                BackgroundMusic.Stop();
            }

            // Phát nhạc Boss
            MusicBoss.Play();
        }
    }
    public void StopMusicGame()
    {
        BackgroundMusic.Stop();
        MusicBoss.Stop();
    }
}

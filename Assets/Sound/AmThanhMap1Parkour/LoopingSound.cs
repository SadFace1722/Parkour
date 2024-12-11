using UnityEngine;

public class LoopingSound : MonoBehaviour
{
    public AudioClip soundClip; // Âm thanh cần phát
    private AudioSource audioSource; // Component AudioSource
    public float detectionRadius = 10f; // Khoảng cách phát hiện (có thể điều chỉnh trong Inspector)
    public float playDistance = 5f; // Khoảng cách để âm thanh bắt đầu phát
    private bool isSoundPlaying = false; // Kiểm tra xem âm thanh đã được phát hay chưa

    private Transform player; // Tham chiếu đến Player

    void Start()
    {
        // Lấy AudioSource từ GameObject
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            // Nếu không có AudioSource, thêm vào GameObject
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Gán âm thanh cho AudioSource
        audioSource.clip = soundClip;

        // Đảm bảo không có âm thanh nào được phát lúc đầu
        audioSource.loop = true;
        audioSource.playOnAwake = false;

        // Tìm kiếm Player trong scene (có thể điều chỉnh nếu tên đối tượng khác)
        player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        // Kiểm tra khoảng cách giữa Player và đối tượng phát âm thanh
        float distance = Vector3.Distance(player.position, transform.position);

        // Nếu Player nằm trong vùng phát âm thanh và âm thanh chưa phát
        if (distance <= playDistance && !isSoundPlaying)
        {
            PlaySound();
        }
        // Nếu Player ra ngoài vùng phát âm thanh và âm thanh đang phát
        else if (distance > playDistance && isSoundPlaying)
        {
            StopSound();
        }
    }

    // Phát âm thanh
    private void PlaySound()
    {
        isSoundPlaying = true;
        audioSource.Play();
    }

    // Dừng âm thanh
    private void StopSound()
    {
        isSoundPlaying = false;
        audioSource.Stop();
    }
}

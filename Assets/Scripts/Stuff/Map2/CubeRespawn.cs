using System.Collections;
using UnityEngine;

public class CubeRespawn : MonoBehaviour
{
    public float checkRadius = 5f;           // Bán kính phát hiện Player
    public LayerMask collisionLayer;        // Lớp kiểm tra va chạm
    public float disappearDelay = 2f;       // Thời gian trước khi Cube biến mất
    public float respawnDelay = 5f;         // Thời gian để Cube hiện lại
    public AudioClip touchSoundClip;        // Âm thanh phát khi chạm vào
    public AudioClip disappearSoundClip;    // Âm thanh phát khi Cube biến mất
    public ParticleSystem crackEffect;      // Hiệu ứng nứt vỡ

    private AudioSource audioSource;        // Component AudioSource
    private Renderer cubeRenderer;          // Quản lý Renderer để ẩn/hiện Cube
    private Collider cubeCollider;          // Quản lý Collider để kích hoạt/không kích hoạt va chạm
    private bool isDisappearing = false;    // Kiểm tra nếu Cube đang biến mất

    private void Start()
    {
        cubeRenderer = GetComponent<Renderer>();
        cubeCollider = GetComponent<Collider>();

        // Lấy AudioSource từ GameObject
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if (!isDisappearing)
        {
            CheckCollision();
        }
    }

    private void CheckCollision()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, checkRadius, collisionLayer);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player") && !isDisappearing)
            {
                Debug.Log("Player đã chạm vào Cube");

                // Phát âm thanh khi chạm vào
                PlayTouchSound();

                StartCoroutine(DisappearAndRespawn());
                break;
            }
        }
    }

    private IEnumerator DisappearAndRespawn()
    {
        isDisappearing = true; // Đánh dấu Cube đang biến mất

        // Chờ trước khi Cube biến mất
        yield return new WaitForSeconds(disappearDelay);

        // Phát âm thanh khi Cube biến mất
        PlayDisappearSound();

        // Kích hoạt hiệu ứng nứt
        PlayCrackEffect();

        // Ẩn Cube bằng cách tắt Renderer và Collider
        cubeRenderer.enabled = false;
        cubeCollider.enabled = false;

        // Chờ thời gian để Cube hiện lại
        yield return new WaitForSeconds(respawnDelay);

        // Hiện lại Cube bằng cách bật Renderer và Collider
        cubeRenderer.enabled = true;
        cubeCollider.enabled = true;

        isDisappearing = false; // Cho phép kiểm tra va chạm lại
    }

    private void PlayTouchSound()
    {
        if (touchSoundClip != null)
        {
            audioSource.PlayOneShot(touchSoundClip);
        }
    }

    private void PlayDisappearSound()
    {
        if (disappearSoundClip != null)
        {
            audioSource.PlayOneShot(disappearSoundClip);
        }
    }

    private void PlayCrackEffect()
    {
        if (crackEffect != null)
        {
            // Tạo hiệu ứng tại vị trí Cube
            crackEffect.transform.position = transform.position;
            crackEffect.transform.rotation = transform.rotation;
            crackEffect.Play();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}

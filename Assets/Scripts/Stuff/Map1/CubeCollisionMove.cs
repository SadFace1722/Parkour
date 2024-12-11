using System.Collections;
using UnityEngine;

public class CubeCollisionMove : MonoBehaviour
{
    public float checkRadius = 5f;        // Bán kính kiểm tra va chạm
    public LayerMask collisionLayer;      // Lớp dùng để kiểm tra va chạm
    public float moveDistance = 5f;       // Khoảng cách di chuyển sang trái hoặc phải
    public float moveDuration = 1f;       // Thời gian di chuyển đến vị trí mới
    public AudioClip soundClip;           // Âm thanh cần phát

    private AudioSource audioSource;      // Component AudioSource

    private Vector3 originalPosition;
    public bool moveLeft = true;          // Biến chọn hướng di chuyển: true cho trái, false cho phải
    private bool hasMovedOnce = false;    // Kiểm tra Cube đã di chuyển

    private void Start()
    {
        originalPosition = transform.position;  // Lưu vị trí ban đầu của Cube

        // Lấy AudioSource từ GameObject
        audioSource = GetComponent<AudioSource>();

        // Nếu không có AudioSource, thêm vào GameObject
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if (!hasMovedOnce)  // Chỉ kiểm tra va chạm nếu Cube chưa di chuyển lần nào
        {
            CheckCollision();
        }
    }

    private void CheckCollision()
    {
        // Kiểm tra các va chạm xung quanh Cube
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, checkRadius, collisionLayer);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player") && !hasMovedOnce)
            {
                Debug.Log("Player đã chạm vào Cube");

                // Bắt đầu di chuyển Cube và phát âm thanh
                StartCoroutine(MoveCube());
                PlaySoundOnce();
                break;
            }
        }
    }

    private IEnumerator MoveCube()
    {
        hasMovedOnce = true; // Đánh dấu Cube đã di chuyển lần đầu tiên

        // Tính toán vị trí mục tiêu cho từng hướng di chuyển
        Vector3 targetPosition = originalPosition;

        if (moveLeft)
        {
            targetPosition = originalPosition + Vector3.left * moveDistance;
        }
        else
        {
            targetPosition = originalPosition + Vector3.right * moveDistance;
        }

        // Di chuyển Cube đến vị trí mục tiêu
        float elapsedTime = 0f;
        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(originalPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition; // Đặt Cube tại vị trí mục tiêu
    }

    // Phát âm thanh một lần duy nhất
    private void PlaySoundOnce()
    {
        if (soundClip != null)
        {
            audioSource.PlayOneShot(soundClip);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}

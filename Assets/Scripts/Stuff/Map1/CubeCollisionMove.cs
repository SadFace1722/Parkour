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
    private bool isPlayerInRadius = false; // Kiểm tra trạng thái Player trong vùng
    private bool isMoving = false;        // Trạng thái di chuyển
    public bool moveLeft = true;          // Biến chọn hướng di chuyển: true cho trái, false cho phải

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
        CheckPlayerInRadius();
    }

    private void CheckPlayerInRadius()
    {
        // Kiểm tra Player trong vùng bán kính checkRadius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, checkRadius, collisionLayer);
        bool playerDetected = false;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                playerDetected = true;
                break;
            }
        }

        // Xử lý khi Player vào vùng hoặc rời vùng
        if (playerDetected && !isPlayerInRadius && !isMoving)
        {
            isPlayerInRadius = true;
            StartCoroutine(MoveCube()); // Di chuyển Cube
            PlaySoundOnce();
        }
        else if (!playerDetected && isPlayerInRadius && !isMoving)
        {
            isPlayerInRadius = false;
            StartCoroutine(MoveCubeBack()); // Di chuyển Cube về vị trí ban đầu
        }
    }

    private IEnumerator MoveCube()
    {
        isMoving = true; // Đặt trạng thái Cube đang di chuyển
        Vector3 targetPosition = originalPosition + (moveLeft ? Vector3.left : Vector3.right) * moveDistance;

        // Di chuyển Cube đến vị trí mục tiêu
        float elapsedTime = 0f;
        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(originalPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition; // Đặt Cube tại vị trí mục tiêu
        isMoving = false; // Kết thúc di chuyển
    }

    private IEnumerator MoveCubeBack()
    {
        isMoving = true; // Đặt trạng thái Cube đang di chuyển
        Vector3 currentPosition = transform.position;

        // Di chuyển Cube trở lại vị trí ban đầu
        float elapsedTime = 0f;
        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(currentPosition, originalPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = originalPosition; // Đặt Cube về vị trí ban đầu
        isMoving = false; // Kết thúc di chuyển
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
        // Vẽ bán kính kiểm tra va chạm
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}

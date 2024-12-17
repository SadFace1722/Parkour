using System.Collections;
using UnityEngine;

public class CubeCollisionMoveVertical : MonoBehaviour
{
    public float checkRadius = 5f;      // Bán kính kiểm tra vùng
    public LayerMask collisionLayer;    // Layer của Player
    public float moveDistance = 5f;     // Khoảng cách Cube di chuyển
    public float moveDuration = 1f;     // Thời gian để di chuyển
    public AudioClip soundClip;         // Âm thanh khi di chuyển

    private AudioSource audioSource;    // AudioSource để phát âm thanh
    private Vector3 originalPosition;   // Lưu vị trí ban đầu của Cube
    private bool isMoving = false;      // Đánh dấu đang di chuyển
    private bool playerInside = false;  // Đánh dấu Player đang trong vùng

    private void Start()
    {
        originalPosition = transform.position; // Lưu lại vị trí ban đầu của Cube

        // Kiểm tra và thêm AudioSource nếu không có
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Update()
    {
        CheckCollision();
    }

    private void CheckCollision()
    {
        // Kiểm tra xem Player có trong vùng không
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, checkRadius, collisionLayer);
        bool playerInZone = false;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                playerInZone = true;
                break;
            }
        }

        // Nếu Player mới vào vùng và Cube không đang di chuyển, thực hiện di chuyển
        if (playerInZone && !playerInside && !isMoving)
        {
            playerInside = true;
            StartCoroutine(MoveCubeVertical());
        }
        else if (!playerInZone)
        {
            // Reset trạng thái khi Player rời khỏi vùng
            playerInside = false;
        }
    }

    private IEnumerator MoveCubeVertical()
    {
        isMoving = true; // Đánh dấu đang di chuyển

        Vector3 targetPosition = originalPosition + Vector3.up * moveDistance; // Vị trí di chuyển lên

        // Di chuyển Cube lên
        float elapsedTime = 0f;
        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(originalPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;

        PlaySoundOnce(); // Phát âm thanh

        // Chờ một chút trước khi quay lại
        yield return new WaitForSeconds(0.5f);

        // Di chuyển trở về vị trí ban đầu
        elapsedTime = 0f;
        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(targetPosition, originalPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = originalPosition;

        isMoving = false; // Cho phép di chuyển lại khi Player vào vùng lần tiếp theo
    }

    private void PlaySoundOnce()
    {
        if (soundClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(soundClip);
        }
    }

    private void OnDrawGizmos()
    {
        // Vẽ bán kính kiểm tra vùng trong Scene
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}

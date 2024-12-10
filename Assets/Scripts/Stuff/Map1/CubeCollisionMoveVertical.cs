using System.Collections;
using UnityEngine;

public class CubeCollisionMoveVertical : MonoBehaviour
{
    public float checkRadius = 5f;
    public LayerMask collisionLayer;
    public float moveDistance = 5f;   // Khoảng cách di chuyển
    public float moveDuration = 1f;   // Thời gian di chuyển đến vị trí mới
    public AudioClip soundClip;       // Âm thanh cần phát

    private AudioSource audioSource;   // Component AudioSource

    private Vector3 originalPosition;
    private bool hasMovedOnce = false; // Biến kiểm tra Cube đã di chuyển

    private void Start()
    {
        originalPosition = transform.position; // Lưu vị trí ban đầu của Cube

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
        if (!hasMovedOnce)
        {
            CheckCollision();
        }
    }

    private void CheckCollision()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, checkRadius, collisionLayer);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player") && !hasMovedOnce)
            {
                Debug.Log("Player đã chạm vào Cube");

                // Bắt đầu di chuyển Cube theo hướng lên/xuống và phát âm thanh
                StartCoroutine(MoveCubeVertical());
                PlaySoundOnce();
                break;
            }
        }
    }

    private IEnumerator MoveCubeVertical()
    {
        hasMovedOnce = true; // Đánh dấu Cube đã di chuyển lần đầu tiên
        Vector3 targetPosition = originalPosition + Vector3.up * moveDistance;

        // Di chuyển Cube lên
        float elapsedTime = 0f;
        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(originalPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;

        // Quay lại vị trí ban đầu
        yield return new WaitForSeconds(0.5f);  // Thời gian nghỉ giữa di chuyển lên và xuống
        elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(targetPosition, originalPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = originalPosition;
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
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}

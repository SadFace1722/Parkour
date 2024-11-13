using System.Collections;
using UnityEngine;

public class CubeCollisionMove : MonoBehaviour
{
    public float checkRadius = 5f;
    public LayerMask collisionLayer;
    public float moveDistance = 5f;   // Khoảng cách di chuyển sang trái, phải hoặc lên
    public float moveDuration = 1f;   // Thời gian di chuyển đến vị trí mới

    private Vector3 originalPosition;
    public bool moveLeft = true;  // Biến chọn hướng di chuyển: true cho trái, false cho phải
    public bool moveUp = false;   // Biến cho phép di chuyển lên
    private bool hasMovedOnce = false; // Biến để kiểm tra xem Cube đã di chuyển một lần hay chưa

    private void Start()
    {
        originalPosition = transform.position; // Lưu vị trí ban đầu của Cube
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

                // Bắt đầu di chuyển Cube theo hướng đã chọn
                StartCoroutine(MoveCube());
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
        else if (moveUp)
        {
            targetPosition = originalPosition + Vector3.up * moveDistance;  // Di chuyển lên
        }
        else
        {
            targetPosition = originalPosition + Vector3.right * moveDistance;
        }

        // Di chuyển đến vị trí mục tiêu
        float elapsedTime = 0f;
        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(originalPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition; // Đặt Cube tại vị trí mục tiêu

        // Sau khi di chuyển xong, quay lại vị trí ban đầu
        yield return new WaitForSeconds(0.5f); // Chờ một chút trước khi quay lại
        elapsedTime = 0f; // Reset thời gian

        // Di chuyển Cube trở lại vị trí ban đầu
        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(targetPosition, originalPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = originalPosition; // Đặt Cube tại vị trí ban đầu
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}

using System.Collections;
using UnityEngine;

public class CubeCollisionMove : MonoBehaviour
{
    public float checkRadius = 5f;
    public LayerMask collisionLayer;
    public float moveDistance = 5f;   // Khoảng cách di chuyển sang trái hoặc phải
    public float moveDuration = 1f;   // Thời gian di chuyển đến vị trí mới

    private Vector3 originalPosition;
    public bool moveLeft = true;  // Biến chọn hướng di chuyển: true cho trái, false cho phải
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

        Vector3 targetPosition = originalPosition + (moveLeft ? Vector3.left : Vector3.right) * moveDistance;
        float elapsedTime = 0f;

        // Di chuyển đến vị trí mục tiêu
        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(originalPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition; // Đặt Cube tại vị trí mục tiêu mà không quay lại
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}

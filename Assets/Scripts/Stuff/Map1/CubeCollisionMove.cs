using System.Collections;
using UnityEngine;

public class CubeCollisionMove : MonoBehaviour
{
    public float checkRadius = 5f;
    public LayerMask collisionLayer;
    public float moveDistance = 5f;   // Khoảng cách di chuyển
    public float moveDuration = 1f;   // Thời gian di chuyển đến vị trí mới
    public bool moveLeftRight = true; // Di chuyển trái-phải
    public bool moveUpDown = false;   // Di chuyển lên-xuống

    private Vector3 originalPosition;
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

        // Tính toán vị trí đích dựa trên hướng di chuyển
        Vector3 targetPosition = originalPosition;

        if (moveLeftRight)
        {
            targetPosition += Vector3.right * moveDistance;
        }
        else if (moveUpDown)
        {
            targetPosition += Vector3.up * moveDistance;
        }

        float elapsedTime = 0f;

        // Di chuyển đến vị trí mục tiêu
        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(originalPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition; // Đặt Cube tại vị trí mục tiêu

        // Nếu di chuyển lên-xuống, di chuyển Cube quay lại vị trí ban đầu
        if (moveUpDown)
        {
            yield return new WaitForSeconds(0.5f); // Chờ một khoảng thời gian trước khi quay lại
            elapsedTime = 0f;

            while (elapsedTime < moveDuration)
            {
                transform.position = Vector3.Lerp(targetPosition, originalPosition, elapsedTime / moveDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = originalPosition; // Đặt Cube về vị trí ban đầu
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}

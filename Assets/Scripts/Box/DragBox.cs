using UnityEngine;

public class DragBox : MonoBehaviour
{
    private Camera mainCamera;
    private bool isDragging = false;
    private Vector3 offset;
    private Rigidbody rb;

    void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody is required on the Cube for collision detection!");
        }
    }

    void OnMouseDown()
    {
        // Bắt đầu kéo
        isDragging = true;
        offset = transform.position - GetMouseWorldPosition();
        rb.isKinematic = true; // Tắt vật lý khi kéo
    }

    void OnMouseUp()
    {
        // Dừng kéo
        isDragging = false;
        rb.isKinematic = false; // Bật lại vật lý
    }

    void FixedUpdate()
    {
        if (isDragging)
        {
            Vector3 newPosition = GetMouseWorldPosition() + offset;
            newPosition.y = transform.position.y; // Giữ nguyên độ cao

            // Di chuyển Cube với kiểm tra va chạm
            rb.MovePosition(newPosition);
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.point;
        }
        return Vector3.zero;
    }
}

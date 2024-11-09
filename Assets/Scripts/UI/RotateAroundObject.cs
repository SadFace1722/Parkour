using UnityEngine;

public class RotateAroundObject : MonoBehaviour
{
    public Transform target;             // Đối tượng mà camera sẽ xoay quanh
    public float rotationSpeed = 10f;    // Tốc độ xoay của camera
    public float distance = 5f;          // Khoảng cách giữa camera và đối tượng

    public float rotationX = 0f;         // Góc xoay quanh trục X
    public float rotationY = 0f;         // Góc xoay quanh trục Y
    public float rotationZ = 0f;         // Góc xoay quanh trục Z

    private float currentAngle = 0f;     // Góc hiện tại của camera quanh đối tượng

    void Update()
    {
        if (target == null) return;

        // Tăng góc xoay dựa trên thời gian và tốc độ
        currentAngle += rotationSpeed * Time.deltaTime;

        // Tính toán vị trí mới của camera dựa trên các góc xoay
        Vector3 offset = new Vector3(
            distance * Mathf.Cos(currentAngle * Mathf.Deg2Rad + rotationX * Mathf.Deg2Rad),
            rotationY,  // Điều chỉnh độ cao trên trục Y
            distance * Mathf.Sin(currentAngle * Mathf.Deg2Rad + rotationZ * Mathf.Deg2Rad)
        );

        // Đặt vị trí mới cho camera và tính toán khoảng cách
        transform.position = target.position + offset;

        // Camera luôn nhìn về phía đối tượng
        transform.LookAt(target);
    }
}

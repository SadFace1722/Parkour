using UnityEngine;

public class Box : MonoBehaviour
{
    public bool isInCorrectPosition = false; // Đã đặt đúng vị trí hay chưa
    public Transform targetPosition;        // Vị trí cần đặt

    void Update()
    {
        // Kiểm tra nếu hộp gần vị trí cần đặt
        if (Vector3.Distance(transform.position, targetPosition.position) < 0.5f)
        {
            isInCorrectPosition = true;
        }
        else
        {
            isInCorrectPosition = false;
        }
    }
}

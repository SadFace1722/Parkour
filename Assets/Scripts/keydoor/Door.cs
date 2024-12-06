using UnityEngine;

public class Door : MonoBehaviour
{
    public Animator doorAnimator; // Gắn Animator của cửa
    private bool isOpen = false; // Biến kiểm tra trạng thái cửa

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player detected near door!");

        // Kiểm tra nếu Player có chìa khóa và cửa chưa mở
        if (other.CompareTag("Player") && PlayerInventory.hasKey && !isOpen)
        {
            Debug.Log("Opening the door...");
            isOpen = true; // Đánh dấu cửa đã mở
            doorAnimator.SetTrigger("Open"); // Kích hoạt animation mở cửa
            Debug.Log("Door unlocked and opened!");
        }
        else if (!PlayerInventory.hasKey)
        {
            Debug.Log("You need a key to open this door!");
        }
    }
}

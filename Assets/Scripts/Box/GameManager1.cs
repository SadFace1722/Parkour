using UnityEngine;

public class GameManager1 : MonoBehaviour
{
    public Box[] boxes;  // Các hộp trong trò chơi
    public Animator doorAnimator; // Animator của cửa

    void Update()
    {
        // Kiểm tra tất cả các hộp đã đặt đúng vị trí chưa
        if (AllBoxesInPosition())
        {
            OpenDoor();
        }
    }

    private bool AllBoxesInPosition()
    {
        foreach (Box box in boxes)
        {
            if (!box.isInCorrectPosition)
                return false;
        }
        return true;
    }

    private void OpenDoor()
    {
        doorAnimator.SetTrigger("Open");
        Debug.Log("Cửa đã mở!");
    }
}

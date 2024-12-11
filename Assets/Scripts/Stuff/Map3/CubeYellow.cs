using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeYellow : MonoBehaviour, PlayerInterface
{
    Animator animator;

    private void Start()
    {
        animator = transform.parent.GetComponent<Animator>();
        animator.SetBool("button", false); // Đảm bảo bắt đầu ở trạng thái idle
    }

    public void Interact()
    {
        if (GameLaser.Instance.isGameOver)
        {
            Debug.Log("Game Over, you can't interact with this cube.");
            return;
        }

        // Thay đổi trạng thái animation khi người chơi nhấn
        if (animator.GetBool("button"))
        {
            animator.SetBool("button", false); // Quay lại trạng thái idle ngay lập tức
        }
        else
        {
            animator.SetBool("button", true); // Chuyển sang trạng thái thụt vào
        }

        // Kiểm tra thứ tự của Yellow
        GameLaser.Instance.CheckOrder("Yellow");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeYellow : MonoBehaviour, PlayerInterface
{
    public static bool isYellowPressed = false; // Biến kiểm tra xem người chơi có nhấn đúng Yellow hay không
    Animator animator;

    private void Start()
    {
        animator = transform.parent.GetComponent<Animator>();
        animator.SetBool("button", false); // Đảm bảo bắt đầu với trạng thái idle
    }

    private void Update()
    {
        // Animation thụt vào khi nút bị nhấn
        if (isYellowPressed)
        {
            animator.SetBool("button", true); // Thực hiện animation
        }
    }

    public void Interact()
    {
        if (GameLaser.Instance.isGameOver)
        {
            Debug.Log("Game Over, you can't interact with this cube.");
            return;
        }

        // Khi người chơi nhấn vào cube vàng
        if (GameLaser.Instance.CheckOrder("Yellow") && !isYellowPressed) // Chỉ khi nhấn đúng và chưa nhấn
        {
            isYellowPressed = true;
            Debug.Log("Yellow Cube pressed correctly!");
        }
        else
        {
            GameLaser.Instance.ResetOrder();
            Debug.Log("Wrong order! Resetting.");
            DisableButton(); // Vô hiệu hóa nút sau khi sai thứ tự
        }
    }

    // Vô hiệu hóa collider sau khi sai thứ tự
    private void DisableButton()
    {
        GetComponent<Collider>().enabled = false; // Vô hiệu hóa collider của CubeYellow
    }
}

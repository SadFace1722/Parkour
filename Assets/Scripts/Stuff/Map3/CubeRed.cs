using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeRed : MonoBehaviour, PlayerInterface
{
    public static bool isRedPressed = false; // Biến kiểm tra xem người chơi có nhấn đúng Red hay không
    Animator animator;

    private void Start()
    {
        animator = transform.parent.GetComponent<Animator>();
        animator.SetBool("button", false); // Đảm bảo bắt đầu với trạng thái idle
    }

    private void Update()
    {
        // Animation thụt vào khi nút bị nhấn
        if (isRedPressed)
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

        // Khi người chơi nhấn vào cube đỏ
        if (GameLaser.Instance.CheckOrder("Red") && !isRedPressed) // Chỉ khi nhấn đúng và chưa nhấn
        {
            isRedPressed = true;
            Debug.Log("Red Cube pressed correctly!");
        }
        else
        {
            GameLaser.Instance.ResetOrder();
            Debug.Log("Wrong order! Resetting.");
            DisableButton(); // Vô hiệu hóa nút sau khi sai thứ tự
        }

        // Dù đúng hay sai thứ tự, animation thụt vào vẫn phải xảy ra khi ấn
        if (!isRedPressed)
        {
            animator.SetBool("button", true);
        }
    }

    // Vô hiệu hóa collider sau khi sai thứ tự
    private void DisableButton()
    {
        GetComponent<Collider>().enabled = false; // Vô hiệu hóa collider của CubeRed
    }
}

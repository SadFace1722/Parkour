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
    }
    private void Update()
    {
        AnimaButton();
    }
    public void Interact()
    {
        if (GameLaser.Instance.isGameOver)
        {
            Debug.Log("isGameOver = " + GameLaser.Instance.isGameOver);

            return;
        }
        // Khi người chơi nhấn vào cube vàng
        if (GameLaser.Instance.CheckOrder("Yellow"))
        {
            isYellowPressed = true;
            Debug.Log("Yellow Cube pressed correctly!");
        }
        else
        {
            GameLaser.Instance.ResetOrder();
            Debug.Log("Wrong order! Resetting.");
        }
    }
    public void AnimaButton()
    {
        if (isYellowPressed == true)
        {
            animator.SetBool("button", true);
        }
        else if (GameLaser.Instance.currentStep == 0)
        {
            animator.SetBool("button", false);
        }
    }
}

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
    }
    private void Update()
    {
        AnimaButton();
    }
    public void Interact()
    {
        // Khi người chơi nhấn vào cube đỏ
        if (GameLaser.Instance.CheckOrder("Red"))
        {
            isRedPressed = true;
            Debug.Log("Red Cube pressed correctly!");
        }
        else
        {
            GameLaser.Instance.ResetOrder();
            Debug.Log("Wrong order! Resetting.");
        }
    }
    public void AnimaButton()
    {
        if (isRedPressed == true)
        {
            animator.SetBool("button", true);
        }
        else if (GameLaser.Instance.currentStep==0)
        {
            animator.SetBool("button", false);
        }
    }                   
}

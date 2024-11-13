using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    public Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Invoke("PlayerAnim", 0.2f);
        Death();
    }

    void PlayerAnim()
    {
        if (PlayerController.Instance.isGrounded)
        {
            anim.SetBool("Jump", false);
            anim.SetBool("Move", PlayerController.Instance.isMoving);
        }
        else
        {
            anim.SetBool("Jump", PlayerController.Instance.isJumping);
        }
    }

    void Death()
    {
        anim.SetBool("Death", PlayerController.Instance.isDead);
    }
}

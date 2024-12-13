using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeRed : MonoBehaviour, PlayerInterface
{
    public static CubeRed Instance;
    Animator animator;
    public bool Active;
    private void Awake()
    {
        if (Instance == null)

        {
            Instance = this;
        }
    }
    private void Start()
    {
        animator = transform.parent.GetComponent<Animator>();
    }
    public void Interact()
    {
        Active = true;
        animator.SetBool("button", Active = true);
    }
    private void LateUpdate()
    {
        animator.SetBool("button", Active);
    }
}

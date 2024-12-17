﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeYellow : MonoBehaviour, PlayerInterface
{
    public static CubeYellow Instance;
    public bool isActive = false;
    Animator anim;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void Start()
    {
        anim = transform.parent.GetComponent<Animator>();
    }
    public void Interact()
    {
        GameLaser.Instance.OnYellowButtonClicked();
        anim.SetBool("button", true);
    }
    public void Activate()
    {
        isActive = true;
    }
    private void LateUpdate()
    {
        anim.SetBool("button", isActive);
    }
    public void Reset()
    {
        isActive = false;
    }
}

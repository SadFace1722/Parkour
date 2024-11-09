using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffAcid : MonoBehaviour, PlayerInterface
{
    Animator anim;
    public static TurnOffAcid Instance;
    public bool turnOff;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void Start()
    {
        anim = transform.GetChild(0).GetComponent<Animator>();
    }
    private void LateUpdate()
    {
        anim.SetBool("Acid", turnOff);
    }
    public void Interact()
    {
        turnOff = !turnOff;
    }
}

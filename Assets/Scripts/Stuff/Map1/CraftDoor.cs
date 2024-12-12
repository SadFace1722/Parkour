using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftDoor : MonoBehaviour, PlayerInterface
{
    public static CraftDoor Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    Animator anim;
    public bool OpenDoor;
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        anim.SetBool("Door", OpenDoor);
        if (OpenDoor)
        {
            SoundManager.Instance.PlaySoundAtPosition(SoundManager.Instance.DoorOpen, transform.position);
        }
        else
        {
            SoundManager.Instance.PlaySoundAtPosition(SoundManager.Instance.DoorClose, transform.position);
        }
    }

    public void Interact()
    {
        if (nv2.Instance.generator && !OpenDoor)
        {
            OpenDoor = true;
        }
    }
}

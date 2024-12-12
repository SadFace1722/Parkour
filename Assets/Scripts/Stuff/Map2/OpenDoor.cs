using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour, PlayerInterface
{
    public static OpenDoor Instance;
    Animator anim;
    public bool IsOpenDoor;
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
    private void Update()
    {
        anim.SetBool("Door", IsOpenDoor);
    }
    public void Interact()
    {
        if (TurnOffAcid.Instance.turnOff && !IsOpenDoor)
        {
            IsOpenDoor = true;
        }
    }
    public void OpenDoorSound()
    {
        SoundManager.Instance.PlaySoundAtPosition(SoundManager.Instance.DoorOpen, transform.position);
    }
    public void CloseDoorSound()
    {
        SoundManager.Instance.PlaySoundAtPosition(SoundManager.Instance.DoorClose, transform.position);
    }
}

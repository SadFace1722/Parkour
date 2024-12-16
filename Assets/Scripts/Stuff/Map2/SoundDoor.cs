using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundDoor : MonoBehaviour
{
    public void OpenDoorSound()
    {
        SoundManager.Instance.PlaySoundAtPosition(SoundManager.Instance.DoorOpen, transform.position);
    }
    public void CloseDoorSound()
    {
        SoundManager.Instance.PlaySoundAtPosition(SoundManager.Instance.DoorClose, transform.position);
    }
}

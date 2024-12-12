using UnityEngine;

public class Door : MonoBehaviour, PlayerInterface
{
    public Animator anim;

    public void Interact()
    {
        if (Key.Instance.hasKey)
        {
            anim.SetBool("Open", true);
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

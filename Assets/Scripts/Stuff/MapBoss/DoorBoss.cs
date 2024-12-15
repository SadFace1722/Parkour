using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBoss : MonoBehaviour
{
    Animator anim;
    bool isLock;
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isLock)
        {
            isLock = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (isLock)
        {
            anim.SetBool("Door", false);
        }
        else
        {
            anim.SetBool("Door", true);
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

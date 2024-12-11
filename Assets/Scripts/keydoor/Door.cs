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
}

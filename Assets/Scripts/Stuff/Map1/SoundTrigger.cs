using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.Water);
        }
    }
}

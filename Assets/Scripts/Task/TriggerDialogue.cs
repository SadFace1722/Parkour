using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialogue : MonoBehaviour
{
    public int FirstIndex, LastIndex;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            DialogueManager.instance.StartDialogue(FirstIndex, LastIndex);
            gameObject.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PadNote : MonoBehaviour,PlayerInterface
{
    [SerializeField] private GameObject NoteCanvas; // Canvas nhập mật khẩu
    bool isActive;
    public void Interact()
    {
        isActive = !isActive;

    }
    void Update()
    {
        NoteCanvas.SetActive(isActive);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PadBTHH : MonoBehaviour,PlayerInterface
{
    [SerializeField] private GameObject BTHHHCanvas; // Canvas nhập mật khẩu
    bool isActive;
    public void Interact()
    {
        isActive = !isActive;

    }
    void Update()
    {
        BTHHHCanvas.SetActive(isActive);
    }
}

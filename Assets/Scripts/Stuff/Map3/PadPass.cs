using System.Collections;
using UnityEngine;

public class PadInteraction : MonoBehaviour, PlayerInterface
{
    [SerializeField] private GameObject PassWordCanvas; // Canvas nhập mật khẩu
    bool isActive;
    public void Interact()
    {
        isActive = !isActive;

    }
    void Update()
    {
        PassWordCanvas.SetActive(isActive);
    }
}
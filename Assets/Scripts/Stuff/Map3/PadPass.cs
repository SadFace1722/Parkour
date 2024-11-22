using System.Collections;
using UnityEngine;

public class PadInteraction : MonoBehaviour, PlayerInterface
{
    [SerializeField] private GameObject PassWordCanvas; // Canvas nhập mật khẩu
    [SerializeField] private CharacterController playerController;
    [SerializeField] private MonoBehaviour firstPersonLook;
    [SerializeField] private MonoBehaviour thirdPersonLook;
    private bool isActive;

    public void Interact()
    {
        isActive = !isActive;

        if (isActive)
        {
            if (playerController != null)
            {
                playerController.enabled = false; 
            }

            if (firstPersonLook != null)
            {
                firstPersonLook.enabled = false; 
            }

            if (thirdPersonLook != null)
            {
                thirdPersonLook.enabled = false; 
            }

            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        else
        {
            if (playerController != null)
            {
                playerController.enabled = true;
            }

            if (firstPersonLook != null)
            {
                firstPersonLook.enabled = true; 
            }

            if (thirdPersonLook != null)
            {
                thirdPersonLook.enabled = true; 
            }

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = false;
        }
    }

    void Update()
    {
        PassWordCanvas.SetActive(isActive);
    }
}

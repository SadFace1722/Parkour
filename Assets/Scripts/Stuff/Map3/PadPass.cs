using System.Collections;
using UnityEngine;

public class PadInteraction : MonoBehaviour, PlayerInterface
{
    [SerializeField] private GameObject PassWordCanvas; // Canvas nhập mật khẩu
    [SerializeField] private CharacterController playerController;
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

           
            Cursor.lockState = CursorLockMode.Confined; 
            Cursor.visible = true; 
        }
        else
        {
            if (playerController != null)
            {
                playerController.enabled = true;
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

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class KeyPad : MonoBehaviour
{
    [SerializeField] private Text Ans; 
    [SerializeField] private GameObject keyPadPanel; 
    private string Answer = "321844";
    private bool isPlayerNearby = false;

    private void Update()
    {
        
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            ToggleKeyPadPanel();
        }
    }

    // Hàm để mở/tắt KeyPad Panel
    private void ToggleKeyPadPanel()
    {
        keyPadPanel.SetActive(!keyPadPanel.activeSelf);
        Ans.text = ""; 
    }

    
    public void Number(int number)
    {
        if (Ans.text == "Nhập Sai!" || Ans.text == "Nhập Đúng!")
        {
            Ans.text = "";
        }
        Ans.text += number.ToString();
    }

    
    public void Execute()
    {
        if (Ans.text == Answer)
        {
            Ans.text = "Nhập Đúng!";
            Invoke("CloseKeyPad", 1f); 
        }
        else
        {
            Ans.text = "Nhập Sai!";
        }
    }

    // Hàm để xóa toàn bộ số đã nhập
    public void Clear()
    {
        Ans.text = "";
    }

    // Đóng KeyPad Panel
    private void CloseKeyPad()
    {
        keyPadPanel.SetActive(false);
    }

    // Khi người chơi đi vào phạm vi của ổ khóa
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
        }
    }

    // Khi người chơi rời khỏi phạm vi của ổ khóa
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }
}
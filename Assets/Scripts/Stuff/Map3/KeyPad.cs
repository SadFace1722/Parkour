using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class KeyPad : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Ans; // Text hiển thị số đã nhập




    private string Answer = "321844"; // Mã đúng để mở cửa



    public void Number(int number)
    {
        Ans.text += number.ToString();
    }

    public void Execute()
    {
        if (Ans.text == Answer)
        {
            Ans.text = "Nhập đúng";
            BunkerDoor.Instance.isOpen = true;

        }
        else
        {
            Ans.text = "Nhập Sai";
            BunkerDoor.Instance.isOpen = false;
        }
    }

    public void Delete()
    {
        Ans.text = "";
    }


}
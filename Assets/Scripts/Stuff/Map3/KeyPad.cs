using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class KeyPad : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Ans; // Text hiển thị số đã nhập
    [SerializeField] private Animator Door; // Animator điều khiển cửa


    private string Answer = "361844"; // Mã đúng để mở cửa



    public void Number(int number)
    {
        Ans.text += number.ToString();
    }

    public void Execute()
    {
        if (Ans.text == Answer)
        {
            Ans.text = "Nhập đúng";
            Door.SetBool("Open", true);
            StartCoroutine("StopDoor");
        }
        else
        {
            Ans.text = "Nhập Sai";
        }
    }

    public void Delete()
    {
        Ans.text = "";
    }

    IEnumerator StopDoor()
    {
        yield return new WaitForSeconds(0.5f);
        Door.SetBool("Open", false);

    }
}
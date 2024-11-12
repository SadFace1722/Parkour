using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLaser : MonoBehaviour
{
    public static GameLaser Instance;
    private List<string> correctOrder = new List<string> { "Yellow", "Blue", "Red" }; // Thứ tự đúng cần nhấn
    public int currentStep = 0; // Theo dõi bước hiện tại của người chơi

    public GameObject[] lasers; // Mảng các laser cần vô hiệu hóa khi đúng thứ tự

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Kiểm tra xem người chơi nhấn đúng thứ tự không
    public bool CheckOrder(string color)
    {
        if (color == "Yellow" && currentStep == 0)
        {
            currentStep++; // Bước tiếp theo nếu đúng là Yellow
            Debug.Log("Yellow pressed correctly!");
            return true;
        }
        else if (color == "Blue" && currentStep == 1)
        {
            currentStep++; // Bước tiếp theo nếu đúng là Blue
            Debug.Log("Blue pressed correctly!");
            return true;
        }
        else if (color == "Red" && currentStep == 2)
        {
            currentStep++; // Bước tiếp theo nếu đúng là Red
            Debug.Log("Red pressed correctly!");
            DisableLasers(); // Nếu đúng hết, vô hiệu hóa laser
            return true;
        }
        else
        {
            // Nếu người chơi nhấn sai thứ tự, reset lại
            ResetOrder();
            Debug.Log("Wrong order! Resetting.");
            return false;
        }
    }

    // Reset thứ tự nếu sai
    public void ResetOrder()
    {
        currentStep = 0;
        Debug.Log("Order Reset.");
    }

    // Vô hiệu hóa tất cả các laser
    private void DisableLasers()
    {
        foreach (GameObject laser in lasers)
        {
            laser.SetActive(false);
        }
        Debug.Log("Lasers Disabled!"); // Thông báo đã vô hiệu hóa laser
    }
}

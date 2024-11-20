using System.Collections.Generic;
using UnityEngine;

public class GameLaser : MonoBehaviour
{
    public static GameLaser Instance;
    private List<string> correctOrder = new List<string> { "Yellow", "Blue", "Red" }; // Thứ tự đúng
    public int currentStep = 0; // Theo dõi bước hiện tại
    public bool isTurnOff; // Kiểm soát việc tắt laser
    public bool isGameOver = false; // Kiểm soát trạng thái sai thứ tự
    public GameObject[] lasers; // Mảng các laser

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Kiểm tra thứ tự
    public bool CheckOrder(string color)
    {
        if (isGameOver) return false; // Không kiểm tra nếu đã sai thứ tự

        if (color == "Yellow" && currentStep == 0)
        {
            currentStep++;
            Debug.Log("Yellow pressed correctly!");
            return true;
        }
        else if (color == "Blue" && currentStep == 1)
        {
            currentStep++;
            Debug.Log("Blue pressed correctly!");
            return true;
        }
        else if (color == "Red" && currentStep == 2)
        {
            currentStep++;
            Debug.Log("Red pressed correctly!");
            isTurnOff = true;
            DisableLasers(); // Vô hiệu hóa laser khi hoàn tất
            return true;
        }
        else
        {
            // Sai thứ tự, kích hoạt trạng thái game kết thúc
            isGameOver = true;
            Debug.Log("Wrong order! Game Over.");
            return false;
        }
    }

    // Reset thứ tự
    public void ResetOrder()
    {
        isTurnOff = false;
        isGameOver = false; // Reset trạng thái game kết thúc
        currentStep = 0;
        Debug.Log("Order Reset.");
    }

    // Vô hiệu hóa laser
    private void DisableLasers()
    {
        foreach (GameObject laser in lasers)
        {
            laser.SetActive(false);
        }
        Debug.Log("Lasers Disabled!");
    }
}

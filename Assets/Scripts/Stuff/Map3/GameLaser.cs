﻿using System.Collections;
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

    public GameObject[] cubes; // Các cube, dùng để vô hiệu hóa chúng

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

        // Kiểm tra thứ tự nhấn theo màu
        if (color == correctOrder[currentStep])
        {
            currentStep++;
            Debug.Log(color + " pressed correctly!");

            // Kiểm tra nếu đã nhấn đủ 3 màu đúng thứ tự
            if (currentStep == correctOrder.Count)
            {
                isTurnOff = true;
                DisableLasers(); // Tắt laser khi đúng thứ tự
            }
            return true;
        }
        else
        {
            // Sai thứ tự, kích hoạt trạng thái game kết thúc
            //isGameOver = true;
            //DisableCubes(); // Vô hiệu hóa tất cả các nút sau khi sai
            //Debug.Log("Wrong order! Game Over.");
            return false;
        }
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

    // Vô hiệu hóa các nút (cubes) nhưng không làm ảnh hưởng đến animation
    private void DisableCubes()
    {
        foreach (GameObject cube in cubes)
        {
            // Vô hiệu hóa collider của các nút để không thể tương tác
            cube.GetComponent<Collider>().enabled = false;
        }
    }

    // Reset thứ tự
    public void ResetOrder()
    {
        isTurnOff = false;
        isGameOver = false; // Reset trạng thái game kết thúc
        currentStep = 0;
        Debug.Log("Order Reset.");

        // Bật lại các nút
        EnableCubes();
    }

    // Kích hoạt lại các nút khi reset
    private void EnableCubes()
    {
        foreach (GameObject cube in cubes)
        {
            cube.GetComponent<Collider>().enabled = true;
        }
    }
}

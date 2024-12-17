using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLaser : MonoBehaviour
{
    public static GameLaser Instance;
    public GameObject Lazer;
    public bool isComplete;
    private int currentStep = 0;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void OnYellowButtonClicked()
    {
        if (currentStep == 0)
        {
            CubeYellow.Instance.Activate();
            currentStep++;
            CheckCompletion();
        }
        else
        {
            ResetGame();
        }
    }

    public void OnBlueBlueButtonClicked()
    {
        if (currentStep == 1)
        {
            CubeBlue.Instance.Activate();
            currentStep++;
            CheckCompletion();
        }
        else
        {
            ResetGame();
        }
    }

    public void OnRedButtonClicked()
    {
        if (currentStep == 2)
        {
            CubeRed.Instance.Activate();
            currentStep++;
            CheckCompletion();
        }
        else
        {
            ResetGame();
        }
    }

    private void CheckCompletion()
    {
        if (CubeYellow.Instance.isActive && CubeBlue.Instance.isActive && CubeRed.Instance.isActive)
        {
            isComplete = true;
            Lazer.SetActive(false);
        }
    }

    private void ResetGame()
    {
        CubeYellow.Instance.Reset();
        CubeBlue.Instance.Reset();
        CubeRed.Instance.Reset();
        currentStep = 0;
    }
}

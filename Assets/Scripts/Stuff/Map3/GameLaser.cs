using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class GameLaser : MonoBehaviour
{

    public static GameLaser Instance;
    public GameObject Lazer;
    public bool isComplete;
    public bool isTrue;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Update()
    {
        if (CubeYellow.Instance.Active)
        {
            if (CubeBlue.Instance.Active)
            {
                if (CubeRed.Instance.Active)
                {
                    Lazer.gameObject.SetActive(false);
                    isComplete = true;
                }
            }
        }
        else
        {
            CubeBlue.Instance.Active = false;
            CubeYellow.Instance.Active = false;
            CubeRed.Instance.Active = false;
        }
    }
}

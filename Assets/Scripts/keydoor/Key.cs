using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, PlayerInterface
{
    public static Key Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public bool hasKey;
    public void Interact()
    {
        hasKey = true;
        gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        Debug.Log("Has Key");
    }
}

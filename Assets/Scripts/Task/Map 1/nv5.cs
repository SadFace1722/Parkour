using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nv5 : MonoBehaviour
{
    public static nv5 Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public bool KiemTraXungQuanh = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!KiemTraXungQuanh)
            {
                KiemTraXungQuanh = true;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nv1 : MonoBehaviour
{
    public static nv1 Instance;
    public bool seeAroundArea;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!seeAroundArea)
            {
                seeAroundArea = true;
                Destroy(gameObject);
            }
        }
    }
}

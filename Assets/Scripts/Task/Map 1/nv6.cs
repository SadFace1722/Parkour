using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nv6 : MonoBehaviour
{
    public static nv6 Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public bool BangQuaAcid = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!BangQuaAcid)
            {
                BangQuaAcid = true;
            }
        }
    }
}

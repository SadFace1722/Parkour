using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyTank : MonoBehaviour, PlayerInterface
{
    public void Interact()
    {
        Debug.Log("EnergyTank: Interact được kích hoạt!");
        nv2.Instance.EnergyTank++;
        Debug.Log("EnergyTank count in nv2: " + nv2.Instance.EnergyTank);
        gameObject.SetActive(false);
    }
}



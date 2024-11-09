using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupGun : MonoBehaviour, PlayerInterface
{
    public static PickupGun Instance;
    public bool PickuptheGun;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public void Interact()
    {
        if (!PickuptheGun)
        {
            PickuptheGun = true;
            gameObject.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupAcid : MonoBehaviour
{
    private void Update()
    {
        if (TurnOffAcid.Instance.turnOff == true)
        {
            gameObject.SetActive(false);
        }
    }
}

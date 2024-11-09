using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nv3 : MonoBehaviour, PlayerInterface
{
    public static nv3 Instance;
    public bool isUseArmor;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public void Interact()
    {
        if (CraftDoor.Instance.OpenDoor && !isUseArmor)
        {
            isUseArmor = true;
            CutsceneManager.Instance.PlayCutscene(0);
        }
    }
}

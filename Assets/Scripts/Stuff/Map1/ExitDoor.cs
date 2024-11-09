using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour, PlayerInterface
{
    public static ExitDoor Instance;
    public bool DestroyDoor;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public void Interact()
    {
        if (CraftDoor.Instance.OpenDoor && !DestroyDoor)
        {
            DestroyDoor = true;
            CutsceneManager.Instance.PlayCutscene(1);
        }
    }
}
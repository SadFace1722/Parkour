using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillMonster : MonoBehaviour
{
    public static KillMonster Instance;
    public int KillCount;
    public bool CompleteKillMonster;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public void Interact()
    {
        if (PickupGun.Instance.PickuptheGun && KillCount >= 5 && !CompleteKillMonster)
        {
            CompleteKillMonster = true;
            CutsceneManager.Instance.PlayCutscene(1);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroGame : MonoBehaviour
{
    void Start()
    {
        CutsceneManager.Instance.PlayCutscene(0);
    }
}

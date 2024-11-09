using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMap : MonoBehaviour
{
    private void Start()
    {
        SceneManager.LoadScene("GamePlayer");
    }
}

using UnityEngine;
using UnityEngine.Playables;
using System.Collections.Generic;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager Instance;
    public List<PlayableDirector> cutscenes;
    private int currentCutsceneIndex = -1;
    public bool isCutscenePlaying { get; private set; } = false; // Biến để theo dõi trạng thái cutscene

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        foreach (PlayableDirector director in cutscenes)
        {
            director.gameObject.SetActive(false);
        }
    }

    public void PlayCutscene(int index)
    {
        if (index < 0 || index >= cutscenes.Count)
        {
            Debug.LogError("Index không hợp lệ");
            return;
        }

        if (currentCutsceneIndex != -1 && cutscenes[currentCutsceneIndex].state == PlayState.Playing)
        {
            cutscenes[currentCutsceneIndex].Stop();
        }

        currentCutsceneIndex = index;
        cutscenes[currentCutsceneIndex].gameObject.SetActive(true);
        cutscenes[currentCutsceneIndex].Play();

        isCutscenePlaying = true; // Đặt biến thành true khi cutscene bắt đầu chạy
        Debug.Log("Đang chạy cutscene thứ: " + index);
    }

    void Update()
    {
        if (currentCutsceneIndex != -1)
        {
            PlayableDirector director = cutscenes[currentCutsceneIndex];
            if (director.state != PlayState.Playing)
            {
                Debug.Log("Cutscene đã kết thúc: " + currentCutsceneIndex);

                director.gameObject.SetActive(false);
                currentCutsceneIndex = -1;
                isCutscenePlaying = false; // Đặt biến thành false khi cutscene kết thúc
            }
        }
    }
}

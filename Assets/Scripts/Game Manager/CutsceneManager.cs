using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager Instance;
    public List<PlayableDirector> cutscenes;
    private int currentCutsceneIndex = -1;
    public bool isCutscenePlaying { get; private set; } = false;

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

        isCutscenePlaying = true;
        Debug.Log("Đang chạy cutscene thứ: " + index);
    }

    public void PlayTwoCutscenesSequentially(int index1, int index2)
    {
        StartCoroutine(PlayCutscenesCoroutine(index1, index2, null));
    }

    public void PlayTwoCutscenesAndLoadScene(int index1, int index2, string sceneName)
    {
        StartCoroutine(PlayCutscenesCoroutine(index1, index2, sceneName));
    }

    private IEnumerator<WaitForSeconds> PlayCutscenesCoroutine(int index1, int index2, string sceneName)
    {
        if (index1 < 0 || index1 >= cutscenes.Count || index2 < 0 || index2 >= cutscenes.Count)
        {
            Debug.LogError("Index không hợp lệ");
            yield break;
        }

        // Chạy cutscene 1
        PlayCutscene(index1);
        yield return new WaitForSeconds((float)cutscenes[index1].duration);
        cutscenes[index1].gameObject.SetActive(false);

        // Chạy cutscene 2
        PlayCutscene(index2);
        yield return new WaitForSeconds((float)cutscenes[index2].duration);
        cutscenes[index2].gameObject.SetActive(false);

        // Load scene nếu có
        if (!string.IsNullOrEmpty(sceneName))
        {
            Debug.Log("Loading scene: " + sceneName);
            SceneManager.LoadScene(sceneName);
        }
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
                isCutscenePlaying = false;
            }
        }
    }
}

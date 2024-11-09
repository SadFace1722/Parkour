using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    public GameObject dialogueParent;
    public GameObject backgroundImage;
    public float readingSpeed = 0.05f;

    private TextMeshProUGUI[] dialogues;
    private int currentStartIndex;
    private int currentEndIndex;
    private Coroutine dialogueCoroutine;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        dialogues = dialogueParent.GetComponentsInChildren<TextMeshProUGUI>(true);
        DisableAllDialogues();
    }

    public void StartDialogue(int startIndex, int endIndex)
    {
        if (dialogueCoroutine != null)
        {
            StopCoroutine(dialogueCoroutine);
        }

        currentStartIndex = startIndex;
        currentEndIndex = endIndex;
        backgroundImage.SetActive(true);
        dialogueCoroutine = StartCoroutine(ShowDialogues());
    }

    private IEnumerator ShowDialogues()
    {
        DisableAllDialogues();

        for (int i = currentStartIndex; i <= currentEndIndex; i++)
        {
            dialogues[i].gameObject.SetActive(true);
            float displayTime = dialogues[i].text.Length * readingSpeed;
            yield return new WaitForSeconds(displayTime);
            dialogues[i].gameObject.SetActive(false);
        }

        backgroundImage.SetActive(false);
    }

    private void DisableAllDialogues()
    {
        foreach (var dialogue in dialogues)
        {
            dialogue.gameObject.SetActive(false);
        }
    }
}

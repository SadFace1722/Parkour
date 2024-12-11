using System.Collections;
using UnityEngine;

public class PlayDialogue : MonoBehaviour
{
    public static PlayDialogue Instance;
    [SerializeField] GameObject[] AlienSke;
    public bool isSolve;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public int FirstIndex, LastIndex, WaitTime;

    private bool isDialogueStarted = false;

    private void Update()
    {
        if (PadBTHH.Instance.isCheck && PadNote.Instance.isCheck && !isDialogueStarted)
        {
            StartCoroutine(WaitAndPlayDialogue());
        }
    }

    private IEnumerator WaitAndPlayDialogue()
    {
        isDialogueStarted = true;
        yield return new WaitForSeconds(WaitTime);
        if (PadBTHH.Instance.isCheck && PadNote.Instance.isCheck)
        {
            isSolve = true;
            DialogueManager.instance.StartDialogue(FirstIndex, LastIndex);
            Debug.Log("Dialogue started after 2 seconds.");
            foreach (var G in AlienSke)
            {
                if (G != null)
                {
                    G.SetActive(true);
                }
            }
        }
    }
}

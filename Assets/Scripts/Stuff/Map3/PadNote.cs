using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PadNote : MonoBehaviour, PlayerInterface
{
    public static PadNote Instance;
    [SerializeField] private GameObject NoteCanvas;
    public int FirstIndex, LastIndex;
    public bool isCheck;
    private bool isActive = false;  // Trạng thái hiện tại của canvas
    private bool wasActive = false; // Trạng thái trước đó của canvas
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public void Interact()
    {
        // Bật/tắt canvas
        isActive = !isActive;
        NoteCanvas.SetActive(isActive);

        // Kiểm tra trạng thái
        if (!isActive && wasActive)
        {
            // Khi canvas đã được bật và sau đó tắt
            isCheck = true;
            DialogueManager.instance.StartDialogue(FirstIndex, LastIndex);
            Debug.Log("Canvas was enabled and then disabled. Logic executed.");
        }

        // Cập nhật trạng thái trước đó
        wasActive = isActive;

    }
}

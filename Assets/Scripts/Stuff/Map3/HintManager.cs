using UnityEngine;
using UnityEngine.UI;

public class HintManager : MonoBehaviour
{
    [Header("UI Components")]
    public Text hintText;        // Text UI để hiển thị gợi ý
    public Text instructionText; // Text UI để hiển thị hướng dẫn "Ấn F để xem gợi ý"

    [Header("Hint Configuration")]
    public string[] hints;       // Mảng chứa các gợi ý
    private bool isPlayerInZone = false; // Kiểm tra người chơi trong khu vực
    private bool isHintShown = false;    // Trạng thái gợi ý đã hiển thị chưa

    private void Start()
    {
        // Kiểm tra và khởi tạo gợi ý nếu cần
        if (hints == null || hints.Length == 0)
        {
            Debug.LogWarning("Hints array is empty or not initialized. Using default hints.");
            hints = new string[]
            {
                "The sun rises in the east.",
                "Water flows calmly in the river.",
                "Fire burns with passion."
            };
        }

        // Ẩn các Text UI ban đầu
        if (hintText != null)
        {
            hintText.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("HintText UI is not assigned in the Inspector!");
        }

        if (instructionText != null)
        {
            instructionText.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("InstructionText UI is not assigned in the Inspector!");
        }
    }

    private void Update()
    {
        // Nếu người chơi trong khu vực và nhấn phím F
        if (isPlayerInZone && Input.GetKeyDown(KeyCode.F) && !isHintShown)
        {
            ShowHints(); // Hiển thị tất cả gợi ý
        }
    }

    private void ShowHints()
    {
        isHintShown = true; // Đặt trạng thái gợi ý đã hiển thị
        hintText.gameObject.SetActive(true); // Hiển thị gợi ý
        instructionText.gameObject.SetActive(false); // Ẩn hướng dẫn

        // Kết hợp tất cả các gợi ý thành một chuỗi duy nhất
        string combinedHints = string.Join("\n", hints);
        hintText.text = combinedHints;

        // Ẩn gợi ý sau 5 giây
        StartCoroutine(HideHintAfterDelay(5f));
    }

    private System.Collections.IEnumerator HideHintAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Tắt text gợi ý và đặt lại trạng thái
        hintText.gameObject.SetActive(false);
        isHintShown = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true; // Xác định người chơi đã vào khu vực
            instructionText.gameObject.SetActive(true); // Hiển thị hướng dẫn
            instructionText.text = "Vui lòng ấn F để xem gợi ý";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false; // Người chơi rời khỏi khu vực
            instructionText.gameObject.SetActive(false); // Ẩn hướng dẫn
            hintText.gameObject.SetActive(false);       // Ẩn gợi ý (nếu đang hiển thị)
            isHintShown = false;
        }
    }
}

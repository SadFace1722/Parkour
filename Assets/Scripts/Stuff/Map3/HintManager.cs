using UnityEngine;
using UnityEngine.UI;

public class HintManager : MonoBehaviour
{
    public Text yellowText; // Tham chiếu đến Text "Yellow"
    public Text blueText;   // Tham chiếu đến Text "Blue"
    public Text redText;    // Tham chiếu đến Text "Red"
    public float hintDuration = 5f; // Thời gian hiển thị gợi ý
    private bool isPlayerInZone = false; // Kiểm tra xem người chơi có trong khu vực hay không
    private bool hasShownHint = false;   // Đảm bảo gợi ý chỉ hiển thị một lần khi nhấn F

    void Start()
    {
        // Đảm bảo các chữ bị tắt khi trò chơi bắt đầu
        HideAllHints();
    }

    void Update()
    {
        // Khi người chơi trong khu vực và nhấn phím F, hiển thị gợi ý
        if (isPlayerInZone && Input.GetKeyDown(KeyCode.F) && !hasShownHint)
        {
            ShowHint();
            hasShownHint = true; // Ngăn gợi ý hiển thị lại nếu đã nhấn
        }
    }

    void ShowHint()
    {
        // Hiển thị tất cả các chữ
        yellowText.gameObject.SetActive(true);
        blueText.gameObject.SetActive(true);
        redText.gameObject.SetActive(true);

        // Ẩn chữ sau thời gian đặt trước
        Invoke("HideAllHints", hintDuration);
    }

    void HideAllHints()
    {
        // Tắt tất cả các chữ
        yellowText.gameObject.SetActive(false);
        blueText.gameObject.SetActive(false);
        redText.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Kiểm tra nếu người chơi vào khu vực
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true; // Bật cờ
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Kiểm tra nếu người chơi rời khỏi khu vực
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false; // Tắt cờ
            hasShownHint = false;   // Cho phép hiển thị lại khi người chơi quay lại
            HideAllHints();         // Ẩn gợi ý ngay khi rời khu vực
        }
    }
}

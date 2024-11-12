using UnityEngine;
using TMPro;

public class StartPoint : MonoBehaviour
{
    public float timeLimit = 30f; // Thời gian tối đa
    public bool timerStarted = false; // Kiểm tra nếu thời gian đã bắt đầu
    public float timer = 0f; // Thời gian đếm ngược

    public TextMeshProUGUI timerText; // Tham chiếu đến UI TextPro
    public GameObject timerTextObject; // Đối tượng chứa UI TextPro

    void Start()
    {
        // Ẩn Text khi bắt đầu
        if (timerTextObject != null)
        {
            timerTextObject.SetActive(false);
        }
    }

    void Update()
    {
        // Chỉ đếm thời gian khi đã bắt đầu
        if (timerStarted)
        {
            // Đếm ngược thời gian
            timer += Time.deltaTime;

            // Hiển thị thời gian lên UI
            if (timerText != null)
            {
                timerText.text = "Time Remaining: " + Mathf.Max(0, (timeLimit - timer)).ToString("F2") + "s";
            }

            // Nếu hết thời gian, log dead và dừng
            if (timer >= timeLimit)
            {
                Debug.Log("Player Dead"); // Log khi hết thời gian

                // Tắt UI Text khi hết thời gian
                if (timerTextObject != null)
                {
                    timerTextObject.SetActive(false);
                }

                timerStarted = false; // Dừng kiểm tra khi hết thời gian
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Nếu va chạm với player và chưa bắt đầu thời gian
        if (other.CompareTag("Player") && !timerStarted)
        {
            timerStarted = true;
            timer = 0f; // Reset lại thời gian
            Debug.Log("Timer started. You have 30 seconds to reach the end!");

            // Bật UI Text khi Player va chạm
            if (timerTextObject != null)
            {
                timerTextObject.SetActive(true);
            }
        }
    }
}

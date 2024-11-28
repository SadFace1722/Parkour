using UnityEngine;
using TMPro;
using System.Collections;

public class StartPoint : MonoBehaviour
{
    public float timeLimit = 30f; // Thời gian tối đa
    public bool timerStarted = false; // Kiểm tra nếu thời gian đã bắt đầu
    public float timer = 0f; // Thời gian đếm ngược

    public TextMeshProUGUI timerText; // Tham chiếu đến UI TextPro
    public GameObject timerTextObject; // Đối tượng chứa UI TextPro

    public float damageAmount = 0.2f;  // Số lượng sát thương mỗi lần
    public float damageInterval = 1f; // Thời gian giữa các lần gây sát thương (đơn vị: giây)
    private Coroutine damageCoroutine;  // Coroutine dùng để gây sát thương liên tục
    private bool hasTouchedEndPoint = false;  // Kiểm tra xem Player đã chạm vào EndPoint chưa

    void Start()
    {
        // Đảm bảo Text bị ẩn khi bắt đầu
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
                timerText.text = "Thời gian chạy: " + Mathf.Max(0, (timeLimit - timer)).ToString("F2");
            }

            // Nếu hết thời gian và chưa chạm vào EndPoint, bắt đầu gây sát thương
            if (timer >= timeLimit && !hasTouchedEndPoint)
            {
                if (damageCoroutine == null)
                {
                    damageCoroutine = StartCoroutine(ApplyContinuousDamage());
                }
            }

            // Nếu hết thời gian, dừng
            if (timer >= timeLimit)
            {
                // Dừng gây sát thương nếu thời gian hết
                if (damageCoroutine != null)
                {
                    StopCoroutine(damageCoroutine);
                    damageCoroutine = null;
                }

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
                timerTextObject.SetActive(true); // Hiển thị Text trong Hierarchy
            }
        }

        // Nếu va chạm với EndPoint, đánh dấu là đã chạm vào EndPoint
        if (other.CompareTag("EndPoint"))
        {
            hasTouchedEndPoint = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Nếu player rời khỏi khu vực
        if (other.CompareTag("Player"))
        {
            // Dừng việc gây sát thương khi player ra khỏi trigger
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }

        // Nếu player rời khỏi EndPoint, đánh dấu là chưa chạm vào EndPoint
        if (other.CompareTag("EndPoint"))
        {
            hasTouchedEndPoint = false;
        }
    }

    // Coroutine gây sát thương liên tục
    private IEnumerator ApplyContinuousDamage()
    {
        while (true)  // Tiếp tục gây sát thương liên tục
        {
            // Kiểm tra nếu player chưa chạm vào EndPoint
            if (!hasTouchedEndPoint)
            {
                // Gây sát thương cho player
                PlayerController player = FindObjectOfType<PlayerController>(); // Lấy Player trong scene
                if (player != null)
                {
                    player.TakeDamage(damageAmount);
                    Debug.Log("Player Health: " + player.curHealth);
                }
            }

            // Chờ trước khi gây sát thương lần tiếp theo
            yield return new WaitForSeconds(damageInterval);
        }
    }

    // Hàm để dừng hẳn thời gian
    public void StopTimer()
    {
        timerStarted = false;

        // Tắt UI Text khi dừng hẳn thời gian
        if (timerTextObject != null)
        {
            timerTextObject.SetActive(false);
        }
    }
}

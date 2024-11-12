using UnityEngine;

public class EndPoint : MonoBehaviour
{
    private StartPoint startPoint;

    void Start()
    {
        // Tìm StartPoint trong scene
        startPoint = FindObjectOfType<StartPoint>();
    }

    void OnTriggerEnter(Collider other)
    {
        // Nếu player va chạm với EndPoint, kiểm tra thời gian và tắt UI Text
        if (other.CompareTag("Player"))
        {
            if (startPoint != null)
            {
                if (startPoint.timer < startPoint.timeLimit)
                {
                    Debug.Log("You made it in time!");
                }

                // Tắt UI Text khi Player chạm vào EndPoint
                if (startPoint.timerTextObject != null)
                {
                    startPoint.timerTextObject.SetActive(false); // Tắt UI Text
                }
            }
        }
    }
}

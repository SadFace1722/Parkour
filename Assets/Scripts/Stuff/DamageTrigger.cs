using System.Collections;
using UnityEngine;

public class DamageTrigger : MonoBehaviour
{
    public float damageAmount = 0.2f;  // Số lượng sát thương mỗi lần
    public float damageInterval = 1f; // Thời gian giữa các lần gây sát thương (đơn vị: giây)
    public bool damageOnlyOnce = false;  // Chỉ gây sát thương một lần

    private Coroutine damageCoroutine;  // Coroutine dùng để gây sát thương liên tục

    private void OnTriggerEnter(Collider other)
    {
        // Kiểm tra nếu đối tượng là Player
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                // Bắt đầu gây sát thương liên tục nếu player bị vào vùng acid
                if (damageCoroutine == null) // Kiểm tra xem đã có coroutine chưa
                {
                    damageCoroutine = StartCoroutine(ApplyContinuousDamage(player));
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Kiểm tra nếu player ra khỏi vùng trigger
        if (other.CompareTag("Player"))
        {
            if (damageCoroutine != null)
            {
                // Dừng việc gây sát thương khi player rời khỏi trigger
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }

    // Coroutine gây sát thương liên tục
    private IEnumerator ApplyContinuousDamage(PlayerController player)
    {
        while (true)  // Tiếp tục gây sát thương cho đến khi ngừng
        {
            player.TakeDamage(damageAmount);  // Gây sát thương cho player
            Debug.Log("Player Health: " + player.currentHealth);

            // Chờ trước khi gây sát thương lần tiếp theo
            yield return new WaitForSeconds(damageInterval);
        }
    }
}

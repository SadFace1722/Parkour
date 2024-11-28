using System.Collections;
using UnityEngine;

public class DamageTrigger : MonoBehaviour
{
    public float damageAmount = 0.2f;    // Lượng sát thương mỗi lần
    public float damageInterval = 1f;    // Thời gian giữa các lần gây sát thương (giây)
    private Coroutine damageCoroutine;   // Coroutine gây sát thương liên tục
    private Collider triggerCollider;    // Collider của vùng trigger

    private void Start()
    {
        triggerCollider = GetComponent<Collider>(); // Lấy Collider của vùng trigger
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null && damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(ApplyContinuousDamage(player));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine); // Dừng sát thương khi player rời vùng
            damageCoroutine = null;
        }
    }

    // Coroutine gây sát thương liên tục
    private IEnumerator ApplyContinuousDamage(PlayerController player)
    {
        while (true)  // Kiểm tra player có còn trong vùng trigger mỗi lần
        {
            // Kiểm tra nếu player vẫn ở trong phạm vi của collider
            if (player == null || !triggerCollider.bounds.Contains(player.transform.position))
            {
                break; // Thoát khỏi vòng lặp nếu player ra khỏi vùng
            }

            // Gây sát thương
            player.TakeDamage(damageAmount);
            Debug.Log("Player Health: " + player.curHealth);

            yield return new WaitForSeconds(damageInterval);
        }

        // Dừng coroutine khi player ra khỏi vùng trigger
        damageCoroutine = null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerTrigger : MonoBehaviour
{
    public float damageAmount = 0.2f;    // Lượng sát thương mỗi lần
    public float damageInterval = 1f;    // Thời gian giữa các lần gây sát thương (giây)
    private float nextDamageTime = 0f;   // Thời điểm có thể gây sát thương tiếp theo

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null && Time.time >= nextDamageTime)
            {
                SoundManager.Instance.PlaySoundAtPosition(SoundManager.Instance.Laser, transform.position);
                player.TakeDamage(damageAmount);
                Debug.Log("Player Health: " + player.curHealth);
                nextDamageTime = Time.time + damageInterval; // Cập nhật thời gian cho lần sát thương tiếp theo
            }
        }
    }
}

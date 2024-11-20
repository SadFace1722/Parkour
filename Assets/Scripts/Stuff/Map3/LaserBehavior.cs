using UnityEngine;

public class LaserBehavior : MonoBehaviour
{
    public float damageAmount = 10f; // Số sát thương laser gây ra mỗi lần chạm

    private void OnTriggerEnter(Collider other)
    {
        // Kiểm tra xem đối tượng va chạm có phải là người chơi không
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damageAmount); // Gây sát thương lên người chơi
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Tiếp tục gây sát thương nếu người chơi vẫn đứng trong vùng laser
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damageAmount * Time.deltaTime); // Gây sát thương liên tục
            }
        }
    }
}

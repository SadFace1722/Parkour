using UnityEngine;

public class LuuViTri : MonoBehaviour
{
    public API api; // Tham chiếu đến API
    private bool isTriggered = false; // Để đảm bảo chỉ lưu một lần khi người chơi đi qua

    private void Start()
    {
        // Gán giá trị cho api
        api = FindObjectOfType<API>();
        if (api == null)
        {
            Debug.LogError("Không tìm thấy đối tượng API!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Kiểm tra xem đối tượng đi qua có phải là người chơi không
        if (other.CompareTag("Player") && !isTriggered)
        {
            isTriggered = true; // Đánh dấu rằng checkpoint đã được kích hoạt

            // Lưu vị trí hiện tại của người chơi
            Vector3 currentPosition = other.transform.position;

            if (api != null) // Kiểm tra xem api có được khởi tạo không
            {
                StartCoroutine(api.SaveCheckpoint(currentPosition));
                Debug.Log("Checkpoint đã được lưu tại vị trí: " + currentPosition);
            }
            else
            {
                Debug.LogError("API không được khởi tạo. Không thể lưu checkpoint.");
            }
        }
    }
}
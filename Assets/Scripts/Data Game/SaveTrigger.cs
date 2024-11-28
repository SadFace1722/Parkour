using UnityEngine;

public class SaveTrigger : MonoBehaviour
{
    public int savePointID; // Mã số save point để lưu trạng thái

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Lấy dữ liệu từ PlayerController
            Vector3 playerPosition = other.transform.position;
            float playerHealth = PlayerController.Instance.curHealth;

            // Lưu vị trí của người chơi vào PlayerController
            PlayerController.Instance.SavePosition(playerPosition);

            // Lưu sức khỏe của người chơi vào PlayerPrefs (hoặc bạn có thể lưu thêm thông tin khác nếu cần)
            PlayerPrefs.SetInt("SavePointID", savePointID);
            PlayerPrefs.SetFloat("PlayerHealth", playerHealth);
            PlayerPrefs.SetFloat("PlayerPosX", playerPosition.x);
            PlayerPrefs.SetFloat("PlayerPosY", playerPosition.y);
            PlayerPrefs.SetFloat("PlayerPosZ", playerPosition.z);
            PlayerPrefs.Save();

            // Thông báo đã lưu thành công
            Debug.Log($"Game Saved at Save Point: {savePointID} with Health: {playerHealth}, Position: {playerPosition}");
        }
    }
}

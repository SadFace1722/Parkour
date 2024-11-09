using UnityEngine;

public class SaveTrigger : MonoBehaviour
{
    public int savePointID; // Mã số save point để lưu trạng thái

    private void Update()
    {
        // Kiểm tra nếu người chơi nhấn phím P
        if (Input.GetKeyDown(KeyCode.P))
        {
            DeleteGameData();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Kiểm tra instance của PlayerController và TaskManager
            if (PlayerController.Instance != null && TaskManager.Instance != null)
            {
                // Lấy dữ liệu GameData từ GameManager (hoặc có thể tạo mới nếu cần thiết)
                GameData gameData = new GameData();

                // Lấy vị trí, lượng máu, chỉ số nhiệm vụ, trạng thái giáp, trạng thái súng từ các lớp khác
                Vector3 playerPosition = other.transform.position;
                float playerHealth = PlayerController.Instance.currentHealth;
                int currentTaskIndex = TaskManager.Instance.CurrentIndex; // Chỉ số nhiệm vụ hiện tại
                bool isArmorEquipped = GameManager.Instance.isArmorEquipped;  // Trạng thái giáp
                bool hasGun = GameManager.Instance.hasGun;  // Trạng thái súng

                // Lưu tất cả dữ liệu vào GameData
                gameData.SaveGame(savePointID, playerPosition, playerHealth, currentTaskIndex, isArmorEquipped, hasGun);

                // Thông báo đã lưu thành công
                Debug.Log($"Game Saved at Save Point: {savePointID} with Health: {playerHealth}, Task Index: {currentTaskIndex}, Armor Equipped: {isArmorEquipped}, Has Gun: {hasGun}");
            }
            else
            {
                Debug.LogError("PlayerController or TaskManager instance is missing.");
            }
        }
    }

    private void DeleteGameData()
    {
        // Xóa dữ liệu trong PlayerPrefs
        PlayerPrefs.DeleteAll();
        Debug.Log("Game data deleted from PlayerPrefs");

        // Nếu bạn sử dụng hệ thống lưu trữ khác ngoài PlayerPrefs, hãy xóa dữ liệu ở đó.
        // Ví dụ: Xóa dữ liệu trong GameManager
        GameManager.Instance.isArmorEquipped = false;
        GameManager.Instance.hasGun = false;

        // Nếu có thể reset thêm dữ liệu khác trong game như điểm nhiệm vụ, sức khỏe, vị trí, v.v.
        // GameManager.Instance.ResetGameData();

        // Thông báo cho người chơi rằng dữ liệu đã bị xóa
        Debug.Log("Game data has been reset.");
    }
}

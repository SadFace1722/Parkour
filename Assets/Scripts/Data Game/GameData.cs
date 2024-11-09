using UnityEngine;

public class GameData
{
    // Lưu tiến độ game bao gồm giáp và súng
    public void SaveGame(int savePointID, Vector3 playerPosition, float currentHealth, int currentTaskIndex, bool isArmorEquipped, bool hasGun)
    {
        PlayerPrefs.SetInt("SavePointID", savePointID);
        PlayerPrefs.SetFloat("PlayerPosX", playerPosition.x);
        PlayerPrefs.SetFloat("PlayerPosY", playerPosition.y);
        PlayerPrefs.SetFloat("PlayerPosZ", playerPosition.z);
        PlayerPrefs.SetFloat("CurrentHealth", currentHealth);
        PlayerPrefs.SetInt("CurrentTaskIndex", currentTaskIndex); // Lưu chỉ số nhiệm vụ
        
        // Lưu trạng thái giáp và súng
        PlayerPrefs.SetInt("IsArmorEquipped", isArmorEquipped ? 1 : 0); // 1 nếu giáp đã được trang bị
        PlayerPrefs.SetInt("HasGun", hasGun ? 1 : 0); // 1 nếu người chơi đã có súng

        PlayerPrefs.Save();
    }

    // Tải tiến độ game bao gồm giáp và súng
    public void LoadGame(out int savePointID, out Vector3 playerPosition, out float currentHealth, out int currentTaskIndex, out bool isArmorEquipped, out bool hasGun)
    {
        savePointID = PlayerPrefs.GetInt("SavePointID", 0); // Mặc định là 0 nếu không có
        float posX = PlayerPrefs.GetFloat("PlayerPosX", 58f);
        float posY = PlayerPrefs.GetFloat("PlayerPosY", 102f);
        float posZ = PlayerPrefs.GetFloat("PlayerPosZ", 128f);
        playerPosition = new Vector3(posX, posY, posZ);
        currentHealth = PlayerPrefs.GetFloat("CurrentHealth", 100f); // Mặc định là 100 nếu không có
        currentTaskIndex = PlayerPrefs.GetInt("CurrentTaskIndex", 0); // Mặc định là nhiệm vụ đầu tiên

        // Tải trạng thái giáp và súng
        isArmorEquipped = PlayerPrefs.GetInt("IsArmorEquipped", 0) == 1; // 1 nếu giáp đã trang bị, 0 nếu chưa
        hasGun = PlayerPrefs.GetInt("HasGun", 0) == 1; // 1 nếu có súng, 0 nếu không có
    }

    // Xóa dữ liệu game khỏi PlayerPrefs
    public void ResetGameData()
    {
        PlayerPrefs.DeleteKey("SavePointID");
        PlayerPrefs.DeleteKey("PlayerPosX");
        PlayerPrefs.DeleteKey("PlayerPosY");
        PlayerPrefs.DeleteKey("PlayerPosZ");
        PlayerPrefs.DeleteKey("CurrentHealth");
        PlayerPrefs.DeleteKey("CurrentTaskIndex"); // Xóa chỉ số nhiệm vụ
        PlayerPrefs.DeleteKey("IsArmorEquipped"); // Xóa trạng thái giáp
        PlayerPrefs.DeleteKey("HasGun"); // Xóa trạng thái súng

        PlayerPrefs.Save();
    }
}

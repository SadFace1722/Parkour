using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Transform playerTransform;
    public bool isArmorEquipped; // Trạng thái giáp
    public bool hasGun; // Trạng thái súng
    public int savePointID; // Định nghĩa biến savePointID
    private GameData gameData = new GameData();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        playerTransform = GameObject.Find("Player").transform;
    }

    private void Update()
    {
        // Kiểm tra lại trạng thái của Armor và Shotgun sau khi load game
        // Điều này đảm bảo rằng trạng thái giáp và súng luôn đúng trong suốt quá trình chơi
        UpdatePlayerEquipment();
    }

    // Cập nhật trạng thái giáp và súng trong quá trình chơi
    private void UpdatePlayerEquipment()
    {
        // Kiểm tra và khôi phục trạng thái Armor
        if (isArmorEquipped && !PlayerController.Instance.Armor.activeSelf)
        {
            PlayerController.Instance.Armor.SetActive(true);
            PlayerController.Instance.Player.SetActive(false);
        }
        else if (!isArmorEquipped && PlayerController.Instance.Armor.activeSelf)
        {
            PlayerController.Instance.Armor.SetActive(false); // Tắt Armor nếu không được trang bị
            PlayerController.Instance.Player.SetActive(true);
        }

        // Kiểm tra và khôi phục trạng thái Shotgun
        if (hasGun && !PlayerController.Instance.Shotgun.activeSelf)
        {
            PlayerController.Instance.Shotgun.SetActive(true); // Kích hoạt Shotgun nếu chưa được kích hoạt
        }
        else if (!hasGun && PlayerController.Instance.Shotgun.activeSelf)
        {
            PlayerController.Instance.Shotgun.SetActive(false); // Tắt Shotgun nếu không có súng
        }
    }

    public void SaveGame(int savePointID, Vector3 playerPosition, float currentHealth, int currentTaskIndex, bool isArmorEquipped, bool hasGun)
    {
        // Lưu thông tin vào PlayerPrefs hoặc hệ thống lưu trữ bạn đang sử dụng
        PlayerPrefs.SetInt("SavePointID", savePointID);
        PlayerPrefs.SetFloat("PlayerHealth", currentHealth);
        PlayerPrefs.SetFloat("PlayerPosX", playerPosition.x);
        PlayerPrefs.SetFloat("PlayerPosY", playerPosition.y);
        PlayerPrefs.SetFloat("PlayerPosZ", playerPosition.z);
        PlayerPrefs.SetInt("CurrentTaskIndex", currentTaskIndex);

        // Lưu trạng thái của Armor và Shotgun
        PlayerPrefs.SetInt("IsArmorEquipped", isArmorEquipped ? 1 : 0);
        PlayerPrefs.SetInt("HasGun", hasGun ? 1 : 0);

        PlayerPrefs.Save();
    }

    public void LoadGameProgress()
    {
        // Đọc dữ liệu đã lưu từ PlayerPrefs
        int savePointID = PlayerPrefs.GetInt("SavePointID", 0);
        float playerHealth = PlayerPrefs.GetFloat("PlayerHealth", 100);
        Vector3 playerPosition = new Vector3(
            PlayerPrefs.GetFloat("PlayerPosX", 0),
            PlayerPrefs.GetFloat("PlayerPosY", 0),
            PlayerPrefs.GetFloat("PlayerPosZ", 0)
        );
        int currentTaskIndex = PlayerPrefs.GetInt("CurrentTaskIndex", 0);

        // Đọc trạng thái Armor và Shotgun
        isArmorEquipped = PlayerPrefs.GetInt("IsArmorEquipped", 0) == 1;
        hasGun = PlayerPrefs.GetInt("HasGun", 0) == 1;

        // Khôi phục vị trí người chơi
        PlayerController.Instance.transform.position = playerPosition;
        PlayerController.Instance.currentHealth = playerHealth;

        // Đảm bảo rằng trạng thái Armor và Shotgun được cập nhật đúng
        UpdatePlayerEquipment();

        Debug.Log("Game Loaded from Save Point: " + savePointID);
    }
}

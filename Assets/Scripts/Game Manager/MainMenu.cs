using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuGame : MonoBehaviour
{
    public Button StartBtn, SettingBtn, ExitBtn;
    public Button BackToBaseBtn, ClearDataBtn;
    public GameObject BasePanel;
    public GameObject SettingPanel;

    void Start()
    {
        // Kiểm tra trạng thái dữ liệu khi game bắt đầu
        UpdateStartButtonText();

        // Các hành động cho các nút
        StartBtn.onClick.AddListener(OnStartButtonClick);
        SettingBtn.onClick.AddListener(ToggleSettings);
        ExitBtn.onClick.AddListener(ExitGame);
        BackToBaseBtn.onClick.AddListener(ToggleSettings);
        ClearDataBtn.onClick.AddListener(ClearGameData);

        ShowBasePanel();
    }

    // Hàm cập nhật lại trạng thái của nút Start dựa trên dữ liệu lưu
    void UpdateStartButtonText()
    {
        if (PlayerPrefs.HasKey("SavePointID"))  // Kiểm tra dữ liệu lưu
        {
            StartBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Tiếp tục";
        }
        else
        {
            StartBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Bắt đầu";
        }
    }

    void OnStartButtonClick()
    {
        if (PlayerPrefs.HasKey("SavePointID"))
        {
            SceneManager.LoadScene("GamePlayer"); // Tải Scene chơi game
        }
        else
        {
            SceneManager.LoadScene("IntroGame");  // Tải Scene intro
        }
    }

    void ToggleSettings()
    {
        bool isBaseActive = BasePanel.activeSelf;
        BasePanel.SetActive(!isBaseActive);
        SettingPanel.SetActive(isBaseActive);
    }

    void ExitGame()
    {
        Debug.Log("Thoát game");
        Application.Quit();
    }

    // Xóa tất cả dữ liệu lưu và cập nhật lại nút "Bắt đầu"
    void ClearGameData()
    {
        PlayerPrefs.DeleteAll();
        UpdateStartButtonText(); // Cập nhật lại nút Start sau khi xóa dữ liệu
        Debug.Log("Dữ liệu đã được xóa.");
    }

    void ShowBasePanel()
    {
        BasePanel.SetActive(true);
        SettingPanel.SetActive(false);
    }
}

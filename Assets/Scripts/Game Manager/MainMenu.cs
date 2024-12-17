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


    public AudioSource buttonAudioSource; // Nguồn âm thanh khi nhấn nút
    public AudioSource backgroundMusicSource; // Nguồn phát nhạc nền
    public AudioClip buttonClickSound; // Âm thanh khi nhấn nút
    public AudioClip backgroundMusic; // Nhạc nền

    void Start()
    { // Bắt đầu phát nhạc nền
        if (backgroundMusicSource != null && backgroundMusic != null)
        {
            backgroundMusicSource.clip = backgroundMusic;
            backgroundMusicSource.loop = true; // Lặp lại nhạc nền
            backgroundMusicSource.Play();
        }
        UpdateStartButtonText();

        // Các hành động cho các nút
        StartBtn.onClick.AddListener(() => { PlayButtonClickSound(); OnStartButtonClick(); });
        SettingBtn.onClick.AddListener(() => { PlayButtonClickSound(); ToggleSettings(); });
        ExitBtn.onClick.AddListener(() => { PlayButtonClickSound(); ExitGame(); });
        BackToBaseBtn.onClick.AddListener(() => { PlayButtonClickSound(); ToggleSettings(); });
        ClearDataBtn.onClick.AddListener(() => { PlayButtonClickSound(); ClearGameData(); });

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

    public void PlayButtonClickSound()
    {
        if (buttonAudioSource != null && buttonClickSound != null)
        {
            buttonAudioSource.PlayOneShot(buttonClickSound);
        }
    }
}

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
        StartBtn.onClick.AddListener(StartGame);
        SettingBtn.onClick.AddListener(ToggleSettings);
        ExitBtn.onClick.AddListener(ExitGame);

        BackToBaseBtn.onClick.AddListener(ToggleSettings);
        ClearDataBtn.onClick.AddListener(ClearGameData);

        ShowBasePanel();
    }

    void StartGame()
    {
        SceneManager.LoadScene("IntroGame");
    }

    void ToggleSettings()
    {
        if (BasePanel.activeSelf)
        {
            BasePanel.SetActive(false);
            SettingPanel.SetActive(true);
        }
        else
        {
            BasePanel.SetActive(true);
            SettingPanel.SetActive(false);
        }
    }

    void ExitGame()
    {
        Debug.Log("Thoát game");
        Application.Quit();
    }

    void ClearGameData()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("Dữ liệu đã được xóa.");
    }

    void ShowBasePanel()
    {
        BasePanel.SetActive(true);
        SettingPanel.SetActive(false);
    }
}

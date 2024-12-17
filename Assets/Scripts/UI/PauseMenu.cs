using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    public GameObject tutorialImage;
    public MonoBehaviour playerController;
    public MonoBehaviour firstPersonLook;
    private bool isPaused = false;
    private bool isTutorialActive = false;


    public AudioSource buttonAudioSource; 
    public AudioClip buttonClickSound;

    void Start()
    {
        pauseMenuPanel.SetActive(false); // Ẩn menu lúc đầu
        tutorialImage.SetActive(false); // Ẩn hình ảnh hướng dẫn lúc đầu
        HideCursor(); // Ẩn con trỏ chuột khi bắt đầu game
        Time.timeScale = 1; 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isTutorialActive)
            {
                CloseTutorial();
            }
            else
            {
                TogglePause();
            }
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        PlayButtonSound();

        pauseMenuPanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : 1; // Tạm dừng hoặc tiếp tục thời gian trong game
        playerController.enabled = !isPaused; // Vô hiệu hóa hoặc kích hoạt script điều khiển Player
        firstPersonLook.enabled = !isPaused; // Vô hiệu hóa hoặc kích hoạt script góc nhìn
        Cursor.visible = isPaused; // Hiển thị hoặc ẩn con trỏ chuột
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;

        if (isPaused)
        {
            DisableInput();
        }
        else
        {
            EnableInput();
        }
    }

    private void DisableInput()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Input.ResetInputAxes();
    }

    private void EnableInput()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    public void PlayGame()
    {
        PlayButtonSound();
        TogglePause();
    }

    public void GoToMainMenu()
    {
        PlayButtonSound();
        Time.timeScale = 1;
        TaskManager.Instance.SaveGame();
        PlayerController.Instance.SavePosition(PlayerController.Instance.transform.position);
        SceneManager.LoadScene("Menu");
    }

    public void Tutorial()
    {
        PlayButtonSound();
        isTutorialActive = true;
        tutorialImage.SetActive(true); // Hiển thị hình ảnh hướng dẫn
        pauseMenuPanel.SetActive(false); // Ẩn menu tạm dừng
        Time.timeScale = 0; // Dừng thời gian trong game
    }

    private void CloseTutorial()
    {
        PlayButtonSound();
        isTutorialActive = false;
        tutorialImage.SetActive(false); // Ẩn hình ảnh hướng dẫn
        pauseMenuPanel.SetActive(true); // Hiển thị lại menu tạm dừng
    }


    private void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void PlayButtonSound()
    {
        if (buttonAudioSource != null && buttonClickSound != null)
        {
            buttonAudioSource.PlayOneShot(buttonClickSound); // Phát âm thanh một lần
        }
    }
}

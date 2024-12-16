using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    public GameObject tutorialImage; 
    public MonoBehaviour playerController;
    public MonoBehaviour firstPersonLook;
    public AudioSource audioSource; 
    public AudioClip pauseSound;
    public AudioClip resumeSound; 
    public AudioClip tutorialSound; 
    public AudioClip mainMenuSound; 

    private bool isPaused = false;
    private bool isTutorialActive = false; 
    private AudioSource[] allAudioSources;

    void Start()
    {
        pauseMenuPanel.SetActive(false); // Ẩn menu lúc đầu
        tutorialImage.SetActive(false); // Ẩn hình ảnh hướng dẫn lúc đầu
        HideCursor(); // Ẩn con trỏ chuột khi bắt đầu game
        Time.timeScale = 1; // Đảm bảo thời gian game chạy bình thường khi bắt đầu
        allAudioSources = FindObjectsOfType<AudioSource>(); // Tìm tất cả các AudioSource trong scene
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

        // Phát âm thanh dựa trên trạng thái
        PlaySound(isPaused ? pauseSound : resumeSound);

        pauseMenuPanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : 1; // Tạm dừng hoặc tiếp tục thời gian trong game
        playerController.enabled = !isPaused; // Vô hiệu hóa hoặc kích hoạt script điều khiển Player
        firstPersonLook.enabled = !isPaused; // Vô hiệu hóa hoặc kích hoạt script góc nhìn
        Cursor.visible = isPaused; // Hiển thị hoặc ẩn con trỏ chuột
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;

        if (isPaused)
        {
            DisableInput();
            MuteAudio(true); // Tắt âm thanh khi game pause
        }
        else
        {
            EnableInput();
            MuteAudio(false); // Bật âm thanh khi game tiếp tục
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

    private void MuteAudio(bool mute)
    {
        foreach (AudioSource audioSource in allAudioSources)
        {
            audioSource.mute = mute;
        }
    }

    public void PlayGame()
    {
        TogglePause();
    }

    public void GoToMainMenu()
    {
        PlaySound(mainMenuSound); // Phát âm thanh trở về menu chính
        Time.timeScale = 1;
        TaskManager.Instance.SaveGame();
        PlayerController.Instance.SavePosition(PlayerController.Instance.transform.position);
        SceneManager.LoadScene("Menu");
    }

    public void Tutorial()
    {
        PlaySound(tutorialSound); // Phát âm thanh mở hướng dẫn
        isTutorialActive = true;
        tutorialImage.SetActive(true); // Hiển thị hình ảnh hướng dẫn
        pauseMenuPanel.SetActive(false); // Ẩn menu tạm dừng
        Time.timeScale = 0; // Dừng thời gian trong game
    }

    private void CloseTutorial()
    {
        PlaySound(tutorialSound); // Phát âm thanh đóng hướng dẫn
        isTutorialActive = false;
        tutorialImage.SetActive(false); // Ẩn hình ảnh hướng dẫn
        pauseMenuPanel.SetActive(true); // Hiển thị lại menu tạm dừng
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip); // Phát âm thanh một lần
        }
    }

    private void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}

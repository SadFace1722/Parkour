using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuPanel; // Kéo Panel menu vào đây trong Inspector
    public MonoBehaviour playerController; // Kéo script điều khiển Player vào đây
    public MonoBehaviour firstPersonLook; // Kéo script điều khiển góc nhìn thứ nhất vào đây
    private bool isPaused = false;
    private AudioSource[] allAudioSources; // Mảng lưu tất cả các AudioSource

    void Start()
    {
        pauseMenuPanel.SetActive(false); // Ẩn menu lúc đầu
        HideCursor(); // Ẩn con trỏ chuột khi bắt đầu game
        Time.timeScale = 1; // Đảm bảo thời gian game chạy bình thường khi bắt đầu
        allAudioSources = FindObjectsOfType<AudioSource>(); // Tìm tất cả các AudioSource trong scene
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        pauseMenuPanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : 1; // Tạm dừng hoặc tiếp tục thời gian trong game
        playerController.enabled = !isPaused; // Vô hiệu hóa hoặc kích hoạt script điều khiển Player
        firstPersonLook.enabled = !isPaused; // Vô hiệu hóa hoặc kích hoạt script góc nhìn
        Cursor.visible = isPaused; // Hiển thị hoặc ẩn con trỏ chuột
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;

        // Khóa tất cả các nút và bàn phím khi game pause
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

    // Vô hiệu hóa tất cả các đầu vào từ bàn phím và chuột khi game bị tạm dừng
    private void DisableInput()
    {
        // Khóa toàn bộ các phím điều khiển và ngừng các đầu vào khác
        Cursor.lockState = CursorLockMode.None; // Cho phép chuột tự do
        Cursor.visible = true; // Hiển thị chuột
        // Tắt sự kiện đầu vào bàn phím trong game (không cho di chuyển, không cho nhấn phím)
        Input.ResetInputAxes();
    }

    // Bật lại các đầu vào khi game tiếp tục
    private void EnableInput()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Tắt hoặc bật âm thanh trong game
    private void MuteAudio(bool mute)
    {
        foreach (AudioSource audioSource in allAudioSources)
        {
            audioSource.mute = mute; // Tắt hoặc bật âm thanh
        }
    }

    public void PlayGame()
    {
        TogglePause(); // Tiếp tục game
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1;
        TaskManager.Instance.SaveGame();
        PlayerController.Instance.SavePosition(PlayerController.Instance.transform.position);
        SceneManager.LoadScene("Menu"); // Load scene Menu chính
    }

    public void Restart()
    {
        // Có thể thực hiện lại thao tác restart ở đây nếu cần
    }

    private void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}

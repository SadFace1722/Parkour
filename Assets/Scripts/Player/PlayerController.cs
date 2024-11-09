using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; set; }
    [SerializeField] CharacterController controller;
    [SerializeField] Camera cam, fpsCam;
    [SerializeField] GameObject thirdPersonCamera, fpsCamera;
    Vector3 hor, ver, camF, camR;
    [SerializeField] float moveSpeed, jumpForce, gravity;
    public bool isGrounded, isFalling, isMoving, isJumping, canControll, isDead;
    public GameObject Player, Armor, Shotgun;
    public float currentHealth = 100;
    public Image HealImage, Crosshair;
    float x, z;

    public bool IsParkour = true;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

    }
    void Start()
    {
        controller = GetComponent<CharacterController>();
        canControll = true;
        ToggleCamera();
    }

    void Update()
    {
        if (Armor.activeSelf && HealImage != null)
        {
            HealImage.gameObject.SetActive(true);  // Hiển thị Health Image
        }
        else if (!Armor.activeSelf && HealImage != null)
        {
            HealImage.gameObject.SetActive(false); // Ẩn Health Image nếu Armor không được kích hoạt
        }

        // Update Health Bar smoothly
        if (HealImage != null)
        {
            HealImage.fillAmount = Mathf.Lerp(HealImage.fillAmount, currentHealth / 100f, Time.deltaTime * 5f);
        }

        if (currentHealth <= 0)
        {
            StartCoroutine(WaitBeforeRespawn(2f));
            isDead = true;
        }
        else
        {
            isDead = false;
        }

        canControll = currentHealth > 0;
        isGrounded = controller.isGrounded;

        if (!isGrounded && ver.y <= 0f && !isJumping)
        {
            isFalling = true;
        }
        else if (isGrounded)
        {
            isFalling = false;
            isJumping = false;
        }

        isMoving = isGrounded && hor.sqrMagnitude > 0.1f;

        // Kiểm tra xem cutscene có đang chạy không
        if (CutsceneManager.Instance.isCutscenePlaying)
        {
            canControll = false;
            ToggleCamera(); // Tắt camera trong khi cutscene
        }
        else
        {
            canControll = true;
            ToggleCamera(); // Bật camera theo `IsParkour` khi hết cutscene

            if (canControll&&!isDead)
            {
                HandleInput();
                HandleMove();

                if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
                {
                    HandleJump();
                }
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    IsParkour = !IsParkour;
                }
            }
        }

        HandleGravity();
    }

    void HandleInput()
    {
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");
    }

    void HandleMove()
    {
        if (IsParkour)
        {
            // Lấy hướng di chuyển từ Camera chính (Third-Person)
            camF = cam.transform.forward;
            camR = cam.transform.right;

            // Bỏ phần trục Y để chỉ lấy hướng ngang
            camF.y = 0;
            camR.y = 0;

            // Tính toán hướng di chuyển
            hor = (x * camR + z * camF).normalized;

            if (hor.sqrMagnitude > 0.1f)
            {
                // Xoay nhân vật về hướng di chuyển
                Quaternion Rota = Quaternion.LookRotation(hor);
                transform.rotation = Quaternion.Slerp(transform.rotation, Rota, moveSpeed * Time.deltaTime);

                // Di chuyển nhân vật theo hướng cam
                controller.Move(hor * moveSpeed * Time.deltaTime);
            }
        }
        else
        {
            // Lấy hướng di chuyển từ FPS Camera
            camF = fpsCam.transform.forward;
            camR = fpsCam.transform.right;

            // Bỏ phần trục Y để chỉ lấy hướng ngang
            camF.y = 0;
            camR.y = 0;

            // Tính toán hướng di chuyển
            hor = (x * camR + z * camF).normalized;

            if (hor.sqrMagnitude > .1f)
            {
                // Di chuyển nhân vật theo hướng cam FPS
                controller.Move(hor * moveSpeed * Time.deltaTime);
            }
        }
    }

    void HandleGravity()
    {
        if (isGrounded)
        {
            if (ver.y < 0)
            {
                ver.y = -2f;
            }
        }
        else
        {
            ver.y -= gravity * Time.deltaTime;
        }
        controller.Move(ver * Time.deltaTime);
    }

    void HandleJump()
    {
        if (canControll && isGrounded)
        {
            ver.y = jumpForce;
            isJumping = true;
            isFalling = false;
        }
    }



    public void TakeDamage(float amount)
    {
        currentHealth = Mathf.Max(0, currentHealth - amount);
    }

    public void TakeHeal(float amount)
    {
        currentHealth = Mathf.Min(100, currentHealth + amount);
    }

    private IEnumerator WaitBeforeRespawn(float delayTime)
    {
        yield return new WaitForSeconds(delayTime); // Đợi 1.5 giây
        GameManager.Instance.LoadGameProgress();
        currentHealth = PlayerController.Instance.currentHealth;  // Lấy currentHealth từ PlayerController
        transform.position = GameManager.Instance.playerTransform.position;
        Debug.Log("Player has respawned at save point.");
    }

    void ToggleCamera()
    {
        // Kiểm tra nếu đang trong cutscene thì tắt tất cả camera
        if (CutsceneManager.Instance.isCutscenePlaying)
        {
            fpsCamera.SetActive(false);
            thirdPersonCamera.SetActive(true);
            Crosshair.gameObject.SetActive(false);
        }
        else
        {
            // Bật/tắt camera theo trạng thái IsParkour khi không trong cutscene
            if (IsParkour)
            {
                fpsCamera.SetActive(false);
                thirdPersonCamera.SetActive(true);
                Crosshair.gameObject.SetActive(false);
            }
            else
            {
                fpsCamera.SetActive(true);
                thirdPersonCamera.SetActive(false);
                Crosshair.gameObject.SetActive(true);
            }
        }
    }
}

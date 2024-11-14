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
    public Image damageImage;  // Hình ảnh khi bị tấn công
    public Image deathImage;   // Hình ảnh khi chết
    float x, z;

    public bool IsParkour = true;
    private bool isTakingDamage;
    private float damageTimer;
    private Color damageColor = new Color(1f, 0f, 0f, 0.5f); // Màu đỏ nhấp nháy khi nhận sát thương
    private Color deathColor = new Color(1f, 0f, 0f, 1f);    // Màu đỏ khi chết
    private float damageFadeDuration = 0.1f;   // Thời gian hiển thị khi nhận sát thương
    private float deathFadeSpeed = 1.5f;       // Tốc độ hiển thị khi chết

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

        if (damageImage != null) damageImage.color = Color.clear; // Ẩn ảnh sát thương
        if (deathImage != null) deathImage.color = Color.clear;   // Ẩn ảnh chết
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

        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            StartCoroutine(HandleDeathEffect());
            StartCoroutine(WaitBeforeRespawn(1.5f)); // Gọi Coroutine để hồi sinh
        }
        else if (currentHealth > 0)
        {
            isDead = false;
            StopCoroutine(HandleDeathEffect());  // Ngừng hiệu ứng chết nếu hồi sinh
        }

        canControll = currentHealth > 0;
        isGrounded = controller.isGrounded;

        if (isTakingDamage && damageImage != null)
        {
            // Kiểm soát thời gian nhấp nháy ảnh sát thương
            damageTimer -= Time.deltaTime;
            damageImage.color = damageColor;

            if (damageTimer <= 0)
            {
                isTakingDamage = false;
                damageImage.color = Color.clear; // Ẩn ảnh khi hết thời gian
            }
        }

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

            if (canControll && !isDead)
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
            camF = cam.transform.forward;
            camR = cam.transform.right;
            camF.y = 0;
            camR.y = 0;
            hor = (x * camR + z * camF).normalized;

            if (hor.sqrMagnitude > 0.1f)
            {
                Quaternion Rota = Quaternion.LookRotation(hor);
                transform.rotation = Quaternion.Slerp(transform.rotation, Rota, moveSpeed * Time.deltaTime);
                controller.Move(hor * moveSpeed * Time.deltaTime);
            }
        }
        else
        {
            camF = fpsCam.transform.forward;
            camR = fpsCam.transform.right;
            camF.y = 0;
            camR.y = 0;
            hor = (x * camR + z * camF).normalized;

            if (hor.sqrMagnitude > .1f)
            {
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

        if (damageImage != null)
        {
            isTakingDamage = true;
            damageTimer = damageFadeDuration; // Reset bộ đếm thời gian nhấp nháy
        }
    }

    public void TakeHeal(float amount)
    {
        currentHealth = Mathf.Min(100, currentHealth + amount);
    }

    private IEnumerator HandleDeathEffect()
    {
        if (deathImage == null) yield break;

        float alpha = 0;
        while (alpha < 1)
        {
            alpha += Time.deltaTime * deathFadeSpeed;
            deathImage.color = new Color(deathColor.r, deathColor.g, deathColor.b, alpha);
            yield return null;
        }
    }

    IEnumerator WaitBeforeRespawn(float delayTime)
    {
        yield return new WaitForSeconds(delayTime); // Đợi 1.5 giây

        // Kiểm tra nếu GameManager và playerTransform hợp lệ
        if (GameManager.Instance != null && GameManager.Instance.playerTransform != null)
        {
            GameManager.Instance.LoadGameProgress();
            transform.position = GameManager.Instance.playerTransform.position; // Đặt lại vị trí
        }

        // Reset trạng thái khi hồi sinh
        currentHealth = 100; // Khôi phục sức khỏe
        isDead = false;
        canControll = true; // Cho phép điều khiển lại

        // Ẩn hiệu ứng chết
        if (deathImage != null)
        {
            deathImage.color = Color.clear;
        }

        Debug.Log("Player has respawned at save point.");
    }

    void ToggleCamera()
    {
        if (CutsceneManager.Instance.isCutscenePlaying)
        {
            fpsCamera.SetActive(false);
            thirdPersonCamera.SetActive(true);
            Crosshair.gameObject.SetActive(false);
        }
        else
        {
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

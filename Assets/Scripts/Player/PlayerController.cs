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
    public float curHealth = 100;
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


    private Vector3 savedPosition; // Lưu vị trí người chơi
    private bool isRespawning = false; // Biến kiểm soát coroutine hồi sinh

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        // Kiểm tra xem có dữ liệu vị trí nào đã lưu không
        if (PlayerPrefs.HasKey("PlayerPosX"))
        {
            float posX = PlayerPrefs.GetFloat("PlayerPosX");
            float posY = PlayerPrefs.GetFloat("PlayerPosY");
            float posZ = PlayerPrefs.GetFloat("PlayerPosZ");
            savedPosition = new Vector3(posX, posY, posZ);
        }
        else
        {
            savedPosition = transform.position;
        }

        // Tắt CharacterController trước khi thay đổi vị trí
        if (controller != null)
        {
            controller.enabled = false;
            transform.position = savedPosition;
            controller.enabled = true;
        }
        else
        {
            transform.position = savedPosition;
        }

        Debug.Log("Player position loaded at: " + savedPosition);
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
        canControll = true;
        isDead = false;
        isRespawning = false; // Đảm bảo trạng thái không bị lệch

        ToggleCamera();

        if (damageImage != null) damageImage.color = Color.clear;
        if (deathImage != null) deathImage.color = Color.clear;
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
            HealImage.fillAmount = Mathf.Lerp(HealImage.fillAmount, curHealth / 100f, Time.deltaTime * 5f);
        }

        if (curHealth <= 0 && !isDead)
        {
            isDead = true;
            SoundManager.Instance.PlaySound(SoundManager.Instance.PDie);
            StartCoroutine(HandleDeathEffect());
            StartCoroutine(WaitBeforeRespawn(1.5f)); // Gọi Coroutine để hồi sinh
        }
        else if (curHealth > 0)
        {
            isDead = false;
            StopCoroutine(HandleDeathEffect());  // Ngừng hiệu ứng chết nếu hồi sinh
        }

        canControll = curHealth > 0;
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
            SoundManager.Instance.PlaySound(SoundManager.Instance.PJump);
            ver.y = jumpForce;
            isJumping = true;
            isFalling = false;
        }
    }

    public void TakeDamage(float amount)
    {
        curHealth = Mathf.Max(0, curHealth - amount);
        //SoundManager.Instance.PlaySound(SoundManager.Instance.PHurt);
        if (damageImage != null)
        {
            isTakingDamage = true;
            SoundManager.Instance.PlaySound(SoundManager.Instance.PHurt);
            damageTimer = damageFadeDuration; // Reset bộ đếm thời gian nhấp nháy
        }
    }

    public void TakeHeal(float amount)
    {
        curHealth = Mathf.Min(100, curHealth + amount);
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

    private IEnumerator WaitBeforeRespawn(float delayTime)
    {
        if (isRespawning) yield break; // Ngừng nếu đã đang hồi sinh
        isRespawning = true;

        yield return new WaitForSeconds(delayTime);

        // Hồi sinh người chơi tại vị trí đã lưu
        transform.position = savedPosition; // Đặt lại vị trí người chơi đã lưu
        controller.enabled = false; // Tắt CharacterController tạm thời để tránh lỗi vật lý
        yield return null; // Đợi 1 frame
        controller.enabled = true; // Kích hoạt lại CharacterController sau khi thay đổi vị trí

        // Cập nhật lại các giá trị khi hồi sinh
        curHealth = 100; // Sức khỏe hồi phục đầy đủ
        isDead = false;
        canControll = true; // Cho phép điều khiển lại nhân vật
        isRespawning = false;

        // Nếu có hình ảnh khi chết, ẩn đi
        if (deathImage != null)
        {
            deathImage.color = Color.clear;
        }

        Debug.Log("Player has respawned at saved position.");
    }

    // Gọi phương thức này khi người chơi va chạm với một điểm lưu
    public void SavePosition(Vector3 newPosition)
    {
        savedPosition = newPosition;
        PlayerPrefs.SetFloat("PlayerPosX", savedPosition.x);
        PlayerPrefs.SetFloat("PlayerPosY", savedPosition.y);
        PlayerPrefs.SetFloat("PlayerPosZ", savedPosition.z);
        PlayerPrefs.Save();  // Lưu lại dữ liệu
        Debug.Log("Player position saved at: " + savedPosition);
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

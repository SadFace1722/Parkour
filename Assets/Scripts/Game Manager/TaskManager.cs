using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    Animator anim;
    public static TaskManager Instance;
    public TextMeshProUGUI Content;
    private List<Task> tasks = new List<Task>();
    public bool IsProcessing;
    public int CurrentIndex;
    public int KillCount;
    [SerializeField] GameObject[] AlienSke;
    private GameData gameData;

    private void Awake()
    {
        gameData = new GameData();

        if (Instance == null)
        {
            Instance = this;
        }
        Invoke("GetAnim", 1f);
    }

    void GetAnim()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        LoadProgress();

        tasks.Add(new Task(5f, "Khám phá khu vực.", BatDauKiemTra, KiemTraHoanTat, () => nv1.Instance.seeAroundArea));
        tasks.Add(new Task(5f, "Thử tìm kiếm xung quanh.", () => nv5.Instance.KiemTraXungQuanh));
        tasks.Add(new Task(15f, "Kích Hoạt lại máy phát điện.", TimKiemNangLuong, KichHoatMayPhatDien, () => nv2.Instance.generator));
        tasks.Add(new Task(10f, "Kiểm tra phòng chế tạo.", MoCuaPhongCheTao, KiemTraPhong, () => CraftDoor.Instance.OpenDoor));
        tasks.Add(new Task(10f, "Sử dụng bộ giáp.", KiemTraBoGiap, SudungBoGiap, () => nv3.Instance.isUseArmor));
        tasks.Add(new Task(2f, "Phá cửa với bộ giáp.", () => ExitDoor.Instance.DestroyDoor));
        tasks.Add(new Task(15f, "Nhảy qua khu vực Acid", () => nv6.Instance.BangQuaAcid));
        tasks.Add(new Task(2, "Nhặt khẩu súng từ xác chết.", NhatKhauSung, NhatSungHoanThanh, () => PickupGun.Instance.PickuptheGun));
        tasks.Add(new Task(2, "Tìm cách tắt Acid.", TimCachTatAcid, DaTatDuocAcid, () => TurnOffAcid.Instance.turnOff));
        tasks.Add(new Task(2, "Tiêu diệt quái vật.", BatDauTieuDietAcidAlien, DaTieuDietXongAcidAlien, () => KillCount >= 5));
        tasks.Add(new Task(2, "Tìm đường thoát khỏi đây.", TimDuongThoatKhoiDay, DaThoatKhoiKhuVucAcid, () => OpenDoor.Instance.IsOpenDoor));
        tasks.Add(new Task(2, "Tìm cách tắt lazer.", TieuDietQuaiVat, TatDuocLazer, () => GameLaser.Instance.isComplete));
        tasks.Add(new Task(2, "Kiểm tra xung quanh.", GiaiDo1, GiaiDo2, () => PlayDialogue.Instance.isSolve));
        tasks.Add(new Task(2, "Tiêu diệt quái vật.", CutscenePhongThiNghiem, KillCountAlienSke, () => KillCount >= 4));
        tasks.Add(new Task(2, "Nhập mật khẩu 314159 để mở cửa.", GiaiDo1, GiaiDo2, () => BunkerDoor.Instance.isOpen));
        tasks.Add(new Task(2, "Tiêu diệt Boss", GiaiDo1, GiaiDo2, () => Boss.Intance.isDead));
        if (tasks.Count > 0)
        {
            if (CurrentIndex == 0) // Nếu là lần đầu tiên chơi
            {
                CutsceneManager.Instance.PlayCutscene(0);
            }
            NextTask();
        }
    }

    private void Update()
    {
        if (IsProcessing && tasks[CurrentIndex].IsActive)
        {
            CheckStatusTask();
            SetTaskText();
        }
        if (anim != null)
        {
            anim.SetBool("Task", IsProcessing);
        }
    }

    void SetTaskText()
    {
        Task task = tasks[CurrentIndex];
        Content.text = task.NameTask;
    }

    public void NextTask()
    {
        if (CurrentIndex >= tasks.Count)
        {
            return;
        }
        Task task = tasks[CurrentIndex];
        task.OnStart?.Invoke();
        IsProcessing = true;
        task.IsActive = true;
    }

    void CheckStatusTask()
    {
        Task task = tasks[CurrentIndex];
        if (task.IsActive && !task.IsComplete && task.CompleteCondition())
        {
            CompleteTask();
        }
    }

    void CompleteTask()
    {
        Task task = tasks[CurrentIndex];
        task.OnComplete?.Invoke();
        task.IsComplete = true;
        IsProcessing = false;

        // Lưu tiến độ ngay khi hoàn thành nhiệm vụ
        SaveGame();

        StartCoroutine(TimeToNextTask());
    }

    IEnumerator TimeToNextTask()
    {
        Task task = tasks[CurrentIndex];
        yield return new WaitForSeconds(task.Delay);

        // Chuyển đến nhiệm vụ tiếp theo
        CurrentIndex++;
        SaveGame();
        NextTask();
    }

    public void SaveGame()
    {
        // Lấy thông tin hiện tại của người chơi và lưu
        Vector3 playerPosition = PlayerController.Instance.transform.position;
        float currentHealth = PlayerController.Instance.curHealth;
        int currentTaskIndex = CurrentIndex;
        bool isArmorEquipped = GameManager.Instance.isArmorEquipped;
        bool hasGun = GameManager.Instance.hasGun;

        // Gọi GameData để lưu
        gameData.SaveGame(GameManager.Instance.savePointID, playerPosition, currentHealth, currentTaskIndex, isArmorEquipped, hasGun);
    }

    void LoadProgress()
    {
        // Tải tiến độ đã lưu từ GameData
        int currentTaskIndex;
        gameData.LoadGame(out int savePointID, out Vector3 playerPosition, out float currentHealth, out currentTaskIndex, out bool isArmorEquipped, out bool hasGun);

        // Cập nhật tiến độ từ dữ liệu tải về
        CurrentIndex = currentTaskIndex;
        GameManager.Instance.savePointID = savePointID;
        GameManager.Instance.playerTransform.position = playerPosition;
        PlayerController.Instance.curHealth = currentHealth;
        GameManager.Instance.isArmorEquipped = isArmorEquipped;
        GameManager.Instance.hasGun = hasGun;

        // Khôi phục trạng thái của giáp và súng nếu đã trang bị trước đó
        if (isArmorEquipped)
        {
            ArmorController.Instance.EquipArmor();
        }
        if (hasGun)
        {
            WeaponManager.Instance.GivePlayerGun();
        }
    }

    private void OnApplicationQuit()
    {
        // Lưu tiến độ khi thoát game
        SaveGame();
    }

    // Các phương thức nhiệm vụ (tùy chỉnh theo nội dung game)
    void BatDauKiemTra() => RenderSettings.fog = true;
    void KiemTraHoanTat() => Debug.Log("");

    void TimKiemNangLuong() => Debug.Log("");
    void KichHoatMayPhatDien() => Debug.Log("");

    void MoCuaPhongCheTao() => CraftDoor.Instance.OpenDoor = true;
    void KiemTraPhong() => CraftDoor.Instance.OpenDoor = true;

    void KiemTraBoGiap() => CraftDoor.Instance.OpenDoor = true;
    void SudungBoGiap() => ArmorController.Instance.EquipArmor();

    void NhatKhauSung() { }
    void NhatSungHoanThanh()
    {
        WeaponManager.Instance.GivePlayerGun();
        if (TurnOffAcid.Instance.turnOff)
        {
            for (int i = 0; i < 5; i++)
            {
                EnemySpawner.Instance.SpawnEnemy();
            }
        }
    }

    void TimCachTatAcid() { }
    void DaTatDuocAcid() => TurnOffAcid.Instance.turnOff = true;

    void BatDauTieuDietAcidAlien()
    {
        TurnOffAcid.Instance.turnOff = true;
        WeaponManager.Instance.GivePlayerGun();
        if (TurnOffAcid.Instance.turnOff)
        {
            for (int i = 0; i < 5; i++)
            {
                EnemySpawner.Instance.SpawnEnemy();
            }
        }
    }
    void DaTieuDietXongAcidAlien() => KillCount = 5;

    void TimDuongThoatKhoiDay() => TurnOffAcid.Instance.turnOff = true;
    void DaThoatKhoiKhuVucAcid() => TurnOffAcid.Instance.turnOff = true;

    void TieuDietQuaiVat() => Debug.Log("");
    void TatDuocLazer() => Debug.Log("");


    void CutscenePhongThiNghiem()
    {
        StartCoroutine(WaitToCut());
        StartCoroutine(WaitToSpawn());
    }
    void KillCountAlienSke()
    {
        KillCount = 0;
    }
    IEnumerator WaitToCut()
    {
        yield return new WaitForSeconds(4);
        CutsceneManager.Instance.PlayCutscene(4);
    }
    IEnumerator WaitToSpawn()
    {
        yield return new WaitForSeconds(8.3f);
        foreach (var G in AlienSke)
        {
            if (G != null)
            {
                G.SetActive(true);
            }
        }
    }

    void GiaiDo1() => Debug.Log("");
    void GiaiDo2() => Debug.Log("");

}

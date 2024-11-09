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

    private GameData gameData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        Invoke("GetAnim", 1f);
        gameData = new GameData(); // Khởi tạo GameData
    }

    void GetAnim()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        LoadProgress(); // Tải tiến độ nhiệm vụ từ GameData khi game bắt đầu
        tasks.Add(new Task(5f, "Khám phá khu vực.", BatDauKiemTra, KiemTraHoanTat, () => nv1.Instance.seeAroundArea));
        tasks.Add(new Task(5f, "Thử tìm kiếm xung quanh.", () => nv5.Instance.KiemTraXungQuanh));
        tasks.Add(new Task(15f, "Kích Hoạt lại máy phát điện.", TimKiemNangLuong, KichHoatMayPhatDien, () => nv2.Instance.generator));
        tasks.Add(new Task(10f, "Kiểm tra phòng chế tạo.", MoCuaPhongCheTao, KiemTraPhong, () => CraftDoor.Instance.OpenDoor));
        tasks.Add(new Task(10f, "Sử dụng bộ giáp.", KiemTraBoGiap, SudungBoGiap, () => nv3.Instance.isUseArmor));
        tasks.Add(new Task(2f, "Phá cửa với bộ giáp.", () => ExitDoor.Instance.DestroyDoor));
        tasks.Add(new Task(5f, "Nhảy qua khu vực Acid", () => nv6.Instance.BangQuaAcid));
        tasks.Add(new Task(2, "Nhặt khẩu súng từ xác chết.", NhatKhauSung, NhatSungHoanThanh, () => PickupGun.Instance.PickuptheGun));
        tasks.Add(new Task(2, "Tìm cách tắt Acid.", TimCachTatAcid, DaTatDuocAcid, () => TurnOffAcid.Instance.turnOff));
        tasks.Add(new Task(2, "Tiêu diệt quái vật.", BatDauTieuDietAcidAlien, DaTieuDietXongAcidAlien, () => KillCount >= 5));
        tasks.Add(new Task(2, "Tìm đường thoát khỏi đây.", TimDuongThoatKhoiDay, DaThoatKhoiKhuVucAcid, () => OpenDoor.Instance.IsOpenDoor));

        if (tasks.Count > 0)
        {
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

        // Lưu tiến độ nhiệm vụ
        SaveProgress();

        StartCoroutine(TimeToNextTask());
    }

    IEnumerator TimeToNextTask()
    {
        Task task = tasks[CurrentIndex];
        yield return new WaitForSeconds(task.Delay);
        CurrentIndex++;
        NextTask();
    }

    void SaveProgress()
    {
        // Lưu tiến độ nhiệm vụ hiện tại (CurrentIndex) qua GameData
        gameData.SaveGame(GameManager.Instance.savePointID, GameManager.Instance.playerTransform.position, PlayerController.Instance.currentHealth, CurrentIndex, GameManager.Instance.isArmorEquipped, GameManager.Instance.hasGun);
    }

    void LoadProgress()
    {
        // Tải tiến độ nhiệm vụ đã lưu từ GameData
        int currentTaskIndex;
        gameData.LoadGame(out int savePointID, out Vector3 playerPosition, out float currentHealth, out currentTaskIndex, out bool isArmorEquipped, out bool hasGun);

        // Cập nhật tiến độ nhiệm vụ từ thông tin tải về
        CurrentIndex = currentTaskIndex;
        GameManager.Instance.savePointID = savePointID;
        GameManager.Instance.playerTransform.position = playerPosition;
        PlayerController.Instance.currentHealth = currentHealth;
        GameManager.Instance.isArmorEquipped = isArmorEquipped;
        GameManager.Instance.hasGun = hasGun;
    }


    // Các phương thức nhiệm vụ (chưa thực hiện trong ví dụ này)
    void BatDauKiemTra()
    {
        RenderSettings.fog = true;
    }
    void KiemTraHoanTat() => Debug.Log("");

    void TimKiemNangLuong() => Debug.Log("");
    void KichHoatMayPhatDien() => Debug.Log("");

    void MoCuaPhongCheTao()
    {
        nv2.Instance.generator = true;
    }
    void KiemTraPhong()
    {
        nv2.Instance.generator = true;
        CraftDoor.Instance.OpenDoor = true;
    }

    void KiemTraBoGiap()
    {
        nv2.Instance.generator = true;
        CraftDoor.Instance.OpenDoor = true;
    }
    void SudungBoGiap()
    {
        nv2.Instance.generator = true;
        CraftDoor.Instance.OpenDoor = true;
        ArmorController.Instance.EquipArmor();
    }

    void NhatKhauSung()
    {
        PlayerController.Instance.IsParkour = false;
    }
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

    void TimCachTatAcid()
    {

    }
    void DaTatDuocAcid()
    {
        TurnOffAcid.Instance.turnOff = true;
    }

    void BatDauTieuDietAcidAlien()
    {
        TurnOffAcid.Instance.turnOff = true;
    }

    void DaTieuDietXongAcidAlien()
    {
        KillCount = 5;
        TurnOffAcid.Instance.turnOff = true;
    }
    void TimDuongThoatKhoiDay()
    {
        TurnOffAcid.Instance.turnOff = true;
    }
    void DaThoatKhoiKhuVucAcid()
    {

    }
}

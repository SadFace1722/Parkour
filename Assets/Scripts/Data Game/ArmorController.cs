using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorController : MonoBehaviour
{
    public static ArmorController Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public void EquipArmor()
    {
        // Logic trang bị giáp
        GameManager.Instance.isArmorEquipped = true; // Cập nhật trạng thái giáp
    }
}




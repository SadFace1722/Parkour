using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public void GivePlayerGun()
    {
        PlayerController.Instance.Shotgun.SetActive(true);
        GameManager.Instance.hasGun = true;
    }
}
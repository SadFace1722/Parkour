using UnityEngine;

public class HoiSinh : MonoBehaviour
{
    // private API api;
    // private Vector3 defaultPosition; 

    // void Start()
    // {
    //     api = FindObjectOfType<API>();

    //     defaultPosition = new Vector3(0, 0, 0); 
    // }

    // void Update()
    // {

    //     if (Input.GetKeyDown(KeyCode.O))
    //     {
    //         Die(); 
    //     }
    // }


    // public void Respawn(Vector3 position)
    // {
    //     transform.position = position;
    //     Debug.Log("Người chơi đã hồi sinh tại: " + position);
    // }


    // private void Die()
    // {
    //     Debug.Log("Người chơi đã chết!");

    //     // Kiểm tra xem có checkpoint đã lưu không
    //     StartCoroutine(api.GetLastCheckpoint(OnGetLastCheckpoint));
    // }

    // private void OnGetLastCheckpoint(Vector3 lastPosition)
    // {
    //     Debug.Log("Vị trí checkpoint cuối cùng nhận được: " + lastPosition);
    //     if (lastPosition != Vector3.zero)
    //     {
    //         Respawn(lastPosition);
    //     }
    //     else
    //     {
    //         Debug.Log("Không tìm thấy vị trí hồi sinh, hồi sinh ở vị trí mặc định.");
    //         Respawn(defaultPosition);
    //     }
    // }
}
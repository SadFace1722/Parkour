using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory.hasKey = true; // Đặt giá trị true khi nhặt chìa khóa
            Debug.Log("Key collected! PlayerInventory.hasKey = " + PlayerInventory.hasKey); // Kiểm tra xem có true không
            Destroy(gameObject); // Xóa chìa khóa khỏi cảnh
        }
    }
    // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }
}

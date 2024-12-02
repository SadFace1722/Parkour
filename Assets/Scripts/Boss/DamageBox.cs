using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBox : MonoBehaviour
{
    [SerializeField] BoxCollider box;
    [SerializeField] int Damage;
    private void Start()
    {
        box = GetComponent<BoxCollider>();
        box.enabled = true;
        Destroy(gameObject, 4.5f);
        Invoke("TurnOffBox", 0.2f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController.Instance.TakeDamage(Damage);
        }
    }
    void TurnOffBox()
    {
        box.enabled = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : MonoBehaviour
{
    [SerializeField] private float fireRate = 0.35f;
    private float lastFireTime = 0f;

    [SerializeField] private GameObject bulletImpactEffect;
    private Animator animator;

    void Start()
    {
        animator = transform.parent.GetComponent<Animator>();
        if (bulletImpactEffect == null)
        {
            Debug.LogError("Bullet Impact Effect chưa được gán!");
        }
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= lastFireTime + fireRate)
        {
            if (RayChecker.Instance.RayCheck())
            {
                PlayerInterface inter = RayChecker.Instance.hit.collider.gameObject.GetComponent<PlayerInterface>();
                if (inter != null)
                {
                    inter.Shoot();
                    Debug.Log("Interact được gọi với: " + RayChecker.Instance.hit.collider.gameObject.name);
                }

                animator.SetTrigger("Shoot");

                SpawnBulletImpactEffect();

                lastFireTime = Time.time;
            }
        }
        else
        {
            animator.ResetTrigger("Shoot");
        }
    }

    private void SpawnBulletImpactEffect()
    {
        if (bulletImpactEffect != null)
        {
            Instantiate(bulletImpactEffect, RayChecker.Instance.hit.point, Quaternion.LookRotation(RayChecker.Instance.hit.normal));
        }
    }
}

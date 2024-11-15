using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayChecker : MonoBehaviour
{
    public static RayChecker Instance;
    public bool iss;
    public Ray ray;
    public RaycastHit hit, hitBlock;
    [SerializeField] LayerMask objectLayer, blockLayer;
    [SerializeField] float disray = 5f, disToObject = 2f, dis = 2f;
    Animator anim;

    public float shootCooldown = 1.0f;
    private float lastShootTime = 0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

    }
    void Update()
    {
        if (anim == null)
        {
            GameObject shotgun = GameObject.Find("Shotgun");

            if (shotgun != null)
            {
                anim = shotgun.GetComponent<Animator>();
            }
        }
        if (RayCheck())
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                PlayerInterface inter = hit.collider.gameObject.GetComponent<PlayerInterface>();
                if (inter != null)
                {
                    inter.Interact();
                    Debug.Log("Interact được gọi với: " + hit.collider.gameObject.name);
                }
            }


            if (!PlayerController.Instance.IsParkour && Input.GetButtonDown("Fire1") && Time.time >= lastShootTime + shootCooldown)
            {
                PlayerInterface inter = hit.collider.gameObject.GetComponent<PlayerInterface>();
                if (anim != null)
                {
                    anim.SetTrigger("Shoot");  // Đảm bảo trigger này có trong Animator
                }
                if (inter != null)
                {
                    inter.Shoot();
                    lastShootTime = Time.time;
                    Debug.Log("Shoot được gọi với: " + hit.collider.gameObject.name);
                }
            }
        }
    }

    public bool RayCheck()
    {
        ray = new Ray(transform.position, transform.forward);
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * disray, Color.red);

        if (Physics.Raycast(ray, out hit, disray, objectLayer))
        {
            if (Physics.Raycast(ray, out hitBlock, disray, blockLayer))
            {
                return false;
            }
            else
            {
                disToObject = Vector3.Distance(transform.position, hit.collider.gameObject.transform.position);
                if (disToObject < dis)
                {
                    return true;
                }
            }
        }
        return false;
    }
}

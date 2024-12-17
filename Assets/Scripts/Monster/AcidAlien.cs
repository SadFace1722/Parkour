using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class AcidAlien : MonoBehaviour, PlayerInterface
{
    NavMeshAgent nav;
    Animator anim;
    public static AcidAlien Instance;

    public float detectionRadius = 10f;
    public float attackRadius = 2f;
    public float attackDamage = 10f;
    public float health = 50f;
    public float healAmount = 5f;

    private Transform player;
    private bool isPlayerInRange = false;

    public float attackCooldown = 2f;
    private float lastAttackTime = 0f;

    private bool isOnNavMeshLink = false;
    public List<Transform> safePoints = new List<Transform>();

    public float lowHealthThreshold = 50f;
    public bool isHealing = false;
    public float healingDistance = 3f;

    private float normalSpeed;
    public float healingSpeedMultiplier = 1.5f;

    private bool isDead = false; // Biến kiểm tra trạng thái chết của quái


    public GameObject projectilePrefab; // Prefab của đạn
    public float rangedAttackRadius = 7f; // Khoảng cách tấn công từ xa
    public float projectileSpeed = 10f; // Tốc độ đạn
    public float rangedAttackCooldown = 3f;
    private float lastRangedAttackTime = 0f;
    public float aimOffset = 1.0f;
    public Transform FirePoint;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        normalSpeed = nav.speed;

        GameObject saveSpotParent = GameObject.Find("SaveSpotOfAcidAlien");
        if (saveSpotParent != null && saveSpotParent.activeInHierarchy)
        {
            foreach (Transform child in saveSpotParent.transform)
            {
                safePoints.Add(child);
            }
        }
    }

    private void Update()
    {
        if (isDead) return;

        if (TurnOffAcid.Instance != null && TurnOffAcid.Instance.turnOff)
        {
            safePoints.Clear();
        }

        CheckHealingArea();

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (health <= lowHealthThreshold && safePoints.Count > 0)
        {
            FindAndMoveToSafePoint();
        }
        else
        {
            if (distanceToPlayer <= detectionRadius)
            {
                isPlayerInRange = true;
                nav.SetDestination(player.position);
                LookAtPlayer();
            }
            else
            {
                isPlayerInRange = false;
            }

            // Kiểm tra tấn công từ xa
            if (isPlayerInRange && distanceToPlayer <= rangedAttackRadius && distanceToPlayer > attackRadius)
            {
                if (Time.time >= lastRangedAttackTime + rangedAttackCooldown)
                {
                    SprayAnim();
                    SoundManager.Instance.PlaySoundAtPosition(SoundManager.Instance.AASpit, transform.position);
                }
            }
            // Tấn công cận chiến nếu đủ gần
            else if (isPlayerInRange && distanceToPlayer <= attackRadius && Time.time >= lastAttackTime + attackCooldown)
            {
                AttackPlayer(); // Gọi hàm tấn công cận chiến
            }
        }

        CheckIfUsingNavMeshLink();
        UpdateWalkingAnimation();
        if (Boss.Intance.isDead)
        {
            Die();
        }
    }


    private void AttackPlayer()
    {
        anim.SetTrigger("Attack");
        SoundManager.Instance.PlaySoundAtPosition(SoundManager.Instance.AAttack, transform.position);
        lastAttackTime = Time.time;
    }

    public void Shoot()
    {
        TakeDamage(15);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        // Kích hoạt animation "Hurt"
        anim.SetTrigger("Hurt");
        SoundManager.Instance.PlaySoundAtPosition(SoundManager.Instance.AAHurt, transform.position);
        if (health <= 0 && !isDead) // Kiểm tra chết
        {
            Die();
        }
    }

    void Heal()
    {
        health += healAmount * Time.deltaTime;
        if (health > 100f)
        {
            health = 100f;
        }
    }

    private void Die()
    {
        isDead = true; // Đánh dấu trạng thái chết
        anim.SetBool("isDead", true);
        SoundManager.Instance.PlaySoundAtPosition(SoundManager.Instance.AADie, transform.position);
        nav.isStopped = true; // Ngăn di chuyển khi chết
        Debug.Log("AcidAlien đã chết!");
        TaskManager.Instance.KillCount++;
    }

    private void CheckIfUsingNavMeshLink()
    {
        if (nav.isOnOffMeshLink && !isOnNavMeshLink)
        {
            if (health <= lowHealthThreshold)
            {
                anim.SetTrigger("Dive");
            }
            isOnNavMeshLink = true;
        }
        else if (!nav.isOnOffMeshLink && isOnNavMeshLink)
        {
            isOnNavMeshLink = false;
        }
    }

    private void FindAndMoveToSafePoint()
    {
        if (safePoints.Count > 0)
        {
            Transform closestSafePoint = null;
            float closestDistance = Mathf.Infinity;

            foreach (Transform safePoint in safePoints)
            {
                float distance = Vector3.Distance(transform.position, safePoint.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestSafePoint = safePoint;
                }
            }

            if (closestSafePoint != null)
            {
                nav.SetDestination(closestSafePoint.position);

                if (closestDistance <= nav.stoppingDistance)
                {
                    anim.SetBool("isWalking", false);
                }
                else
                {
                    anim.SetBool("isWalking", true);
                }
            }
        }
        else
        {
            // Tấn công player khi không có điểm hồi máu
            nav.SetDestination(player.position);
            anim.SetBool("isWalking", true);
        }
    }

    private void UpdateWalkingAnimation()
    {
        if (nav.velocity.sqrMagnitude > 0.1f)
        {
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
    }

    private void CheckHealingArea()
    {
        if (safePoints.Count == 0)
        {
            isHealing = false;
            return;
        }

        foreach (Transform safePoint in safePoints)
        {
            float distance = Vector3.Distance(transform.position, safePoint.position);
            if (distance <= healingDistance && safePoint.gameObject != null)
            {
                isHealing = true;
                Heal();
                return;
            }
        }
        isHealing = false;
    }
    void SprayAnim()
    {
        anim.SetTrigger("Spray");
        lastRangedAttackTime = Time.time;
    }
    public void RangedAttack()
    {
        GameObject projectile = Instantiate(projectilePrefab, FirePoint.position, Quaternion.identity);

        // Tính hướng bắn với độ lệch
        Vector3 targetPosition = player.position + new Vector3(Random.Range(-aimOffset, aimOffset), Random.Range(-aimOffset, aimOffset), Random.Range(-aimOffset, aimOffset));
        Vector3 direction = (targetPosition - transform.position).normalized;

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.velocity = direction * projectileSpeed;
    }

    void LookAtPlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        directionToPlayer.y = 0; // Đảm bảo không xoay theo trục Y

        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // Điều chỉnh tốc độ quay
    }
}

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
    public float health = 100f;
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
        else
        {
            Debug.LogWarning("Không tìm thấy hoặc SaveSpotOfAcidAlien bị hủy hoặc không hoạt động.");
        }
    }

    private void Update()
    {
        if (isDead) return; // Ngăn không cho di chuyển hoặc tấn công khi quái đã chết

        if (TurnOffAcid.Instance != null && TurnOffAcid.Instance.turnOff)
        {
            safePoints.Clear(); // Xóa danh sách điểm hồi máu
        }

        CheckHealingArea();

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Điều kiện: Nếu máu thấp nhưng không có điểm hồi máu => Tấn công player
        if (health <= lowHealthThreshold && safePoints.Count > 0)
        {
            FindAndMoveToSafePoint();
        }
        else
        {
            if (distanceToPlayer <= detectionRadius)
            {
                isPlayerInRange = true;
                nav.SetDestination(player.position); // Quái sẽ di chuyển đến player khi không tìm thấy điểm hồi máu
            }
            else
            {
                isPlayerInRange = false;
            }

            if (isPlayerInRange && distanceToPlayer <= attackRadius && Time.time >= lastAttackTime + attackCooldown)
            {
                AttackPlayer();
            }
        }

        CheckIfUsingNavMeshLink();
        UpdateWalkingAnimation();

        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeDamage(15);
        }
    }

    private void AttackPlayer()
    {
        anim.SetTrigger("Attack");
        lastAttackTime = Time.time;
    }

    public void Shoot()
    {
        TakeDamage(15);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
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
        nav.isStopped = true; // Ngăn di chuyển khi chết
        Debug.Log("AcidAlien đã chết!");
        TaskManager.Instance.KillCount++;
        Destroy(gameObject, 2f);
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
}

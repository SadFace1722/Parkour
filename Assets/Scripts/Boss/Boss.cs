using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour, PlayerInterface
{
    public float health = 100f;
    public float damage = 20f;
    public float attackRange = 10f;
    public float safeDistance = 5f;
    public float fireRate = 1f;
    public float bulletSpeed = 20f;
    public float accuracy = 0.5f;
    public GameObject bulletPrefab;
    public GameObject specialBulletPrefab;
    public Transform firePoint;
    public Transform firePointLeft;
    public Transform firePointRight;
    public Transform player;

    private NavMeshAgent navMeshAgent;
    private float nextFireTime = 0f;
    private bool isPlayerInRange = false;
    private bool isAttacking = false;
    private bool isDead = false;

    // Shield variables
    private bool isShieldActive = false;
    public float shieldMaxHealth = 100f;   // Máu tối đa của khiên
    private float shieldCurrentHealth;     // Máu hiện tại của khiên
    public float shieldActivationChance = 0.2f; // Xác suất kích hoạt khiên (20%)

    // Cooldown variables
    public float shieldCooldown = 10f;     // Thời gian hồi chiêu của khiên
    private bool shieldOnCooldown = false; // Trạng thái đang hồi chiêu

    private float damageTaken = 0f;        // Theo dõi lượng máu đã mất

    public Animator anim, animSkill2;
    private int LayerAttack, LayerSkill1;

    void Start()
    {
        anim = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player")?.transform;

        if (player == null)
        {
            Debug.LogError("Player not found! Ensure the player has the correct tag.");
            enabled = false;
            return;
        }

        LayerAttack = anim.GetLayerIndex("Attack");
        LayerSkill1 = anim.GetLayerIndex("Skill1");

        InvokeRepeating("CheckHealthAndUpdate", 0f, 1f);
    }

    void Update()
    {
        if (isDead) return;

        Idle();
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        isPlayerInRange = distanceToPlayer <= attackRange;

        if (isPlayerInRange)
        {
            ChasePlayer();
        }
        else
        {
            navMeshAgent.ResetPath();
        }

        if (damageTaken >= 20f)
        {
            anim.SetTrigger("Skill1");
            damageTaken = 0f;
            anim.SetLayerWeight(LayerAttack, 0);
            anim.SetLayerWeight(LayerSkill1, 1);
            Invoke("TurnOffSkill", 2f);
        }

        if (isPlayerInRange && Time.time >= nextFireTime && !isAttacking)
        {
            isAttacking = true;
            anim.SetTrigger("Attack");
        }

        RotateTowardsPlayer();
    }

    void CheckHealthAndUpdate()
    {
        if (health <= 0 && !isDead)
        {
            isDead = true;
            Die();
        }
    }

    void TurnOffSkill()
    {
        anim.SetLayerWeight(LayerAttack, 1);
        anim.SetLayerWeight(LayerSkill1, 0);
        isAttacking = false;
    }

    void ChasePlayer()
    {
        if (Vector3.Distance(transform.position, player.position) > safeDistance)
        {
            navMeshAgent.SetDestination(player.position);
        }
        else
        {
            navMeshAgent.ResetPath();
        }
    }
    public void Shoot()
    {
        TakeDamage(Random.Range(5, 10));
    }
    public void TakeDamage(float amount)
    {
        if (isShieldActive)
        {
            shieldCurrentHealth -= amount; // Giảm máu khiên
            Debug.Log("Shield health: " + shieldCurrentHealth);

            if (shieldCurrentHealth <= 0) // Khi khiên hết máu
            {
                DeactivateShield();
            }
            return; // Không nhận sát thương vào máu chính khi có khiên
        }

        // Kích hoạt khiên với xác suất nhất định và nếu không đang trong cooldown
        if (!shieldOnCooldown && Random.value < shieldActivationChance && !isShieldActive && !isDead)
        {
            ActivateShield();
            return;
        }

        // Nếu không có khiên, boss nhận sát thương bình thường
        health -= amount;
        damageTaken += amount;
        anim.SetTrigger("Hurt2");

        if (health <= 0 && !isDead)
        {
            health = 0;
            isDead = true;
            Die();
        }
    }

    void ActivateShield()
    {
        isShieldActive = true;
        shieldCurrentHealth = shieldMaxHealth; // Đặt lại máu của khiên
        shieldOnCooldown = true; // Bắt đầu cooldown
        Debug.Log("Shield activated!");
        animSkill2.SetBool("Skill2", true);
    }

    void DeactivateShield()
    {
        isShieldActive = false;
        Debug.Log("Shield deactivated!");
        animSkill2.SetBool("Skill2", false);

        // Bắt đầu hồi chiêu khi khiên bị phá
        Invoke("ResetShieldCooldown", shieldCooldown);
    }

    void ResetShieldCooldown()
    {
        shieldOnCooldown = false; // Khiên đã sẵn sàng kích hoạt lại
        Debug.Log("Shield cooldown reset! Ready to activate again.");
    }

    void Idle()
    {
        anim.SetBool("Idle", isPlayerInRange);
    }

    void Die()
    {
        anim.SetBool("Death", isDead);
        navMeshAgent.isStopped = true;
    }

    void RotateTowardsPlayer()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0;
        Quaternion rotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5f);
    }
    public void Fire()
    {
        nextFireTime = Time.time + 1f / fireRate;
        isAttacking = false;

        Vector3 directionToPlayer = (player.position - firePoint.position).normalized;
        float deviationAmount = 1.0f - accuracy;
        Vector3 deviationVector = new Vector3(
            Random.Range(-deviationAmount, deviationAmount),
            Random.Range(-deviationAmount / 2, deviationAmount / 2),
            0
        );

        Vector3 firingDirection = (directionToPlayer + deviationVector).normalized;
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        bulletRb.velocity = firingDirection * bulletSpeed;
    }

    public void FireSpecialBullet()
    {
        nextFireTime = Time.time + 1f / fireRate;
        FireBulletFrom(firePointLeft);
        FireBulletFrom(firePointRight);
    }

    void FireBulletFrom(Transform firePoint)
    {
        Vector3 directionToPlayer = (player.position - firePoint.position).normalized;
        for (int i = -1; i <= 1; i++)
        {
            Vector3 firingDirection = directionToPlayer + firePoint.right * i * 0.5f;
            firingDirection.Normalize();
            GameObject bullet = Instantiate(specialBulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            bulletRb.velocity = firingDirection * bulletSpeed;
        }
    }
}

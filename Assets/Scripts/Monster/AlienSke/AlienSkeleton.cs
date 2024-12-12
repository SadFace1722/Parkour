using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AlienSkeleton : MonoBehaviour, PlayerInterface
{
    public float detectionRadius = 10f;  // Bán kính phát hiện Player
    public float attackDistance = 2f;    // Khoảng cách tấn công Player
    public float baseSpeed = 3.5f;       // Tốc độ di chuyển cơ bản
    public float increasedSpeed = 6.5f;  // Tốc độ tăng khi máu thấp
    public float attackCooldown = 1.5f;  // Thời gian hồi chiêu tấn công bình thường
    public float fastAttackCooldown = 1f; // Thời gian hồi chiêu tấn công khi máu thấp
    public int maxHealth = 100;          // Máu tối đa

    private NavMeshAgent agent;
    private Transform player;
    private bool isDead = false;
    private bool canAttack = true;

    private Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        agent.speed = baseSpeed;
    }

    void Update()
    {
        if (isDead) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Quay liên tục về phía người chơi
        LookAtPlayer();

        // Kiểm tra điều kiện tốc độ di chuyển
        bool isFast = maxHealth <= 50; // Kiểm tra máu có dưới hoặc bằng 150
        if (isFast)
        {
            SoundManager.Instance.PlaySoundAtPosition(SoundManager.Instance.ASRoar, transform.position);
            animator.SetBool("Fast", true);  // Di chuyển nhanh
            animator.SetBool("Move", false);  // Dừng animation Move
            agent.speed = increasedSpeed;    // Tăng tốc độ di chuyển
        }
        else
        {
            animator.SetBool("Fast", false); // Dừng animation Fast
            animator.SetBool("Move", distanceToPlayer <= detectionRadius && agent.remainingDistance > agent.stoppingDistance); // Di chuyển bình thường
            agent.speed = baseSpeed;          // Tốc độ di chuyển bình thường
        }

        // Điều khiển hành động khi gần Player
        if (distanceToPlayer <= detectionRadius)
        {
            if (distanceToPlayer <= attackDistance && canAttack)
            {
                AttackPlayer();
            }
            else
            {
                agent.SetDestination(player.position);
            }
        }
    }


    private void LookAtPlayer()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0;

        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // Tốc độ quay (5f có thể điều chỉnh)
    }

    // Hàm tấn công người chơi
    private void AttackPlayer()
    {
        canAttack = false;

        if (animator != null)
        {
            if (maxHealth <= 150)
            {
               // SoundManager.Instance.PlaySoundAtPosition(SoundManager.Instance.ASAttack, transform.position);
                animator.SetTrigger("AttackFast");
            }
            else
            {
               // SoundManager.Instance.PlaySoundAtPosition(SoundManager.Instance.ASAttack, transform.position);
                animator.SetTrigger("Attack");
            }
        }
        StartCoroutine(AttackCooldown());
    }

    private IEnumerator AttackCooldown()
    {
        float cooldownTime = maxHealth <= 150 ? attackCooldown : fastAttackCooldown;
        yield return new WaitForSeconds(cooldownTime);
        canAttack = true;
    }
    public void Shoot()
    {
        TakeDamage(15);
    }
    // Hàm nhận sát thương
    public void TakeDamage(int damage)
    {
        if (isDead) return;

        maxHealth -= damage;  // Giảm máu khi nhận sát thương
        animator.SetTrigger("Hurt");
     //  SoundManager.Instance.PlaySoundAtPosition(SoundManager.Instance.ASHurt, transform.position);
        if (maxHealth <= 0)
        {
            maxHealth = 0;
            SoundManager.Instance.PlaySoundAtPosition(SoundManager.Instance.ASDie, transform.position);
            Die();
        }
    }

    // Hàm chết của AlienSkeleton
    private void Die()
    {
        isDead = true;
        agent.isStopped = true;
        TaskManager.Instance.KillCount++;
        SoundManager.Instance.PlaySoundAtPosition(SoundManager.Instance.ASDie, transform.position);
        if (animator != null)
        {
            animator.SetBool("Death", true);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }
}

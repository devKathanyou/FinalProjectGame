using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform player; // อ้างอิงถึงผู้เล่น
    public float speed = 2f; // ความเร็วของศัตรู
    public float attackRange = 1f; // ระยะเริ่มโจมตี
    public int damage = 1; // ความเสียหายที่ศัตรูทำ
    public float attackCooldown = 1f; // เวลาระหว่างการโจมตี
    private float lastAttackTime; // เวลาที่โจมตีครั้งล่าสุด

    private PlayerHide playerHide; // อ้างอิงถึงสคริปต์ PlayerHide
    private Animator animator; // อ้างอิงถึงคอมโพเนนต์ Animator
    private Vector3 localScale; // ขนาดเดิมของศัตรู

    public Transform[] waypoints; // จุด Waypoints สำหรับการเดิน
    private int currentWaypointIndex = 0; // Waypoint ปัจจุบัน
    private bool isReturningToWaypoints = false; // ศัตรูกำลังเดินตาม Waypoints หรือไม่
    public float waitTime = 2f; // เวลาที่รอที่ Waypoint
    private bool isWaiting = false; // ศัตรูกำลังรออยู่หรือไม่

    public float viewDistance = 5f; // ระยะที่ศัตรูมองเห็นผู้เล่น
    public float viewAngle = 60f; // มุมมองของศัตรูในองศา

    public AudioSource audioSource; // แหล่งเสียง
    public AudioClip enemyAlertClip; // เสียงที่เล่นเมื่อเห็นผู้เล่น

    private bool isSoundPlaying = false; // เช็คว่าเสียงกำลังเล่นหรือไม่

    private void Start()
    {
        playerHide = player.GetComponent<PlayerHide>();
        animator = GetComponent<Animator>(); // รับคอมโพเนนต์ Animator
        localScale = transform.localScale; // บันทึกขนาดเดิมของศัตรู

        if (waypoints.Length > 0)
        {
            transform.position = waypoints[0].position; // เริ่มต้นที่ Waypoint แรก
        }
    }

    private void Update()
    {
        if (speed == 0) return; // หยุดการเคลื่อนไหวถ้า speed เป็น 0

        float distanceToPlayer = Mathf.Abs(transform.position.x - player.position.x);

        if (playerHide.IsHiding)
        {
            isReturningToWaypoints = true; // เดินตาม Waypoints
            WalkAwayFromPlayer();
        }
        else if (CanSeePlayer())
        {
            if (!isSoundPlaying)
            {
                PlayAlertSound(); // เล่นเสียงเมื่อมองเห็นผู้เล่น
            }

            isReturningToWaypoints = false; // หยุดเดินตาม Waypoints
            if (distanceToPlayer < attackRange)
            {
                if (Time.time - lastAttackTime >= attackCooldown)
                {
                    AttackPlayer();
                    lastAttackTime = Time.time;
                }
            }
            else
            {
                FollowPlayer();
            }
        }
        else
        {
            if (isSoundPlaying)
            {
                StopAlertSound(); // หยุดเสียงเมื่อผู้เล่นออกนอกระยะมองเห็น
            }

            isReturningToWaypoints = true; // กลับไปเดินตาม Waypoints
            WalkAwayFromPlayer();
        }

        if (!isReturningToWaypoints && Mathf.Approximately(transform.position.x, player.position.x))
        {
            animator.SetBool("isWalking", false);
        }
    }

    // ฟังก์ชันสำหรับเปิด/ปิดการเคลื่อนไหว
    public void SetCanMove(bool canMove)
    {
        if (!canMove)
        {
            speed = 0f; // หยุดการเคลื่อนไหว
            animator.SetBool("isWalking", false);
        }
        else
        {
            speed = 5.5f; // กลับมาเคลื่อนไหวตามปกติ
        }
    }

    void PlayAlertSound()
    {
        if (audioSource != null && enemyAlertClip != null)
        {
            audioSource.clip = enemyAlertClip;
            audioSource.Play();
            isSoundPlaying = true;
        }
    }

    void StopAlertSound()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
            isSoundPlaying = false;
        }
    }

    void FollowPlayer()
    {
        animator.SetBool("isWalking", true);

        if (transform.position.x < player.position.x)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            transform.localScale = new Vector3(Mathf.Abs(localScale.x), localScale.y, localScale.z);
        }
        else if (transform.position.x > player.position.x)
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
            transform.localScale = new Vector3(-Mathf.Abs(localScale.x), localScale.y, localScale.z);
        }
    }

    void WalkAwayFromPlayer()
    {
        if (isWaiting) return;

        animator.SetBool("isWalking", true);

        if (waypoints.Length > 0)
        {
            Transform targetWaypoint = waypoints[currentWaypointIndex];
            Vector3 direction = (targetWaypoint.position - transform.position).normalized;
            transform.Translate(direction * speed * Time.deltaTime);

            if (direction.x > 0)
                transform.localScale = new Vector3(Mathf.Abs(localScale.x), localScale.y, localScale.z);
            else if (direction.x < 0)
                transform.localScale = new Vector3(-Mathf.Abs(localScale.x), localScale.y, localScale.z);

            if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
            {
                StartCoroutine(WaitAtWaypoint());
            }
        }
    }

    IEnumerator WaitAtWaypoint()
    {
        isWaiting = true;
        animator.SetBool("isWalking", false);
        yield return new WaitForSeconds(waitTime);
        isWaiting = false;

        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
    }

    void AttackPlayer()
    {
        animator.SetTrigger("Attack");
        StartCoroutine(DealDamageAfterAttack());
    }

    IEnumerator DealDamageAfterAttack()
    {
        yield return new WaitForSeconds(1.0f);

        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
    }

    private bool CanSeePlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.right, directionToPlayer);

        if (angle < viewAngle / 2f && Vector3.Distance(transform.position, player.position) < viewDistance)
        {
            return true;
        }

        angle = Vector3.Angle(-transform.right, directionToPlayer);
        if (angle < viewAngle / 2f && Vector3.Distance(transform.position, player.position) < viewDistance)
        {
            return true;
        }

        return false;
    }
}
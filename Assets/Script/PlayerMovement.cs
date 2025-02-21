using UnityEngine;
using DG.Tweening; // Import DOTween namespace

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float runSpeedMultiplier = 2f;
    public float smoothDuration = 0.1f;
    public Animator animator;
    public bool isRunning;
    public bool isMoving;
    public bool canMove = true;

    private Rigidbody2D rb;
    private Vector2 movement;
    private SpriteRenderer spriteRenderer;
    private float currentSpeed;

    public StaminaBar staminaBar;

    // Audio components
    public AudioSource walkAudioSource;
    public AudioSource runAudioSource;
    public AudioSource staminaRegenAudioSource; // เพิ่มเสียงฟื้นฟู Stamina

    // To keep track of ongoing tweens
    private Tween speedTween;
    private float previousStamina; // เก็บค่าก่อนหน้าเพื่อเช็คการเปลี่ยนแปลง

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentSpeed = 0f;
        previousStamina = staminaBar.currentStamina; // กำหนดค่าเริ่มต้น
    }

    void Update()
    {
        if (!canMove) return;

        float targetSpeed = Input.GetAxis("Horizontal") * moveSpeed;

        isMoving = Mathf.Abs(targetSpeed) > 0.1f;

        if (Input.GetKey(KeyCode.LeftShift) && isMoving && staminaBar.currentStamina > 0)
        {
            isRunning = true;
            targetSpeed *= runSpeedMultiplier;
        }
        else
        {
            isRunning = false;
        }

        HandleAudio();
        HandleStaminaRegenAudio(); // เรียกใช้การเช็คเสียงฟื้นฟู Stamina

        speedTween?.Kill();
        speedTween = DOTween.To(() => currentSpeed, x => currentSpeed = x, targetSpeed, smoothDuration)
                          .SetEase(Ease.Linear);

        animator.SetBool("isWalking", isMoving);
        animator.SetBool("isRunning", isRunning);

        if (targetSpeed > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (targetSpeed < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            rb.velocity = new Vector2(currentSpeed, rb.velocity.y);
        }
    }

    void OnDestroy()
    {
        speedTween?.Kill();
    }

    private void HandleAudio()
    {
        if (isMoving && !isRunning)
        {
            if (!walkAudioSource.isPlaying)
            {
                walkAudioSource.Play();
            }
            runAudioSource.Stop();
        }
        else if (isRunning)
        {
            if (!runAudioSource.isPlaying)
            {
                runAudioSource.Play();
            }
            walkAudioSource.Stop();
        }
        else
        {
            walkAudioSource.Stop();
            runAudioSource.Stop();
        }
    }

    private void HandleStaminaRegenAudio()
    {
        // ตรวจสอบว่า Stamina กำลังเพิ่มขึ้น และต้องไม่เล่นเสียงซ้ำ
        if (staminaBar.currentStamina > previousStamina && staminaBar.currentStamina < staminaBar.maxStamina)
        {
            if (!staminaRegenAudioSource.isPlaying)
            {
                staminaRegenAudioSource.Play();
            }
        }
        else
        {
            staminaRegenAudioSource.Stop();
        }

        previousStamina = staminaBar.currentStamina; // อัปเดตค่าเก่า
    }

    public void SetCanMove(bool canMove)
    {
        this.canMove = canMove;
        if (!canMove)
        {
            rb.velocity = Vector2.zero;
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
        }
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerHealth : MonoBehaviour
{
    [Header("Heart UI Elements")]
    public Image[] hearts;
    public Sprite heartFull;
    public Sprite heartEmpty;

    [Header("Health Settings")]
    public int maxHealth = 5;
    public int currentHealth;

    [Header("Animation Settings")]
    public float animationDuration = 0.2f;
    public Vector3 animationScale = new Vector3(1.5f, 1.5f, 1);

    [Header("Screen Flash Settings")]
    public Image screenFlash;
    public Color flashColor = Color.red;
    public float flashDuration3Hearts = 1.0f;
    public float flashDuration2Hearts = 0.5f;
    public float flashDuration1Heart = 0.25f;
    public float alpha3Hearts = 0.5f;
    public float alpha2Hearts = 0.75f;
    public float alpha1Heart = 1.0f;

    [Header("Game Over Manager")]
    public GameOverManager gameOverManager;

    [Header("Audio Settings")]
    public AudioSource audioSource;  // AudioSource ที่ใช้ในการเล่นเสียง
    public AudioClip damageSound;  // เสียงที่เล่นเมื่อเสียเลือด

    private Animator animator;
    private bool isFlashing = false;
    private Tween currentFlashTween;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHearts();

        if (gameOverManager == null)
        {
            Debug.LogError("GameOverManager is not assigned in the inspector.");
        }

        animator = GetComponent<Animator>();

        if (screenFlash != null)
        {
            screenFlash.color = new Color(flashColor.r, flashColor.g, flashColor.b, 0);
        }
    }

    public void TakeDamage(int amount)
    {
        int previousHealth = currentHealth;
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHearts();

        // เล่นเอฟเฟกต์ทั้งหมดเมื่อโดนโจมตี
        AnimateHeartsChange(previousHealth, currentHealth, isDamage: true);

        // เริ่มอนิเมชัน Hurt หลังจากโดนโจมตี
        animator.SetTrigger("Hurt");

        // เล่นเสียงเมื่อเสียเลือด
        if (audioSource != null && damageSound != null)
        {
            audioSource.PlayOneShot(damageSound);  // เล่นเสียง
        }

        // กระพริบขอบจอตาม HP
        if (currentHealth <= 3 && !isFlashing)
        {
            StartFlashingScreen();
        }
        else if (currentHealth > 3 && isFlashing)
        {
            StopFlashingScreen();
        }

        if (currentHealth <= 0)
        {
            StartCoroutine(HandleDeath());
        }
    }

    IEnumerator HandleDeath()
    {
        animator.SetTrigger("Die");
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Die")
            && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f);

        GameOver();
    }

    public void Heal(int amount)
    {
        int previousHealth = currentHealth;
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHearts();
        AnimateHeartsChange(previousHealth, currentHealth, isDamage: false);

        if (currentHealth > 3 && isFlashing)
        {
            StopFlashingScreen();
        }
    }

    void AnimateHeartsChange(int previousHealth, int currentHealth, bool isDamage)
    {
        if (isDamage && currentHealth < previousHealth)
        {
            for (int i = currentHealth; i < previousHealth; i++)
            {
                AnimateHeart(hearts[i]);
            }
        }
        else if (!isDamage && currentHealth > previousHealth)
        {
            for (int i = previousHealth; i < currentHealth; i++)
            {
                AnimateHeart(hearts[i]);
            }
        }
    }

    void AnimateHeart(Image heart)
    {
        Vector3 originalScale = heart.transform.localScale;
        Vector3 targetScale = originalScale * animationScale.x;
        heart.transform.DOScale(targetScale, animationDuration).SetLoops(2, LoopType.Yoyo);
    }

    void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
            {
                hearts[i].sprite = heartFull;
            }
            else
            {
                hearts[i].sprite = heartEmpty;
            }
        }
    }

    void StartFlashingScreen()
    {
        isFlashing = true;
        float flashDuration = flashDuration3Hearts;
        float alpha = alpha3Hearts;

        if (currentHealth == 2)
        {
            flashDuration = flashDuration2Hearts;
            alpha = alpha2Hearts;
        }
        else if (currentHealth == 1)
        {
            flashDuration = flashDuration1Heart;
            alpha = alpha1Heart;
        }

        if (screenFlash != null)
        {
            currentFlashTween = screenFlash.DOFade(alpha, flashDuration)
                                .SetLoops(-1, LoopType.Yoyo);
        }
    }

    void StopFlashingScreen()
    {
        isFlashing = false;
        if (currentFlashTween != null)
        {
            currentFlashTween.Kill();
        }
        screenFlash.color = new Color(flashColor.r, flashColor.g, flashColor.b, 0);
    }

    void GameOver()
    {
        if (gameOverManager != null)
        {
            gameOverManager.ShowGameOver();
        }
        else
        {
            Debug.LogError("GameOverManager reference is missing.");
        }
    }
}

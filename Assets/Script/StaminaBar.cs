using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class StaminaBar : MonoBehaviour
{
    public Image staminaFill;
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaDrainRate = 10f;
    public float staminaRegenRate = 5f;
    public float fillDuration = 0.5f;
    public float colorDuration = 0.5f;

    public PlayerMovement playerMovement;

    private bool isRunning;
    private bool isFlashing;

    // ตัวแปรใหม่สำหรับการคงที่สแตมินา
    private bool isFrozen;
    private float frozenStaminaValue;

    private Tween fillTween;
    private Tween colorTween;
    private Tween flashingTween;

    // อ้างอิงไปยัง StaminaOverlay
    public StaminaOverlay staminaOverlay;

    void Start()
    {
        currentStamina = maxStamina;
        staminaFill.color = Color.yellow;
        staminaFill.fillAmount = currentStamina / maxStamina;

        if (staminaOverlay == null)
        {
            staminaOverlay = GetComponentInChildren<StaminaOverlay>();
        }
    }

    void Update()
    {
        if (isFrozen)
        {
            // ในขณะคงที่สแตมินา ไม่ทำการลดหรือเพิ่มสแตมินา
            currentStamina = frozenStaminaValue;
        }
        else
        {
            // ลอจิกการลดและเพิ่มสแตมินาแบบเดิม
            if (Input.GetKey(KeyCode.LeftShift) && playerMovement.isMoving)
            {
                if (currentStamina > 0)
                {
                    isRunning = true;
                    DrainStamina();
                }
                else
                {
                    isRunning = false;
                }
            }
            else
            {
                isRunning = false;
                RegenerateStamina();
            }
        }

        // อัปเดต UI และสี
        UpdateStaminaUI();
        UpdateStaminaColor();

        // แจ้งสถานะการวิ่งให้กับ PlayerMovement
        playerMovement.isRunning = currentStamina > 0 && isRunning;
    }

    void DrainStamina()
    {
        if (currentStamina > 0)
        {
            currentStamina -= staminaDrainRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        }
        else
        {
            isRunning = false;
        }
    }

    void RegenerateStamina()
    {
        if (currentStamina < maxStamina && !isRunning)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        }
    }

    void UpdateStaminaUI(bool immediate = false)
    {
        float targetFillAmount = currentStamina / maxStamina;

        if (immediate)
        {
            staminaFill.fillAmount = targetFillAmount;
            return;
        }

        fillTween?.Kill();
        fillTween = staminaFill.DOFillAmount(targetFillAmount, fillDuration).SetEase(Ease.Linear);
    }

    void UpdateStaminaColor()
    {
        float staminaPercentage = (currentStamina / maxStamina) * 100f;

        if (staminaPercentage <= 20f)
        {
            if (!isFlashing)
            {
                StartFlashingRed();
            }
        }
        else
        {
            if (isFlashing)
            {
                StopFlashingRed();
            }

            SetStaminaColor(Color.yellow);
        }
    }

    void StartFlashingRed()
    {
        isFlashing = true;

        flashingTween?.Kill();
        flashingTween = staminaFill.DOColor(new Color(1, 0, 0, 1), colorDuration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.Linear);
    }

    void StopFlashingRed()
    {
        isFlashing = false;

        flashingTween?.Kill();
        SetStaminaColor(Color.yellow, immediate: true);
    }

    void SetStaminaColor(Color newColor, bool immediate = false)
    {
        if (immediate)
        {
            staminaFill.color = newColor;
            colorTween?.Kill();
        }
        else
        {
            colorTween?.Kill();
            colorTween = staminaFill.DOColor(newColor, colorDuration).SetEase(Ease.Linear);
        }
    }

    // เมธอดใหม่สำหรับการคงที่สแตมินา
    public void FreezeStamina(float duration)
    {
        if (!isFrozen)
        {
            StartCoroutine(FreezeStaminaCoroutine(duration));
        }
    }

    private IEnumerator FreezeStaminaCoroutine(float duration)
    {
        isFrozen = true;
        frozenStaminaValue = currentStamina; // เก็บค่าสแตมินาปัจจุบัน

        // แสดง Overlay Animation
        staminaOverlay.StartOverlay();

        // คงที่สแตมินาตลอดระยะเวลาที่กำหนด
        while (duration > 0)
        {
            currentStamina = frozenStaminaValue;
            duration -= Time.deltaTime;
            yield return null;
        }

        isFrozen = false;

        // หยุด Overlay Animation
        staminaOverlay.StopOverlay();
    }

    void OnDestroy()
    {
        fillTween?.Kill();
        colorTween?.Kill();
        flashingTween?.Kill();
    }
}

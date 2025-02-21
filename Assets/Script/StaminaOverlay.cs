using UnityEngine;
using UnityEngine.UI;

public class StaminaOverlay : MonoBehaviour
{
    public Image overlayImage; // ลิงก์ไปยัง Image ของ Overlay
    public float animationDuration = 1f; // ระยะเวลาสำหรับแต่ละวัฏจักรของ Animation
    public float targetAlpha = 0.5f; // ค่าสี Alpha ที่ต้องการ

    private bool isAnimating = false;
    private float elapsedTime = 0f;
    private bool isFadingOut = false;

    void Start()
    {
        if (overlayImage == null)
        {
            overlayImage = GetComponent<Image>();
        }
        // เริ่มต้นให้ Overlay ซ่อนอยู่
        overlayImage.color = new Color(overlayImage.color.r, overlayImage.color.g, overlayImage.color.b, 0f);
    }

    void Update()
    {
        if (isAnimating)
        {
            // คำนวณเวลาที่ผ่านไปในแต่ละ frame
            elapsedTime += Time.deltaTime;

            // คำนวณ progress ของการ fade
            float progress = elapsedTime / animationDuration;

            if (!isFadingOut)
            {
                // Fade In
                float alpha = Mathf.Lerp(0f, targetAlpha, progress);
                overlayImage.color = new Color(overlayImage.color.r, overlayImage.color.g, overlayImage.color.b, alpha);
                if (progress >= 1f)
                {
                    isFadingOut = true;
                    elapsedTime = 0f;
                }
            }
            else
            {
                // Fade Out
                float alpha = Mathf.Lerp(targetAlpha, 0f, progress);
                overlayImage.color = new Color(overlayImage.color.r, overlayImage.color.g, overlayImage.color.b, alpha);
                if (progress >= 1f)
                {
                    isFadingOut = false;
                    elapsedTime = 0f;
                }
            }
        }
    }

    // เริ่ม Animation Overlay
    public void StartOverlay()
    {
        isAnimating = true;
        elapsedTime = 0f;
        isFadingOut = false;
    }

    // หยุด Animation Overlay
    public void StopOverlay()
    {
        isAnimating = false;
        // ตั้งค่า Alpha กลับเป็น 0 ทันที
        overlayImage.color = new Color(overlayImage.color.r, overlayImage.color.g, overlayImage.color.b, 0f);
    }
}

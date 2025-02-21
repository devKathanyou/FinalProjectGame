using UnityEngine;

public class FadeSprite : MonoBehaviour
{
    public SpriteRenderer targetSprite; // ใส่ Sprite ที่ต้องการให้จางลง
    public float fadeToAlpha = 0.5f; // ค่า Alpha ที่ต้องการ (0 = โปร่งใส, 1 = ทึบ)
    public float fadeSpeed = 2f; // ความเร็วในการจางลง

    private float originalAlpha; // ค่า Alpha เริ่มต้น
    private bool isFading = false;

    void Start()
    {
        if (targetSprite != null)
        {
            originalAlpha = targetSprite.color.a;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // ตรวจจับเฉพาะ Player
        {
            isFading = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isFading = false;
        }
    }

    void Update()
    {
        if (targetSprite != null)
        {
            Color color = targetSprite.color;
            if (isFading)
            {
                color.a = Mathf.Lerp(color.a, fadeToAlpha, fadeSpeed * Time.deltaTime);
            }
            else
            {
                color.a = Mathf.Lerp(color.a, originalAlpha, fadeSpeed * Time.deltaTime);
            }
            targetSprite.color = color;
        }
    }
}

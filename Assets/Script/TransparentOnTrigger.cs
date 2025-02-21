using UnityEngine;
using DG.Tweening; // เพิ่ม namespace ของ DOTween

public class TransparentOnTrigger : MonoBehaviour
{
    public SpriteRenderer frontSprite; // Sprite ด้านหน้าที่จะเปลี่ยนความโปร่งใส
    public float transparentAlpha = 0.5f; // ระดับความโปร่งใส (0 = โปร่งใสเต็ม, 1 = ทึบเต็ม)
    public float fadeDuration = 0.5f; // ระยะเวลาในการเปลี่ยนความโปร่งใส

    private float originalAlpha; // เก็บค่า Alpha เดิมของ Sprite

    private void Start()
    {
        if (frontSprite != null)
        {
            originalAlpha = frontSprite.color.a; // เก็บค่า Alpha เดิม
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // ตรวจสอบว่าผู้เล่นชน Collider
        {
            if (frontSprite != null)
            {
                frontSprite.DOFade(transparentAlpha, fadeDuration); // ใช้ DOTween ทำการ Fade
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // ตรวจสอบว่าผู้เล่นออกจาก Collider
        {
            if (frontSprite != null)
            {
                frontSprite.DOFade(originalAlpha, fadeDuration); // ใช้ DOTween ทำการ Fade กลับ
            }
        }
    }
}

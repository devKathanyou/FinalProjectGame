using UnityEngine;
using DG.Tweening; // ต้องใช้ namespace ของ DOTween

public class ShowIconWithDOTween : MonoBehaviour
{
    public GameObject icon; // อ้างอิงไอคอน
    private Tween scaleTween; // เก็บ Tween ของการขยาย/หด

    private void Start()
    {
        if (icon != null)
        {
            icon.SetActive(false); // เริ่มต้นโดยการซ่อนไอคอน
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // ตรวจสอบว่าผู้เล่นเข้าใกล้
        {
            if (icon != null)
            {
                icon.SetActive(true); // แสดงไอคอน
                StartScaleAnimation(); // เริ่มต้นเอฟเฟกต์ขยาย/หด
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // ตรวจสอบว่าผู้เล่นออกห่าง
        {
            if (icon != null)
            {
                StopScaleAnimation(); // หยุดเอฟเฟกต์
                icon.SetActive(false); // ซ่อนไอคอน
            }
        }
    }

    private void StartScaleAnimation()
    {
        if (icon != null)
        {
            // รีเซ็ตขนาดเริ่มต้นก่อนเริ่ม Tween
            icon.transform.localScale = Vector3.one;

            // เริ่ม Tween ขยายและหดวนซ้ำ
            scaleTween = icon.transform
                .DOScale(new Vector3(0.8f, 0.8f, 0.8f), 1f) // ขยายใน 0.5 วินาที
                .SetLoops(-1, LoopType.Yoyo) // วนซ้ำตลอดไปแบบ Yoyo
                .SetEase(Ease.InOutSine); // ใช้ Ease เพื่อการเคลื่อนไหวที่นุ่มนวล
        }
    }

    private void StopScaleAnimation()
    {
        if (scaleTween != null)
        {
            scaleTween.Kill(); // หยุด Tween
            icon.transform.localScale = Vector3.one; // รีเซ็ตขนาดกลับเป็นปกติ
        }
    }
}

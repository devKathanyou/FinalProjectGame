using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditScroll : MonoBehaviour
{
    public RectTransform creditText;
    public float scrollSpeed = 50f;

    void Update()
    {
        // ทำให้เครดิตเลื่อนขึ้น
        creditText.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);
    }

    public void SkipCredits()
    {
        // เปลี่ยนไปหน้าเมนูหลัก
        SceneManager.LoadScene("MainMenu");
    }
}

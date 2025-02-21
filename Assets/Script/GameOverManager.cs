using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverManager : MonoBehaviour
{
    [Header("Game Over UI")]
    public GameObject gameOverPanel; // อ้างอิงไปที่ UI เกมโอเวอร์
    public Button restartButton;      // ปุ่มรีสตาร์ท

    [Header("Animation Settings")]
    public float animationDuration = 0.2f; // ระยะเวลาแอนิเมชัน
    public Vector3 animationScale = new Vector3(1.2f, 1.2f, 1); // ขนาดที่ต้องการในการแอนิเมชัน

    void Start()
    {
        gameOverPanel.SetActive(false); // ซ่อน UI เกมโอเวอร์เริ่มต้น
        restartButton.onClick.AddListener(RestartGame); // ตั้งค่าฟังก์ชันรีสตาร์ทเมื่อคลิกปุ่ม
    }

    // แสดงหน้าจอ Game Over พร้อมกับแอนิเมชัน
    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true); // แสดง UI เกมโอเวอร์
        AnimateGameOverPanel();
        Time.timeScale = 0; // หยุดเวลาของเกม
    }

    // ฟังก์ชันสำหรับรีสตาร์ทเกม
    void RestartGame()
    {
        Time.timeScale = 1; // เริ่มเวลาของเกมใหม่
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // โหลดฉากปัจจุบันใหม่
    }

    // Coroutine สำหรับแอนิเมชันของ Game Over Panel (ถ้าต้องการ)
    void AnimateGameOverPanel()
    {
        // หากต้องการเพิ่มแอนิเมชันเพิ่มเติม สามารถเพิ่มโค้ดที่นี่ได้
        // เช่น การขยายและย่อของ panel
        // ตัวอย่าง:
        /*
        StartCoroutine(ScaleAnimation(gameOverPanel.transform));
        */
    }

    // ตัวอย่าง Coroutine สำหรับแอนิเมชันสเกล (ไม่จำเป็นต้องใช้)
    IEnumerator ScaleAnimation(Transform target)
    {
        Vector3 originalScale = target.localScale;
        Vector3 targetScale = originalScale * 1.2f;
        float elapsed = 0f;

        // ขยายขึ้น
        while (elapsed < animationDuration)
        {
            target.localScale = Vector3.Lerp(originalScale, targetScale, elapsed / animationDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        target.localScale = targetScale;

        // ย่อกลับลง
        elapsed = 0f;
        while (elapsed < animationDuration)
        {
            target.localScale = Vector3.Lerp(targetScale, originalScale, elapsed / animationDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        target.localScale = originalScale;
    }
}

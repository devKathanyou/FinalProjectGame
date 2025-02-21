using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    [System.Serializable]
    public class TutorialStep
    {
        public string id;                     // ไอดีของ Tutorial (เช่น "movement", "hide", "useItem")
        public string message;                // ข้อความที่ต้องการแสดง
        public float displayTime = 3f;        // เวลาที่จะแสดงข้อความ
        public GameObject tutorialPanel;      // Panel ที่จะใช้แสดงข้อความ
        public TMP_Text tutorialText;         // ข้อความใน Panel
        public CanvasGroup canvasGroup;       // ใช้สำหรับการทำ Fade ของ Panel
    }

    public List<TutorialStep> tutorials;     // ลิสต์ของ Tutorial Steps

    private HashSet<string> shownTutorials = new HashSet<string>();
    private Coroutine currentCoroutine;

    void Awake()
    {
        // ตรวจสอบว่าเป็น Singleton หรือไม่
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowTutorial(string id)
    {
        // ถ้า Tutorial นี้เคยแสดงแล้ว จะไม่แสดงซ้ำ
        if (shownTutorials.Contains(id)) return;

        TutorialStep step = tutorials.Find(t => t.id == id);
        if (step != null)
        {
            StartCoroutine(DisplayTutorial(step));
            shownTutorials.Add(id);
        }
        else
        {
            Debug.LogWarning("Tutorial ID not found: " + id);
        }
    }

    private IEnumerator DisplayTutorial(TutorialStep step)
    {
        if (step.tutorialPanel != null && step.tutorialText != null && step.canvasGroup != null)
        {
            step.tutorialText.text = step.message;

            // ทำการเฟดเข้า
            yield return StartCoroutine(FadeIn(step.canvasGroup, 0.5f)); // ระยะเวลาเฟดเข้า 0.5 วินาที

            yield return new WaitForSeconds(step.displayTime); // แสดงผลตามเวลาที่กำหนด

            // ทำการเฟดออก
            yield return StartCoroutine(FadeOut(step.canvasGroup, 0.5f)); // ระยะเวลาเฟดออก 0.5 วินาที

            step.tutorialPanel.SetActive(false); // ปิด Panel หลังเฟดออก
        }
        else
        {
            Debug.LogWarning("TutorialPanel, TutorialText, or CanvasGroup is not assigned for Tutorial ID: " + step.id);
        }
    }

    // ฟังก์ชันสำหรับการเฟดเข้า
    private IEnumerator FadeIn(CanvasGroup canvasGroup, float duration)
    {
        float elapsed = 0f;
        canvasGroup.alpha = 0f; // เริ่มจากความโปร่งใส 0
        canvasGroup.gameObject.SetActive(true); // แสดง Panel

        while (elapsed < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / duration); // ค่อยๆ ปรับค่า alpha จาก 0 ไป 1
            elapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1f; // ตั้งค่า alpha ให้เป็น 1 เมื่อเฟดเข้าเสร็จสิ้น
    }

    // ฟังก์ชันสำหรับการเฟดออก
    private IEnumerator FadeOut(CanvasGroup canvasGroup, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / duration); // ค่อยๆ ปรับค่า alpha จาก 1 ไป 0
            elapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0f; // ตั้งค่า alpha ให้เป็น 0 เมื่อเฟดออกเสร็จสิ้น
        canvasGroup.gameObject.SetActive(false); // ปิด Panel หลังเฟดออก
    }
}

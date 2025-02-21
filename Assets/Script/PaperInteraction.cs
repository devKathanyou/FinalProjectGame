using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

public class PaperInteraction : MonoBehaviour
{
    public GameObject messagePanel; // Panel แสดงข้อความ
    public Image messageImage; // รูปภาพใน Panel
    public Sprite paperSprite; // รูปภาพของกระดาษ
    public Light2D paperLight; // แสงสำหรับเน้นกระดาษ
    public GameObject interactIcon; // ไอคอนแสดงเมื่อเข้าใกล้

    private bool isPlayerNearby = false;

    void Start()
    {
        // ปิดแสงและไอคอนเมื่อเริ่มเกม
        if (paperLight != null) paperLight.enabled = false;
        if (interactIcon != null) interactIcon.SetActive(false);
    }

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.F))
        {
            ToggleMessagePanel();
        }
    }

    private void ToggleMessagePanel()
    {
        bool isActive = messagePanel.activeSelf;
        messagePanel.SetActive(!isActive);

        if (isActive)
        {
            Time.timeScale = 1f;
        }
        else
        {
            Time.timeScale = 0f;
            messageImage.sprite = paperSprite;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNearby = true;

            if (paperLight != null) paperLight.enabled = true;
            if (interactIcon != null) interactIcon.SetActive(true); // เปิดไอคอน
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNearby = false;

            if (paperLight != null) paperLight.enabled = false;
            if (interactIcon != null) interactIcon.SetActive(false); // ปิดไอคอน
        }
    }
}

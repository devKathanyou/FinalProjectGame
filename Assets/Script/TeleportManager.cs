using System.Collections;
using UnityEngine;

public class TeleportManager : MonoBehaviour
{
    public FadeManager fadeManager;
    public Transform player;       // ลาก Player ลงใน Inspector
    public Vector3 newPosition;    // ตำแหน่งที่ต้องการย้ายผู้เล่นไป
    public GameObject interactionIcon; // ลาก GameObject ไอคอนลงใน Inspector

    private bool isTeleporting = false;  // ป้องกันการทำงานซ้ำ
    private bool isPlayerInTrigger = false;  // ตรวจสอบว่าผู้เล่นอยู่ใน Trigger หรือไม่
    private PlayerMovement playerMovement;   // อ้างอิงไปที่ระบบการเคลื่อนไหวของผู้เล่น

    private void Start()
    {
        // ค้นหา PlayerMovement ในผู้เล่น
        playerMovement = player.GetComponent<PlayerMovement>();  // สมมติว่าผู้เล่นมีคอมโพเนนต์ PlayerMovement

        // ซ่อนไอคอนเมื่อเริ่มเกม
        if (interactionIcon != null)
        {
            interactionIcon.SetActive(false);
        }
    }

    private void Update()
    {
        // ตรวจสอบว่าผู้เล่นอยู่ใน Trigger และกดปุ่ม E หรือไม่
        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.E) && !isTeleporting)
        {
            StartCoroutine(TeleportSequence());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInTrigger = true;
            if (interactionIcon != null)
            {
                interactionIcon.SetActive(true); // แสดงไอคอน
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
            if (interactionIcon != null)
            {
                interactionIcon.SetActive(false); // ซ่อนไอคอน
            }
        }
    }

    private IEnumerator TeleportSequence()
    {
        isTeleporting = true;  // ป้องกันการเรียกใช้ซ้ำ

        // ปิดการเคลื่อนไหวของผู้เล่น
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        // ทำให้หน้าจอมืดลง
        yield return StartCoroutine(fadeManager.FadeToBlack());

        // ย้ายผู้เล่นไปยังตำแหน่งใหม่
        player.position = newPosition;

        // ทำให้หน้าจอสว่างขึ้น
        yield return StartCoroutine(fadeManager.FadeToClear());

        // เปิดการเคลื่อนไหวของผู้เล่น
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }

        isTeleporting = false;  // รีเซ็ตสถานะเมื่อการย้ายเสร็จสิ้น
    }
}

using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    [Header("Key and Door Settings")]
    public string keyID; // ระบุ ID ของกุญแจที่เปิดประตูนี้
    public Transform warpDestination; // จุดวาร์ปที่ผู้เล่นจะย้ายไปเมื่อผ่านประตูนี้
    public Transform warpExit; // จุดวาร์ปขาออก (ถ้าประตูมีขาเข้าและขาออก)

    [Header("Door Appearance")]
    public SpriteRenderer doorSpriteRenderer; // ตัวแปร SpriteRenderer ของประตู
    public Color enterColor = new Color(1, 1, 1, 0.5f); // สีจางเมื่อเข้าประตู
    public Color exitColor = new Color(1, 1, 1, 1); // สีสว่างเมื่อออกประตู

    [Header("Player Layer Settings")]
    public int playerOrderInLayerWhenEnter = 0; // Order in Layer เมื่อเข้าประตู
    public int playerOrderInLayerWhenExit = 2; // Order in Layer เมื่อออกประตู

    [Header("Lock and Open Settings")]
    public bool isLocked = true; // สถานะว่าประตูล็อกหรือไม่ (true คือ ล็อก)
    private bool isPlayerNear = false; // ตรวจสอบว่าผู้เล่นอยู่ใกล้ประตูหรือไม่
    private bool isOpen = false; // ตรวจสอบว่าประตูเปิดอยู่หรือไม่
    private bool isUnlocked = false; // ตรวจสอบว่าประตูถูกปลดล็อกแล้วหรือไม่

    [Header("Fade Settings")]
    public CanvasGroup fadeCanvasGroup; // ตัวแปร CanvasGroup สำหรับการทำ Fade
    public float fadeDuration = 1f; // ระยะเวลาสำหรับการทำ Fade

    [Header("Door Icons")]
    public GameObject lockIcon; // ไอคอนประตูล็อก
    public GameObject enterIcon; // ไอคอนประตูเปิดหรือสามารถเข้าถึงได้

    private Animator animator; // ตัวแปร Animator

    public AudioSource doorSound; // ตัวแปรสำหรับ AudioSource
    public AudioClip openSound; // เสียงเปิดประตู
    public AudioClip closeSound; // เสียงปิดประตู

    private Vector3 originalPosition; // ตำแหน่งเดิมของประตู (ก่อนผู้เล่นจะผ่านเข้าไป)

    private void Start()
    {
        animator = GetComponent<Animator>(); // อ้างอิง Animator จากประตู
        originalPosition = transform.position; // บันทึกตำแหน่งเริ่มต้นของประตู
        UpdateDoorIcons(); // ตั้งค่าไอคอนเริ่มต้นตามสถานะของประตู
    }

    private void Update()
    {
        if (isPlayerNear)
        {
            if (Input.GetKeyDown(KeyCode.F)) // กด F เพื่อเปิดหรือปิดประตู
            {
                TryOpenDoor();
            }

            if (Input.GetKeyDown(KeyCode.E) && isOpen) // กด E เพื่อวาร์ปเมื่อประตูเปิดอยู่
            {
                StartCoroutine(WarpWithFade()); // ทำการวาร์ปพร้อมกับเอฟเฟกต์ Fade
            }
        }
    }

    private void TryOpenDoor()
    {
        if (!isLocked || isUnlocked)
        {
            ToggleDoor();
            return;
        }

        InventorySystem inventory = InventorySystem.Instance;

        for (int i = 0; i < inventory.inventory.Length; i++)
        {
            Item currentItem = inventory.inventory[i];
            if (currentItem != null && currentItem.isKey && currentItem.keyID == keyID)
            {
                Debug.Log("เปิดประตูแล้ว!");

                isUnlocked = true;
                isLocked = false;
                ToggleDoor();

                inventory.RemoveItem(currentItem);
                UpdateDoorIcons();
                return;
            }
        }

        Debug.Log("ไม่มีกุญแจที่ถูกต้อง");
    }

    private void ToggleDoor()
    {
        isOpen = !isOpen;
        animator.SetBool("isOpen", isOpen);
        UpdateDoorIcons();

        if (isOpen && doorSound != null && openSound != null)
        {
            doorSound.PlayOneShot(openSound);
        }
        else if (!isOpen && doorSound != null && closeSound != null)
        {
            doorSound.PlayOneShot(closeSound);
        }
    }

    private IEnumerator WarpWithFade()
    {
        yield return StartCoroutine(Fade(1f));
        WarpPlayer();
        yield return StartCoroutine(Fade(0f));
    }

    private void WarpPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            SpriteRenderer playerSpriteRenderer = player.GetComponent<SpriteRenderer>();

            if (player.transform.position == warpDestination.position)
            {
                player.transform.position = warpExit.position;
                ChangeDoorColor(exitColor);
                transform.position = originalPosition; // คืนตำแหน่งของประตูกลับไปที่เดิม

                if (playerSpriteRenderer != null)
                {
                    playerSpriteRenderer.sortingOrder = playerOrderInLayerWhenExit;
                }
            }
            else
            {
                player.transform.position = warpDestination.position;
                ChangeDoorColor(enterColor);
                transform.position = player.transform.position; // ปรับตำแหน่งของประตูให้ตรงกับตำแหน่งของผู้เล่น

                if (playerSpriteRenderer != null)
                {
                    playerSpriteRenderer.sortingOrder = playerOrderInLayerWhenEnter;
                }
            }

            Debug.Log("วาร์ปผู้เล่นแล้ว!");
        }
        else
        {
            Debug.LogError("ไม่พบผู้เล่นในฉาก!");
        }
    }

    private IEnumerator Fade(float targetAlpha)
    {
        if (fadeCanvasGroup != null)
        {
            float startAlpha = fadeCanvasGroup.alpha;
            float elapsedTime = 0f;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
                yield return null;
            }

            fadeCanvasGroup.alpha = targetAlpha;
        }
    }

    private void ChangeDoorColor(Color newColor)
    {
        if (doorSpriteRenderer != null)
        {
            doorSpriteRenderer.color = newColor;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNear = true;
            Debug.Log("อยู่ใกล้ประตู กด F เพื่อเปิดหรือปิด หรือกด E เพื่อวาร์ป");
            UpdateDoorIcons(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNear = false;
            UpdateDoorIcons(false);
        }
    }

    private void UpdateDoorIcons(bool show = false)
    {
        if (show)
        {
            if (isLocked)
            {
                lockIcon?.SetActive(true);
                enterIcon?.SetActive(false);
            }
            else
            {
                lockIcon?.SetActive(false);
                enterIcon?.SetActive(true);
            }
        }
        else
        {
            lockIcon?.SetActive(false);
            enterIcon?.SetActive(false);
        }
    }
}

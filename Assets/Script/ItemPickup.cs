using UnityEngine;
using UnityEngine.Rendering.Universal; // ใช้สำหรับ Light2D

public class ItemPickup : MonoBehaviour
{
    public Item item;
    public Light2D itemLight; // อ้างอิงไปยัง Light2D ของไอเท็ม
    public float lightBlinkSpeed = 2f; // ความเร็วในการกระพริบ
    public AudioClip pickupSound; // คลิปเสียงตอนเก็บไอเท็ม
    private AudioSource audioSource; // ตัวเล่นเสียง
    private bool isPlayerNearby = false;

    void Start()
    {
        // ซ่อนแสงเมื่อเริ่มเกม
        HideItemLight();

        // กำหนด AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // เพิ่ม AudioSource หากไม่มีใน GameObject
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (isPlayerNearby)
        {
            // ทำให้แสงกระพริบ
            BlinkItemLight();
        }

        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            if (InventorySystem.Instance.AddItem(item))
            {
                // เล่นเสียง
                if (pickupSound != null)
                {
                    audioSource.PlayOneShot(pickupSound);
                }

                Destroy(gameObject, pickupSound.length); // ทำลาย object หลังเสียงเล่นจบ
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            ShowItemLight(); // แสดงแสงเมื่อผู้เล่นอยู่ใกล้
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            HideItemLight(); // ซ่อนแสงเมื่อผู้เล่นออกห่าง
        }
    }

    public void ShowItemLight()
    {
        if (itemLight != null)
            itemLight.enabled = true; // เปิดแสง
    }

    public void HideItemLight()
    {
        if (itemLight != null)
            itemLight.enabled = false; // ปิดแสง
    }

    private void BlinkItemLight()
    {
        if (itemLight != null)
        {
            // ทำให้ความสว่างของแสงกระพริบโดยใช้ Mathf.PingPong
            float intensity = Mathf.PingPong(Time.time * lightBlinkSpeed, 1f);
            itemLight.intensity = intensity;
        }
    }
}

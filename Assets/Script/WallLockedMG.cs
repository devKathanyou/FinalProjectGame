using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallLockedMG : MonoBehaviour
{
    public WallLocked door; // ลากประตูเข้ามาใน Inspector

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("คุณเจอกุญแจแล้ว!");
            door.isLocked = false; // ปลดล็อกประตู
            //Destroy(gameObject); // ทำลายไอเท็มกุญแจหลังจากเก็บ
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallLocked : MonoBehaviour
{
    public bool isLocked = true; // สถานะเริ่มต้นของประตู

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (isLocked)
            {
                Debug.Log("ประตูล็อกอยู่! คุณไม่สามารถผ่านได้");
                // คุณสามารถเพิ่มระบบแจ้งเตือนบนหน้าจอได้ที่นี่
            }
            else
            {
                Debug.Log("ประตูถูกปลดล็อก คุณสามารถผ่านได้!");
            }
        }
    }
}


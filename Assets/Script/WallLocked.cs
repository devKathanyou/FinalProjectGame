using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallLocked : MonoBehaviour
{
    public bool isLocked = true; // ʶҹ�������鹢ͧ��е�

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (isLocked)
            {
                Debug.Log("��е���͡����! �س�������ö��ҹ��");
                // �س����ö�����к�����͹��˹�Ҩ�������
            }
            else
            {
                Debug.Log("��еٶ١�Ŵ��͡ �س����ö��ҹ��!");
            }
        }
    }
}


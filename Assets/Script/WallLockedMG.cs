using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallLockedMG : MonoBehaviour
{
    public WallLocked door; // �ҡ��е������� Inspector

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("�س�͡ح�����!");
            door.isLocked = false; // �Ŵ��͡��е�
            //Destroy(gameObject); // �����������ح���ѧ�ҡ��
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class BombTrigger : MonoBehaviour
{
    private bool isPlayerInRange = false;
    private bool bombPlaced = false;

    void Update()
    {
        // ถ้าผู้เล่นอยู่ในระยะ และกดปุ่ม F จะวางระเบิด
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F) && !bombPlaced)
        {
            PlaceBomb();
        }
    }

    private void PlaceBomb()
    {
        bombPlaced = true;
        Debug.Log("Bomb Placed!");

        // แสดงเอฟเฟกต์หรือเสียง (ถ้ามี)
        // สามารถเพิ่มอนิเมชันระเบิดได้ตรงนี้

        // เปลี่ยนไปที่หน้า EndCredits หลังจาก 3 วินาที
        Invoke("GoToEndCredits", 0f);
    }

    private void GoToEndCredits()
    {
        SceneManager.LoadScene("EndCredits");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("Player is ready to place the bomb (Press F)");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }
}

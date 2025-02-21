using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class MainMenu : MonoBehaviour
{
    public GameObject exitPanel;     // Panel สำหรับยืนยันการออกเกม

    public void StartGame()
    {
        SceneManager.LoadScene("Cutscenes"); // เปลี่ยนเป็นชื่อ Scene ของเกม
    }

    public void ShowExitPanel()
    {
        exitPanel.SetActive(true);  // แสดง UI ยืนยัน
    }

    public void HideExitPanel()
    {
        exitPanel.SetActive(false); // ซ่อน UI ยืนยัน
    }

    public void ConfirmExit()
    {
        Application.Quit(); // ออกจากเกม
    }
}

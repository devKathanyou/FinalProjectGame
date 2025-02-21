using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public string tutorialId; // ไอดีของ Tutorial ที่จะเรียกใช้
    public bool triggerOnce = true; // ถ้า true จะ Trigger เพียงครั้งเดียว

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && (!triggerOnce || !hasTriggered))
        {
            TutorialManager.Instance.ShowTutorial(tutorialId);
            if (triggerOnce)
            {
                hasTriggered = true;
            }
        }
    }
}

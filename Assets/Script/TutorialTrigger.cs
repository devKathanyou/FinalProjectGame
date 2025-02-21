using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public string tutorialId; // �ʹբͧ Tutorial �������¡��
    public bool triggerOnce = true; // ��� true �� Trigger ��§��������

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

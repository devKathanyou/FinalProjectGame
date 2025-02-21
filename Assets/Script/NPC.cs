using UnityEngine;

public class NPC : MonoBehaviour
{
    public Quest quest;
    public Dialogue dialogueBeforeAccept;
    public Dialogue dialogueAfterAccept;
    public Dialogue dialogueAfterComplete;
    public Dialogue dialogueNoQuest;

    private bool playerInRange;
    public GameObject interactIcon; // เปลี่ยนจากข้อความเป็นไอคอน

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.T))
        {
            if (QuestUI.Instance.IsQuestPanelActive() || QuestUI.Instance.IsDialogueActive())
            {
                QuestUI.Instance.HideAllPanels();
                OnDialogueEnd();
            }
            else
            {
                HandleDialogue();
            }
        }
    }

    private void HandleDialogue()
    {
        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.SetCanMove(false);
        }

        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            enemy.SetCanMove(false);
        }

        if (quest != null)
        {
            if (QuestManager.Instance.IsQuestCompletedGlobally(quest))
            {
                if (dialogueNoQuest != null)
                {
                    QuestUI.Instance.StartDialogue(dialogueNoQuest, null, null);
                }
                return;
            }

            if (QuestManager.Instance.HasActiveQuest(quest))
            {
                if (QuestManager.Instance.IsQuestCompleted(quest))
                {
                    if (dialogueAfterComplete != null)
                    {
                        QuestUI.Instance.StartDialogue(dialogueAfterComplete, null, quest);
                    }
                }
                else
                {
                    if (dialogueAfterAccept != null)
                    {
                        QuestUI.Instance.StartDialogue(dialogueAfterAccept, null, null);
                    }
                }
            }
            else
            {
                if (dialogueBeforeAccept != null)
                {
                    QuestUI.Instance.StartDialogue(dialogueBeforeAccept, quest, null);
                }
            }
        }
        else
        {
            if (dialogueNoQuest != null)
            {
                QuestUI.Instance.StartDialogue(dialogueNoQuest, null, null);
            }
        }
    }

    private void OnDialogueEnd()
    {
        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.SetCanMove(true);
        }

        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            enemy.SetCanMove(true);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (interactIcon != null)
            {
                interactIcon.SetActive(true); // แสดงไอคอนเมื่อเข้าใกล้
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            QuestUI.Instance.HideAllPanels();
            if (interactIcon != null)
            {
                interactIcon.SetActive(false); // ซ่อนไอคอนเมื่อออกจากระยะ
            }
        }
    }
}

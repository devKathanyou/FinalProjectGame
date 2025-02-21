using TMPro;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class QuestUI : MonoBehaviour
{
    public static QuestUI Instance; // Singleton

    [Header("Quest Panel")]
    public GameObject questPanel;
    public TMP_Text questNameText;
    public TMP_Text questDescriptionText;
    private CanvasGroup questCanvasGroup;

    [Header("Quest Status Panel")]
    public GameObject questStatusPanel;
    public Transform activeQuestsListParent;
    public GameObject questStatusPrefab;
    private CanvasGroup questStatusCanvasGroup;

    [Header("Dialogue Panel")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueNameText;
    public TMP_Text dialogueContentText;
    private CanvasGroup dialogueCanvasGroup;
    public GameObject continueIcon; // เพิ่มบรรทัดนี้

    [Header("Typing Sound")]
    public AudioClip typingSound; // เสียงพิมพ์ข้อความ
    private AudioSource audioSource; // AudioSource สำหรับเล่นเสียง

    private bool isInDialogue = false;
    private string[] currentDialogueLines;
    private int currentDialogueIndex = 0;

    private Quest currentQuestToAccept = null;
    private Quest currentQuestToSubmit = null;

    // การตั้งค่าแอนิเมชัน
    public float animationDuration = 0.5f;
    public float typingSpeed = 0.05f; // ความเร็วในการพิมพ์ข้อความทีละตัวอักษร

    // Tweens สำหรับการจัดการแอนิเมชัน
    private Tween questFadeTween;
    private Tween questStatusFadeTween;
    private Tween dialogueFadeTween;
    private Tween typingTween;

    // ตรวจสอบว่ากำลังพิมพ์ข้อความอยู่หรือไม่
    private bool isTyping = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // เริ่มต้น CanvasGroups
            questCanvasGroup = questPanel.GetComponent<CanvasGroup>();
            questStatusCanvasGroup = questStatusPanel.GetComponent<CanvasGroup>();
            dialogueCanvasGroup = dialoguePanel.GetComponent<CanvasGroup>();

            // เริ่มต้น AudioSource
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;

            // เริ่มต้นแผงเป็นปิดและโปร่งใส
            InitializePanel(questPanel, questCanvasGroup);
            InitializePanel(dialoguePanel, dialogueCanvasGroup);
            InitializeQuestStatusPanel();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializePanel(GameObject panel, CanvasGroup canvasGroup)
    {
        panel.SetActive(false);
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    private void InitializeQuestStatusPanel()
    {
        if (activeQuestsListParent.childCount > 0)
        {
            ShowPanel(questStatusPanel, questStatusCanvasGroup);
        }
        else
        {
            HidePanel(questStatusPanel, questStatusCanvasGroup);
        }
    }

    // แสดง Quest Panel ด้วยแอนิเมชัน fade-in
    public void ShowQuest(Quest quest)
    {
        Debug.Log($"ShowQuest called with Quest: {quest.questName}");
        if (!questPanel.activeSelf)
        {
            ShowPanel(questPanel, questCanvasGroup);
        }
        // ตั้งค่ารายละเอียดเควส
        questNameText.text = quest.questName;
        questDescriptionText.text = quest.description;
    }

    // Overload: แสดง Quest Panel ด้วยชื่อและข้อความที่กำหนดเอง
    public void ShowQuest(string title, string message)
    {
        Debug.Log($"ShowQuest called with Title: {title}, Message: {message}");
        if (!questPanel.activeSelf)
        {
            ShowPanel(questPanel, questCanvasGroup);
        }
        // ตั้งค่ารายละเอียดที่กำหนดเอง
        questNameText.text = title;
        questDescriptionText.text = message;
    }

    // ซ่อน Quest Panel ด้วยแอนิเมชัน fade-out
    public void HideQuest()
    {
        HidePanel(questPanel, questCanvasGroup);
    }

    // ซ่อน Dialogue Panel ด้วยแอนิเมชัน fade-out
    public void HideDialogue()
    {
        HidePanel(dialoguePanel, dialogueCanvasGroup);
        isInDialogue = false;
        currentDialogueLines = null;
        currentDialogueIndex = 0;
        currentQuestToAccept = null;
        currentQuestToSubmit = null;
    }

    // ตรวจสอบว่า Quest Panel เปิดอยู่หรือไม่
    public bool IsQuestPanelActive()
    {
        return questPanel.activeSelf;
    }

    // ตรวจสอบว่า Dialogue Panel เปิดอยู่หรือไม่
    public bool IsDialogueActive()
    {
        return dialoguePanel.activeSelf;
    }

    // ใน QuestUI เรียกใช้ UpdateQuestStatus เพื่อแสดงความคืบหน้า
    public void ShowQuestStatus(Quest quest, Dictionary<Item, int> itemCounts, List<int> requiredCounts)
    {
        if (!questStatusPanel.activeSelf)
        {
            ShowPanel(questStatusPanel, questStatusCanvasGroup);
        }

        foreach (Transform child in activeQuestsListParent)
        {
            QuestStatusUI existingStatusUI = child.GetComponent<QuestStatusUI>();
            if (existingStatusUI != null && existingStatusUI.GetQuest() == quest)
            {
                existingStatusUI.UpdateStatus(itemCounts, requiredCounts);
                return;
            }
        }

        GameObject questStatusObj = Instantiate(questStatusPrefab, activeQuestsListParent);
        QuestStatusUI newStatusUI = questStatusObj.GetComponent<QuestStatusUI>();
        newStatusUI.SetQuest(quest);
        newStatusUI.UpdateStatus(itemCounts, requiredCounts);
    }

    // อัปเดตสถานะเควส
    public void UpdateQuestStatus(Quest quest, Dictionary<Item, int> itemCounts, List<int> requiredCounts)
    {
        foreach (Transform child in activeQuestsListParent)
        {
            QuestStatusUI statusUI = child.GetComponent<QuestStatusUI>();
            if (statusUI != null && statusUI.GetQuest() == quest)
            {
                statusUI.UpdateStatus(itemCounts, requiredCounts);
            }
        }
    }

    // ลบ QuestStatusUI หลังจากเสร็จสิ้นเควสด้วยแอนิเมชัน fade-out
    public void RemoveQuestStatus(Quest quest)
    {
        foreach (Transform child in activeQuestsListParent)
        {
            QuestStatusUI statusUI = child.GetComponent<QuestStatusUI>();
            if (statusUI != null && statusUI.GetQuest() == quest)
            {
                Destroy(child.gameObject);
                break;
            }
        }

        // ถ้าไม่มีเควสที่กำลังดำเนินการอยู่, ซ่อน QuestStatusPanel
        if (activeQuestsListParent.childCount == 0)
        {
            HidePanel(questStatusPanel, questStatusCanvasGroup);
        }
    }

    // ซ่อน QuestStatusPanel ด้วยแอนิเมชัน fade-out (หากต้องการ)
    public void HideQuestStatusPanel()
    {
        HidePanel(questStatusPanel, questStatusCanvasGroup);
    }

    // เริ่มต้นการแสดง Dialogue ด้วยแอนิเมชัน fade-in
    public void StartDialogue(Dialogue dialogue, Quest questToAccept = null, Quest questToSubmit = null)
    {
        Debug.Log($"StartDialogue called with Dialogue: {dialogue.npcName}");
        isInDialogue = true;
        currentDialogueLines = dialogue.dialogueLines;
        currentDialogueIndex = 0;
        currentQuestToAccept = questToAccept;
        currentQuestToSubmit = questToSubmit;

        // ซ่อน QuestPanel ถ้ามีการแสดงอยู่
        if (questPanel.activeSelf)
        {
            HideQuest();
        }

        // แสดง DialoguePanel ด้วยแอนิเมชัน fade-in
        ShowPanel(dialoguePanel, dialogueCanvasGroup);
        dialogueNameText.text = dialogue.npcName;

        // เริ่มพิมพ์ข้อความทีละตัวอักษร
        StartTyping(currentDialogueLines[currentDialogueIndex]);
    }

    // เริ่มพิมพ์ข้อความทีละตัวอักษร
    private void StartTyping(string text)
    {
        isTyping = true;
        dialogueContentText.text = ""; // ล้างข้อความก่อนเริ่มพิมพ์

        // เล่นเสียงพิมพ์
        if (typingSound != null)
        {
            audioSource.clip = typingSound;
            audioSource.loop = true; // ให้เสียงเล่นวนจนกว่าจะพิมพ์เสร็จ
            audioSource.Play();
        }

        typingTween = dialogueContentText.DOText(text, text.Length * typingSpeed)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                isTyping = false;
                // หยุดเสียงเมื่อพิมพ์เสร็จ
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                }

                // แสดงไอคอน Continue หลังจากข้อความพิมพ์เสร็จ
                continueIcon.SetActive(true);
            });
    }

    // ดำเนินการต่อการแสดง Dialogue
    public void ContinueDialogue()
    {
        if (!isInDialogue || currentDialogueLines == null || isTyping)
            return;

        // ซ่อนไอคอน Continue ทันทีเมื่อกด Spacebar
        continueIcon.SetActive(false); // สมมติว่า 'continueIcon' คือ GameObject สำหรับไอคอนของคุณ

        currentDialogueIndex++;

        if (currentDialogueIndex < currentDialogueLines.Length)
        {
            // เริ่มพิมพ์ข้อความถัดไป
            StartTyping(currentDialogueLines[currentDialogueIndex]);
        }
        else
        {
            // เมื่อถึงบรรทัดสุดท้าย ให้จบการสนทนาโดยอัตโนมัติ
            EndDialogue();
        }
    }

    // จบการแสดง Dialogue ด้วยแอนิเมชัน fade-out
    private void EndDialogue()
    {
        HidePanel(dialoguePanel, dialogueCanvasGroup);
        isInDialogue = false;
        currentDialogueLines = null;
        currentDialogueIndex = 0;

        // จัดการการรับหรือส่งเควสหลังจาก Dialogue
        if (currentQuestToAccept != null)
        {
            // รับเควส
            QuestManager.Instance.AcceptQuest(currentQuestToAccept, QuestManager.Instance.GetOriginNPC(currentQuestToAccept));
            ShowQuest(currentQuestToAccept);
            ShowQuestStatus(currentQuestToAccept, QuestManager.Instance.GetQuestItemCounts(currentQuestToAccept), currentQuestToAccept.requiredItemCounts);
            currentQuestToAccept = null;
        }
        else if (currentQuestToSubmit != null)
        {
            // ส่งเควส
            QuestManager.Instance.CompleteQuest(currentQuestToSubmit, QuestManager.Instance.GetOriginNPC(currentQuestToSubmit));
            RemoveQuestStatus(currentQuestToSubmit);
            currentQuestToSubmit = null;
        }
    }

    // ซ่อนทุกแผงด้วยแอนิเมชัน (ไม่รวม questStatusPanel)
    public void HideAllPanels()
    {
        HideQuest();
        HideDialogue();
        // ไม่ซ่อน questStatusPanel เพื่อให้มันยังคงอยู่
    }

    // ฟังก์ชันสำหรับแสดง Panel ด้วย DOTween fade-in
    private void ShowPanel(GameObject panel, CanvasGroup canvasGroup)
    {
        panel.SetActive(true);
        // Kill any ongoing tween
        GetTween(canvasGroup).Kill();
        // เริ่ม tween fade-in
        canvasGroup.DOFade(1f, animationDuration).SetEase(Ease.Linear);
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    // ฟังก์ชันสำหรับซ่อน Panel ด้วย DOTween fade-out
    private void HidePanel(GameObject panel, CanvasGroup canvasGroup)
    {
        // Kill any ongoing tween
        GetTween(canvasGroup).Kill();
        // เริ่ม tween fade-out
        canvasGroup.DOFade(0f, animationDuration).SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                panel.SetActive(false);
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            });
    }

    // ฟังก์ชันช่วยเหลือในการดึง Tween ปัจจุบันจาก CanvasGroup
    private Tween GetTween(CanvasGroup canvasGroup)
    {
        if (canvasGroup == questCanvasGroup)
            return questFadeTween;
        if (canvasGroup == questStatusCanvasGroup)
            return questStatusFadeTween;
        if (canvasGroup == dialogueCanvasGroup)
            return dialogueFadeTween;
        return null;
    }

    // ฟังก์ชันช่วยเหลือในการตั้งค่า Tween
    private void SetTween(CanvasGroup canvasGroup, Tween tween)
    {
        if (canvasGroup == questCanvasGroup)
            questFadeTween = tween;
        else if (canvasGroup == questStatusCanvasGroup)
            questStatusFadeTween = tween;
        else if (canvasGroup == dialogueCanvasGroup)
            dialogueFadeTween = tween;
    }

    // Update ถูกเรียกทุกเฟรม
    void Update()
    {
        if (isInDialogue && Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                // ข้ามการพิมพ์ข้อความหากกำลังพิมพ์อยู่
                typingTween.Complete();
                isTyping = false;
            }
            else
            {
                ContinueDialogue();
            }
        }
    }
}
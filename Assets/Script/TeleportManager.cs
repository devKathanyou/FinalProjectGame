using System.Collections;
using UnityEngine;

public class TeleportManager : MonoBehaviour
{
    public FadeManager fadeManager;
    public Transform player;       // �ҡ Player ŧ� Inspector
    public Vector3 newPosition;    // ���˹觷���ͧ������¼������
    public GameObject interactionIcon; // �ҡ GameObject �ͤ͹ŧ� Inspector

    private bool isTeleporting = false;  // ��ͧ�ѹ��÷ӧҹ���
    private bool isPlayerInTrigger = false;  // ��Ǩ�ͺ��Ҽ���������� Trigger �������
    private PlayerMovement playerMovement;   // ��ҧ�ԧ价���к��������͹��Ǣͧ������

    private void Start()
    {
        // ���� PlayerMovement 㹼�����
        playerMovement = player.GetComponent<PlayerMovement>();  // �������Ҽ������դ���๹�� PlayerMovement

        // ��͹�ͤ͹������������
        if (interactionIcon != null)
        {
            interactionIcon.SetActive(false);
        }
    }

    private void Update()
    {
        // ��Ǩ�ͺ��Ҽ���������� Trigger ��С����� E �������
        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.E) && !isTeleporting)
        {
            StartCoroutine(TeleportSequence());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInTrigger = true;
            if (interactionIcon != null)
            {
                interactionIcon.SetActive(true); // �ʴ��ͤ͹
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
            if (interactionIcon != null)
            {
                interactionIcon.SetActive(false); // ��͹�ͤ͹
            }
        }
    }

    private IEnumerator TeleportSequence()
    {
        isTeleporting = true;  // ��ͧ�ѹ������¡����

        // �Դ�������͹��Ǣͧ������
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        // �����˹�Ҩ��״ŧ
        yield return StartCoroutine(fadeManager.FadeToBlack());

        // ���¼�������ѧ���˹�����
        player.position = newPosition;

        // �����˹�Ҩ����ҧ���
        yield return StartCoroutine(fadeManager.FadeToClear());

        // �Դ�������͹��Ǣͧ������
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }

        isTeleporting = false;  // ����ʶҹ�����͡�������������
    }
}

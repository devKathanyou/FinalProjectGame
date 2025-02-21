using UnityEngine;
using DG.Tweening; // ��ͧ�� namespace �ͧ DOTween

public class ShowIconWithDOTween : MonoBehaviour
{
    public GameObject icon; // ��ҧ�ԧ�ͤ͹
    private Tween scaleTween; // �� Tween �ͧ��â���/˴

    private void Start()
    {
        if (icon != null)
        {
            icon.SetActive(false); // ��������¡�ë�͹�ͤ͹
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // ��Ǩ�ͺ��Ҽ�����������
        {
            if (icon != null)
            {
                icon.SetActive(true); // �ʴ��ͤ͹
                StartScaleAnimation(); // ��������Ϳ࿡�����/˴
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // ��Ǩ�ͺ��Ҽ������͡��ҧ
        {
            if (icon != null)
            {
                StopScaleAnimation(); // ��ش�Ϳ࿡��
                icon.SetActive(false); // ��͹�ͤ͹
            }
        }
    }

    private void StartScaleAnimation()
    {
        if (icon != null)
        {
            // ���絢�Ҵ������鹡�͹����� Tween
            icon.transform.localScale = Vector3.one;

            // ����� Tween �������˴ǹ���
            scaleTween = icon.transform
                .DOScale(new Vector3(0.8f, 0.8f, 0.8f), 1f) // ����� 0.5 �Թҷ�
                .SetLoops(-1, LoopType.Yoyo) // ǹ��ӵ�ʹ�Ẻ Yoyo
                .SetEase(Ease.InOutSine); // �� Ease ���͡������͹��Ƿ��������
        }
    }

    private void StopScaleAnimation()
    {
        if (scaleTween != null)
        {
            scaleTween.Kill(); // ��ش Tween
            icon.transform.localScale = Vector3.one; // ���絢�Ҵ��Ѻ�繻���
        }
    }
}

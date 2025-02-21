using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameTitleShake : MonoBehaviour
{
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        // ���������������
        ShakeTitle();
    }

    public void ShakeTitle()
    {
        // �������˹ѧ������ (Shake Effect)
        rectTransform.DOShakePosition(1f, new Vector3(10f, 10f, 0), 20, 90, false, true);
    }

    // �������ͪ�������
    public void OnMouseEnter()
    {
        ShakeTitle();
    }
}

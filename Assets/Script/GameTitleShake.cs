using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameTitleShake : MonoBehaviour
{
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        // สั่นเมื่อเริ่มเกม
        ShakeTitle();
    }

    public void ShakeTitle()
    {
        // ทำให้ตัวหนังสือสั่น (Shake Effect)
        rectTransform.DOShakePosition(1f, new Vector3(10f, 10f, 0), 20, 90, false, true);
    }

    // สั่นเมื่อชี้เมาส์
    public void OnMouseEnter()
    {
        ShakeTitle();
    }
}

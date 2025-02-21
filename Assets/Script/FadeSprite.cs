using UnityEngine;

public class FadeSprite : MonoBehaviour
{
    public SpriteRenderer targetSprite; // ��� Sprite ����ͧ������ҧŧ
    public float fadeToAlpha = 0.5f; // ��� Alpha ����ͧ��� (0 = �����, 1 = �ֺ)
    public float fadeSpeed = 2f; // ��������㹡�èҧŧ

    private float originalAlpha; // ��� Alpha �������
    private bool isFading = false;

    void Start()
    {
        if (targetSprite != null)
        {
            originalAlpha = targetSprite.color.a;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // ��Ǩ�Ѻ੾�� Player
        {
            isFading = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isFading = false;
        }
    }

    void Update()
    {
        if (targetSprite != null)
        {
            Color color = targetSprite.color;
            if (isFading)
            {
                color.a = Mathf.Lerp(color.a, fadeToAlpha, fadeSpeed * Time.deltaTime);
            }
            else
            {
                color.a = Mathf.Lerp(color.a, originalAlpha, fadeSpeed * Time.deltaTime);
            }
            targetSprite.color = color;
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering.Universal;  // ใช้สำหรับ Light 2D

public class PlayerHide : MonoBehaviour
{
    public Animator animator;
    private bool isHiding = false;

    private Rigidbody2D rb;
    private PlayerMovement playerMovement;

    private bool isNearTable = false;

    public GameObject hideIcon;

    public Light2D playerLight;  // เพิ่มตัวแปรสำหรับ Light 2D ของผู้เล่น
    public float lightIntensityNormal = 1f;  // ความเข้มของแสงปกติ
    public float lightIntensityHidden = 0.2f;  // ความเข้มของแสงเมื่อซ่อนตัว
    public float lightChangeSpeed = 2f;  // ความเร็วในการปรับความเข้มของแสง

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();

        if (hideIcon != null)
        {
            hideIcon.SetActive(false);
        }

        if (playerLight != null)
        {
            playerLight.intensity = lightIntensityNormal;  // ตั้งค่าเริ่มต้นของความเข้มแสง
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && isNearTable)
        {
            ToggleHide();
        }

        if (hideIcon != null)
        {
            hideIcon.SetActive(isNearTable && !isHiding);
        }

        // ควบคุมความเข้มของแสงให้ลดลง/เพิ่มขึ้นอย่างราบรื่น
        if (playerLight != null)
        {
            float targetIntensity = isHiding ? lightIntensityHidden : lightIntensityNormal;
            playerLight.intensity = Mathf.Lerp(playerLight.intensity, targetIntensity, lightChangeSpeed * Time.deltaTime);
        }
    }

    void ToggleHide()
    {
        isHiding = !isHiding;
        animator.SetBool("isHiding", isHiding);

        if (isHiding)
        {
            rb.velocity = Vector2.zero;
            playerMovement.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Table"))
        {
            isNearTable = true;
        }
    }

    public void EnableMovement()
    {
        playerMovement.enabled = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Table"))
        {
            isNearTable = false;
        }
    }

    public bool IsHiding
    {
        get { return isHiding; }
    }
}

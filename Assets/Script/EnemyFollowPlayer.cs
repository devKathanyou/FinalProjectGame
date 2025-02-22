using UnityEngine;

public class EnemyFollowPlayer : MonoBehaviour
{
    public float speed = 2f;
    private Transform player;

    public void SetPlayerTarget(Transform playerTransform)
    {
        player = playerTransform;
    }

    void Update()
    {
        if (player != null)
        {
            // ติดตามเฉพาะแกน X
            Vector2 direction = new Vector2(player.position.x - transform.position.x, 0).normalized;
            transform.position += (Vector3)direction * speed * Time.deltaTime;
        }
    }
}

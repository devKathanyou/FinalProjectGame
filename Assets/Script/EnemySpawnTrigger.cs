using UnityEngine;

public class EnemySpawnTrigger : MonoBehaviour
{
    public GameObject enemyPrefab; // ศัตรูที่จะเกิด
    public Transform spawnPoint;   // จุดเกิดศัตรู
    private bool hasSpawned = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasSpawned)
        {
            hasSpawned = true;
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        enemy.GetComponent<EnemyFollowPlayer>().SetPlayerTarget(GameObject.FindGameObjectWithTag("Player").transform);
    }
}

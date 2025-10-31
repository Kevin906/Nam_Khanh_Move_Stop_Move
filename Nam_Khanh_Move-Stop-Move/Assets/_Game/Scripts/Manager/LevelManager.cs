using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    [Header("Prefabs")]
    public Enemy enemyPrefab;
    public Player playerPrefab;
    public Level[] levelPrefabs;

    private Level currentLevel;
    private List<Enemy> activeEnemies = new List<Enemy>();

    private int levelIndex = 0;
    private int totalSpawnedEnemies = 0;

    void Start()
    {
        UIManager.Instance.OpenUI<MainMenu>();
        OnInit();
    }

    public void OnInit()
    {
        levelIndex = 0;
        currentLevel = Instantiate(levelPrefabs[levelIndex]);

        NavMesh.RemoveAllNavMeshData();
        NavMesh.AddNavMeshData(currentLevel.navMeshData);

        totalSpawnedEnemies = 0;
        activeEnemies.Clear();

        SpawnPlayer();
        InitialEnemySpawn();
    }

    private void SpawnPlayer()
    {
        if (currentLevel.spawnPoints == null || currentLevel.spawnPoints.Length == 0)
        {
            Debug.LogWarning("No spawn points found in current level.");
            return;
        }

        int randomIndex = Random.Range(0, currentLevel.spawnPoints.Length);
        Transform randomSpawn = currentLevel.spawnPoints[randomIndex];

        Player player = Instantiate(playerPrefab, randomSpawn.position, Quaternion.identity);
        player.OnInit();
    }

    private void InitialEnemySpawn()
    {
        int spawnCount = currentLevel.GetInitialSpawnCount();

        for (int i = 0; i < spawnCount; i++)
        {
            SpawnEnemy();
        }
    }

    private void Update()
    {
        // Kiểm tra và spawn thêm enemy nếu cần
        if (currentLevel.CanSpawnMore(activeEnemies.Count, totalSpawnedEnemies))
        {
            SpawnEnemy();
        }

        // Xóa enemy null (đã despawn)
        activeEnemies.RemoveAll(e => e == null || !e.gameObject.activeInHierarchy);
    }

    private void SpawnEnemy()
    {
        if (!currentLevel.CanSpawnMore(activeEnemies.Count, totalSpawnedEnemies))
            return;

        int randomIndex = Random.Range(0, currentLevel.spawnPoints.Length);
        Transform spawnBase = currentLevel.spawnPoints[randomIndex];

        Vector3 spawnPos = GetValidSpawnPosition(spawnBase.position, 4f);

        Enemy enemy = SimplePool.Spawn<Enemy>(PoolType.Enemy, spawnPos, Quaternion.identity);
        enemy.OnInit();
        activeEnemies.Add(enemy);

        totalSpawnedEnemies++;
    }

    private Vector3 GetValidSpawnPosition(Vector3 origin, float radius)
    {
        if (NavMesh.SamplePosition(origin + Random.insideUnitSphere * radius, out NavMeshHit hit, radius, NavMesh.AllAreas))
            return hit.position;
        return origin;
    }
}

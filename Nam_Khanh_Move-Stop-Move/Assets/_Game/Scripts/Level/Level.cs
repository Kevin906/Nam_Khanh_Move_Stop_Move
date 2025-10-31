using UnityEngine;
using UnityEngine.AI;

public class Level : MonoBehaviour
{
    [Header("NavMesh")]
    public NavMeshData navMeshData;

    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    [Header("Enemy Settings")]
    public int maxConcurrentEnemies = 4;   // Số enemy tối đa có thể xuất hiện cùng lúc
    public int totalSpawnLimit = -1;       // Giới hạn tổng số enemy có thể spawn (-1 = vô hạn)
    public int initialSpawn = 0;           // Số enemy spawn ban đầu

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (spawnPoints == null) return;
        foreach (var point in spawnPoints)
        {
            if (point != null)
                Gizmos.DrawSphere(point.position, 0.3f);
        }
    }

    public int GetInitialSpawnCount()
    {
        int count = initialSpawn > 0 ? initialSpawn : maxConcurrentEnemies;
        if (totalSpawnLimit >= 0)
            count = Mathf.Min(count, totalSpawnLimit);
        return Mathf.Max(0, count);
    }

    public bool CanSpawnMore(int currentActive, int totalSpawned)
    {
        if (currentActive >= maxConcurrentEnemies) return false;
        if (totalSpawnLimit >= 0 && totalSpawned >= totalSpawnLimit) return false;
        return true;
    }

    public int GetHowManyCanSpawn(int currentActive, int totalSpawned)
    {
        if (!CanSpawnMore(currentActive, totalSpawned)) return 0;
        int remainConcurrent = maxConcurrentEnemies - currentActive;
        if (totalSpawnLimit >= 0)
        {
            int remainTotal = totalSpawnLimit - totalSpawned;
            return Mathf.Min(remainConcurrent, remainTotal);
        }
        return remainConcurrent;
    }

    private void OnValidate()
    {
        maxConcurrentEnemies = Mathf.Max(0, maxConcurrentEnemies);
        totalSpawnLimit = Mathf.Max(-1, totalSpawnLimit);
        initialSpawn = Mathf.Max(0, initialSpawn);
    }
}

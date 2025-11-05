using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LevelManager : MonoBehaviour
{
	[Header("Prefabs")]
	public Enemy enemyPrefab;
	public Player playerPrefab;
	public Level[] levelPrefabs;

	private Level currentLevel;
	private int levelIndex;
	private List<Enemy> enemies = new List<Enemy>();
	private List<Transform> shuffledSpawns = new List<Transform>();
	private int currentSpawnIndex = 0;
	public int CharacterAmount => currentLevel.botAmount + 1;

	void Start()
	{
		UIManager.Instance.OpenUI<MainMenu>();
		OnInit();
	}

	public void OnInit()
	{
		levelIndex = 0;
		currentLevel = Instantiate(levelPrefabs[levelIndex]);
		currentLevel.OnInit();

		NavMesh.RemoveAllNavMeshData();
		NavMesh.AddNavMeshData(currentLevel.navMeshData);

		PrepareSpawnPoints();

		SpawnPlayer();

		for (int i = 0; i < currentLevel.botAmount; i++)
		{
			SpawnEnemy();
		}
	}

	private void PrepareSpawnPoints()
	{
		shuffledSpawns = new List<Transform>(currentLevel.SpawnPoints);

		// Fisher-Yates shuffle
		for (int i = 0; i < shuffledSpawns.Count; i++)
		{
			int randomIndex = Random.Range(i, shuffledSpawns.Count);
			(shuffledSpawns[i], shuffledSpawns[randomIndex]) = (shuffledSpawns[randomIndex], shuffledSpawns[i]);
		}

		currentSpawnIndex = 0;
	}

	private Transform GetNextSpawnPoint()
	{
		if (currentSpawnIndex >= shuffledSpawns.Count)
		{
			Debug.LogWarning("No spawn points left!");
			currentSpawnIndex = 0;
		}

		return shuffledSpawns[currentSpawnIndex++];
	}

	private void SpawnPlayer()
	{
		Transform randomSpawn = GetNextSpawnPoint();
		Player newPlayer = Instantiate(playerPrefab, randomSpawn.position, Quaternion.identity);
		newPlayer.OnInit();
	}

	private void SpawnEnemy()
	{
		Transform randomSpawn = GetNextSpawnPoint();

		Vector3 spawnPos = randomSpawn.position;
		NavMeshHit hit;

		if (NavMesh.SamplePosition(randomSpawn.position, out hit, 2f, NavMesh.AllAreas))
		{
			spawnPos = hit.position;
		}
		else
		{
			Debug.LogWarning($"Spawn point {randomSpawn.name} is not on NavMesh!");
		}

		Enemy enemy = SimplePool.Spawn<Enemy>(PoolType.Enemy, spawnPos, Quaternion.identity);
		enemy.OnInit();
		enemies.Add(enemy);
	}

	//private Transform GetRandomSpawnPoint()
	//{
	//	int randomIndex = Random.Range(0, currentLevel.SpawnPoints.Count);
	//	return currentLevel.SpawnPoints[randomIndex];
	//}
}

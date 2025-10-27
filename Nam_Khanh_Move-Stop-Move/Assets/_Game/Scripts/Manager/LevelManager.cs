using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class LevelManager : MonoBehaviour
{
    public Enemy enemyPrefab;
    public Player playerPrefab;
	public Level[] levelPrefabs;
	private Level currentLevel;

    private int levelIndex;
	public int CharacterAmount => currentLevel.botAmount + 1;
	private List<Enemy> enemies = new List<Enemy>();
	void Start()
    {
		UIManager.Instance.OpenUI<MainMenu>();
		OnInit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnInit()
    {
        levelIndex = 0;
        currentLevel = Instantiate(levelPrefabs[levelIndex]);

		NavMesh.RemoveAllNavMeshData();
		NavMesh.AddNavMeshData(currentLevel.navMeshData);

		SpawnPlayer();
        SpawnEnemy();
		for (int i = 0; i < currentLevel.botAmount; i++)
		{
			int randomIndex = Random.Range(0, currentLevel.SpawnPoint.Length);
			Transform randomSpawn = currentLevel.SpawnPoint[randomIndex];

			Enemy enemy = SimplePool.Spawn<Enemy>(PoolType.Enemy, randomSpawn.position, Quaternion.identity);
			enemy.OnInit();
			enemies.Add(enemy);
		}
	}

    private void SpawnPlayer()
    {
        int randomIndex = Random.Range(0, currentLevel.SpawnPoint.Length);
        Transform randomSpawn = currentLevel.SpawnPoint[randomIndex];

        Player newPlayer = Instantiate(playerPrefab, randomSpawn.position, Quaternion.identity);
        newPlayer.OnInit();
    }

    private void SpawnEnemy()
    {
        int RandomIndex = Random.Range(0, currentLevel.SpawnPoint.Length);
        Transform randomSpawn = currentLevel.SpawnPoint[RandomIndex];

		Enemy enemy = SimplePool.Spawn<Enemy>(PoolType.Enemy, randomSpawn.position, Quaternion.identity);
		enemy.OnInit();
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Level : MonoBehaviour
{
	[Header("Level Settings")]
	public NavMeshData navMeshData;
	public int botAmount;

	[Header("Spawn Points")]
	public List<Transform> SpawnPoints = new List<Transform>();

	public List<Point> points = new List<Point>();

	public void OnInit()
	{
		points.Clear();
		points.AddRange(GetComponentsInChildren<Point>());

		if (SpawnPoints.Count == 0 && points.Count > 0)
		{
			foreach (var p in points)
			{
				SpawnPoints.Add(p.transform);
			}
		}
	}
}

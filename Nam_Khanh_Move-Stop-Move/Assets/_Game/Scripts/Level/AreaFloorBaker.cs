using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using NavMeshBuilder = UnityEngine.AI.NavMeshBuilder;

public class AreaFloorBaker : MonoBehaviour
{
	[SerializeField] private NavMeshSurface Surface;
	[SerializeField] private Enemy Player;
	[SerializeField] private float UpdateRate = 1.0f;
	[SerializeField] private float MovementThreshold;
	[SerializeField] private Vector3 NavMeshSize = new Vector3(5, 5, 5);

	private Vector3 WorldAnchor;
	private NavMeshData NavMeshData;
	private List<NavMeshBuildSource> Sources = new List<NavMeshBuildSource>();

	private void Start()
	{
		NavMeshData = new NavMeshData();
		NavMesh.AddNavMeshData(NavMeshData);
		BuildNavMesh(false);
		StartCoroutine(CheckPlayerMovement());
	}

	private IEnumerator CheckPlayerMovement()
	{
		WaitForSeconds Wait = new WaitForSeconds(UpdateRate);
		while (true)
		{
			if (Vector3.Distance(WorldAnchor, Player.transform.position) > MovementThreshold)
			{
				BuildNavMesh(true);
				WorldAnchor = Player.transform.position;
			}

			yield return Wait;
		}
	}

	private void BuildNavMesh(bool Async)
	{
		Bounds navmeshBounds = new Bounds(Player.transform.position, NavMeshSize);
		List<NavMeshBuildMarkup> makups = new List<NavMeshBuildMarkup>();

		List<NavMeshModifier> modifiers;
		if (Surface.collectObjects == CollectObjects.Children)
		{
			modifiers = new List<NavMeshModifier>(Surface.GetComponentsInChildren<NavMeshModifier>());
		}
		else
		{
			modifiers = NavMeshModifier.activeModifiers;
		}
		for (int i = 0; i < modifiers.Count; i++)
		{
			if (((Surface.layerMask & (1 << modifiers[i].gameObject.layer)) == 1) && modifiers[i].AffectsAgentType(Surface.agentTypeID))
			{
				makups.Add(new NavMeshBuildMarkup()
				{
					root = modifiers[i].transform,
					overrideArea = modifiers[i].overrideArea,
					area = modifiers[i].area,
					ignoreFromBuild = modifiers[i].ignoreFromBuild
				});
			}
		}
		if (Surface.collectObjects == CollectObjects.Children)
		{
			NavMeshBuilder.CollectSources(Surface.transform, Surface.layerMask, Surface.useGeometry, Surface.defaultArea, makups, Sources);
		}
		else
		{
			NavMeshBuilder.CollectSources(navmeshBounds, Surface.layerMask, Surface.useGeometry, Surface.defaultArea, makups, Sources);
		}

		Sources.RemoveAll(sources => sources.component != null && sources.component.gameObject.GetComponent<NavMeshAgent>() != null);

		if (Async)
		{
			NavMeshBuilder.UpdateNavMeshDataAsync(NavMeshData, Surface.GetBuildSettings(), Sources, new Bounds(Player.transform.position, NavMeshSize));
		}
		else
		{
			NavMeshBuilder.UpdateNavMeshData(NavMeshData, Surface.GetBuildSettings(), Sources, new Bounds(Player.transform.position, NavMeshSize));
		}
	}
}

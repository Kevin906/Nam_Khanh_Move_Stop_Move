using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyScriptableObject")]
public class EnemyScriptableObject : ScriptableObject
{
    public int health = 100;
    public float attackDelay = 1f;
	public int damage = 100;
	public float attackRadius = 1.5f;

	public float speed = 1f;
    public bool IsRanged = false;
}

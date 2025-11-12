using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyScriptableObject")]
public class EnemyScriptableObject : ScriptableObject
{
    public int health = 100;
    public float speed = 1f;
    public float attackRange;
    public int damage = 5;
    public bool IsRanged = false;


}

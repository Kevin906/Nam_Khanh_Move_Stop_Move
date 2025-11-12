using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackRangeRadius : AttackRange
{
    public NavMeshAgent Agent;
    public AxeProjectile AxeProjectilePrefab;
    public Vector3 ProjectileSpawnOffset = new Vector3(0,1,0);
    public LayerMask Mask;

}

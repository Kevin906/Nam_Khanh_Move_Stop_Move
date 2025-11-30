using UnityEngine;

[CreateAssetMenu(fileName ="New Weapon",menuName ="Scriptable Objects/Weapon")]
public class WeaponInfo : ScriptableObject
{
    [Header("Weapon Type")]
    public GameObject[] WeaponType;

    [Header("Add Attack Range")]
    public float[] AddAttackRange;

    [Header("Add Attack Speed")]
    public float[] AddAttackSpeed;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    public void CreateProjectile(Vector3 ProjectilePosition,int _ownerID,int _opponentID,Material[] _bulletMaterial)
    {
        GameObject projectile = Pooling.instance._Pull(gameObject.tag,GetPath(gameObject.tag));
        projectile.transform.GetChild(0).gameObject.GetComponent<Renderer>().sharedMaterials = _bulletMaterial;
        projectile.transform.position = ProjectilePosition;
        Projectiles _projectile = projectile.GetComponent<Projectiles>();
        _projectile.SetID(_ownerID, _opponentID);
        _projectile.BulletMove();
    }
    string GetPath(string tag)
    {
        switch (tag)
        {
            case "Arrow":
                return "Prefabs/Projectile/Arrow";
            case "Axe_0":
                return "Prefabs/Projectile/Axe_0";
            case "Boomerang":
                return "Prefabs/Projectile/boomerang";
            case "candy_0":
                return "Prefabs/Projectile/candy_0";
            case "Hammer":
                return "Prefabs/Projectile/Hammer";
            case "Knife":
                return "Prefabs/Projectile/knife";
            case "Uzi":
                return "Prefabs/Projectile/uzi";
            default:
                return "Prefabs/Projectile/Z";
        }
    }
}

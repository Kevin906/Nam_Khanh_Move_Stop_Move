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
                return "Prefabs/Projectile/Throw/Arrows";
            case "Axe_0":
                return "Prefabs/Projectile/Throw/Axe_0s";
            case "Boomerang":
                return "Prefabs/Projectile/Throw/boomerangs";
            case "candy_0":
                return "Prefabs/Projectile/Throw/candy_0s";
            case "Hammer":
                return "Prefabs/Projectile/Throw/Hammers";
            case "Knife":
                return "Prefabs/Projectile/Throw/knifes";
            case "Uzi":
                return "Prefabs/Projectile/Throw/uzis";
            default:
                return "Prefabs/Projectile/Throw/ZZZ";
        }
    }
}

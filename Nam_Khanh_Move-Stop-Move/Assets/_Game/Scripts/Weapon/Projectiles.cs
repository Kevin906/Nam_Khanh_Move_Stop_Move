using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles : MonoBehaviour
{
    private Vector3 TargetPos,OwnerAttackPos;
    private float ProjectileSpeed;
    private float AttackRange;
    private Rigidbody _projectile;
    private int OwnerID, OpponentID;
    void Start()
    {
        BulletMove();
    }
    private void OnEnable()
    {
        GameManager.Instance.PlayAttackAudio();
    }
    private void Update()
    {
        if (Vector3.Distance(OwnerAttackPos, transform.position) > AttackRange)
        {
            DestroyProjectile();
        }
    }
    public void BulletMove()
    {
        _projectile = GetComponent<Rigidbody>();
        Vector3 dirrect = TargetPos - transform.position;
        _projectile.velocity=dirrect.normalized * ProjectileSpeed;
        transform.LookAt(TargetPos);
    }

    public void SetID(int _ownerID,int _oppenentID)
    {
        OwnerID = _ownerID;
        OpponentID = _oppenentID;
        GetPower(OwnerID);
        FindTarget();
    }
    
    void GetPower(int _ownerID)
    {
        for (int i = 0; i < GameManager.Instance.CharacterList.Count; i++)
        {
            if (GameManager.Instance.CharacterList[i].gameObject.GetInstanceID() == _ownerID && GameManager.Instance.CharacterList[i].gameObject.activeSelf)
            {
                if (GameManager.Instance.CharacterList[i].IsDeath == false)
                {
                    AttackRange = GameManager.Instance.CharacterList[i].AttackRange;
                    ProjectileSpeed = GameManager.Instance.CharacterList[i].AttackSpeed;
                    transform.localScale = GameManager.Instance.CharacterList[i].gameObject.transform.localScale;
                }
            }
        }
    }

    public void FindTarget()
    {
        for (int i = 0; i < GameManager.Instance.CharacterList.Count; i++)
        {
            if (GameManager.Instance.CharacterList[i].gameObject.GetInstanceID() == OpponentID && GameManager.Instance.CharacterList[i].gameObject.activeSelf)
            {
                TargetPos = GameManager.Instance.CharacterList[i].gameObject.transform.position;
                TargetPos.y = 1f;
            }
            else if (GameManager.Instance.CharacterList[i].gameObject.GetInstanceID() == OwnerID)
            {
                OwnerAttackPos = GameManager.Instance.CharacterList[i].gameObject.transform.position;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetInstanceID() != OwnerID)
        {
            if (other.CompareTag("Enemy"))         
            {
                if (other.GetComponent<Character>().IsDeath == false)
                {
                    other.gameObject.GetComponent<IOnhit>().OnHit();
                    AddOwnerLevel();
                    DestroyProjectile();
                }
            }
            else if (other.CompareTag("Player"))
            {
                if (other.GetComponent<Character>().IsDeath == false)
                {
                    for (int i = 0; i < GameManager.Instance.CharacterList.Count; i++)
                    {
                        if (GameManager.Instance.CharacterList[i].gameObject.GetInstanceID() == OwnerID)
                        {
                            if (GameManager.Instance.CharacterList[i].gameObject.CompareTag("Enemy"))
                            {
                                other.GetComponent<Player>().KillerName = GameManager.Instance.CharacterList[i].gameObject.GetComponent<Enemy>().enemyName;
                            }
                        }
                    }
                    other.gameObject.GetComponent<IOnhit>().OnHit();
                    AddOwnerLevel();
                    DestroyProjectile();
                }
            }

        }
        else if (other.CompareTag("Obstacle"))
        {
            GameManager.Instance.PlayWeaponImpackSound();
            DestroyProjectile();
        }
    }
    void AddOwnerLevel()
    {
        for (int i = 0; i < GameManager.Instance.CharacterList.Count; i++)
        {
            if (GameManager.Instance.CharacterList[i].gameObject.GetInstanceID() == OwnerID && GameManager.Instance.CharacterList[i].gameObject.activeSelf)
            {
                if (GameManager.Instance.CharacterList[i].IsDeath == false)
                {
                    GameManager.Instance.CharacterList[i].AddLevel();
                }
            }
        }
    }
    void DestroyProjectile()
    {
        _projectile.velocity = Vector3.zero;
        Pooling.instance._Push(gameObject.tag, gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [HideInInspector] public enum CharacterName { Thor, Kevin, AAAAA, Hello_world, Im_not_a_Bot, Am_I_a_Bot, Zaahen2025, Aatrox2013, Varus2012, Rhaast2017, Naafiri2023, Yasuo15phGG, Bruh, Badboy666, KKKK, Imposter123 }
    [HideInInspector]public enum weaponType { Arrow, Axe_0, boomerang, candy_0, Hammer, knife, uzi, Z}
    [HideInInspector] public enum clothesType 
    { 
        Arrow, Cowboy, Crown, Ear, Hat, Hat_Cap, Hat_Yellow, HeadPhone, Rau, Khien, Shield,
        Batman, Chambi, comy, dabao, onion, pokemon, rainbow, Skull, Vantim }
    [HideInInspector]
    public enum SetFullOrNormal{SetFull, Normal}
    public SetFullOrNormal lastClothes;
    [SerializeField] private Animator anim;
    public UnityAction OnAttack;
    public UnityAction OnRun;
    public UnityAction OnIdle;
    public UnityAction OnDeath;
    public UnityAction OnWin;
    public UnityAction OnDance;
    public UnityAction OnResetAllTrigger;
    public Attack attackScript;
    public float AttackRange;
    public float AttackSpeed;
    public float MoveSpeed;
    public ClothesInfo CharacterClothes;
    public Transform ShieldPosition;
    public Transform LeftHandPosition;
    public Transform HeadPosition;
    public Transform weaponPosition;
    public Renderer PantsPositionRenderer;
    public GameObject[] weaponArray = new GameObject[8];
    public Animator characterCanvasAnim;
    public WeaponInfo _weapon;
    public bool enableToAttackFlag=false;
    public float distanceToNearistEnemy;
    public Vector3 nearistEnemyPosition;
    public int opponentID;
    public int EnemySkinID;
    public bool IsDeath;
    public AudioSource audiosource;
    
    [SerializeField] private AudioClip[] DieAudio;
    [SerializeField] private AudioClip SizeUpAudio;
    [SerializeField] private AudioClip WinAudio;

    private void Start()
    {
        audiosource = GetComponent<AudioSource>();
        _weapon = GetComponent<WeaponInfo>();
        PantsPositionRenderer = GetComponent<Renderer>();
    }
    public virtual void attack()
    { 

    }

    public virtual void move() 
    {

    }

    public virtual void AddLevel()
    {

    }

    public void PlayDieAudio()
    {
        if (!GameManager.Instance.OpenSound) return;

        int index = Random.Range(0, DieAudio.Length);
        audiosource.PlayOneShot(DieAudio[index], 0.7f);
    }
    public void PlaySizeUpAudio()
    {
        if (GameManager.Instance.OpenSound) audiosource.PlayOneShot(SizeUpAudio);
    }
    public void PlayWinAudio()
    {
        if (GameManager.Instance.OpenSound) audiosource.PlayOneShot(WinAudio, 0.3f);
    }
    public void weaponListCreate()
    {
        int childCount = weaponPosition.childCount;

        for (int i = 0; i < weaponArray.Length; i++)
        {
            if (i < childCount)
                weaponArray[i] = weaponPosition.GetChild(i).gameObject;
            else
                weaponArray[i] = null;
        }
    }

    public Vector3 FindNearistEnemy(float attackRange)
    {
        distanceToNearistEnemy = 1000f;
        for (int i = 0; i < GameManager.Instance.CharacterList.Count; i++)
        {
            if (GameManager.Instance.CharacterList[i].gameObject.GetInstanceID() != gameObject.GetInstanceID() && Vector3.Distance(GameManager.Instance.CharacterList[i].gameObject.transform.position, gameObject.transform.position) < attackRange && GameManager.Instance.CharacterList[i].gameObject.activeSelf)
            {
                if (Vector3.Distance(GameManager.Instance.CharacterList[i].gameObject.transform.position, gameObject.transform.position) < distanceToNearistEnemy && GameManager.Instance.CharacterList[i].IsDeath == false)
                {
                    distanceToNearistEnemy = Vector3.Distance(GameManager.Instance.CharacterList[i].gameObject.transform.position, gameObject.transform.position);
                    nearistEnemyPosition = GameManager.Instance.CharacterList[i].gameObject.transform.position;
                    opponentID = GameManager.Instance.CharacterList[i].gameObject.GetInstanceID();
                }
            }
        }
        if (distanceToNearistEnemy > 900f) return Vector3.zero;
        else return nearistEnemyPosition;
    }

    public void AddWeaponPower()
    {
        for (int i = 0; i < weaponArray.Length; i++)
        {
            if (weaponArray[i].activeSelf)
            {
                AttackRange += _weapon.AddAttackRange[i];
                AttackSpeed += _weapon.AddAttackSpeed[i];
                break;
            }
        }
    }

    public void ChangeClothes(clothesType _ClothesType)
    {
        switch (_ClothesType)
        {
            case clothesType.Arrow:
            {
                ResetHeadPosition();
                Instantiate(CharacterClothes.HeadPosition[0]);
                break;
            }
            case clothesType.Cowboy:
            {
                ResetHeadPosition();
                Instantiate(CharacterClothes.HeadPosition[1]);
                break;
            }
            case clothesType.Crown:
            {
                ResetHeadPosition();
                Instantiate(CharacterClothes.HeadPosition[2]);
                break;
            }
            case clothesType.Ear:
            {
                ResetHeadPosition();
                Instantiate(CharacterClothes.HeadPosition[3]);
                break;
            }
            case clothesType.Hat:
            {
                ResetHeadPosition();
                Instantiate(CharacterClothes.HeadPosition[4]);
                break;
            }
            case clothesType.Hat_Cap:
            {
                ResetHeadPosition();
                Instantiate(CharacterClothes.HeadPosition[5]);
                break;
            }
            case clothesType.Hat_Yellow:
            {
                ResetHeadPosition();
                Instantiate(CharacterClothes.HeadPosition[6]);
                break;
            }
            case clothesType.HeadPhone:
            {
                ResetHeadPosition();
                Instantiate(CharacterClothes.HeadPosition[7]);
                break;
            }
            case clothesType.Rau:
            {
                ResetHeadPosition();
                Instantiate(CharacterClothes.HeadPosition[8]);
                break;
            }
            case clothesType.Khien:
            {
                ResetShieldPosition();
                Instantiate(CharacterClothes.LeftHandPosition[2], ShieldPosition);
                break;
            }
            case clothesType.Shield:
            {
                ResetShieldPosition();
                Instantiate(CharacterClothes.LeftHandPosition[3], ShieldPosition);
                break;
            }
            case clothesType.Batman:
            {
                GetDefaultClothes();
                PantsPositionRenderer.sharedMaterial = CharacterClothes.PantsMaterials[0];
                break;
            }
            case clothesType.Chambi:
            {
                GetDefaultClothes();
                PantsPositionRenderer.sharedMaterial = CharacterClothes.PantsMaterials[1];
                break;
            }
            case clothesType.comy:
            {
                GetDefaultClothes();
                PantsPositionRenderer.sharedMaterial = CharacterClothes.PantsMaterials[2];
                break;
            }
            case clothesType.dabao:
            {
                GetDefaultClothes();
                PantsPositionRenderer.sharedMaterial = CharacterClothes.PantsMaterials[3];
                break;
            }
            case clothesType.onion:
            {
                GetDefaultClothes();
                PantsPositionRenderer.sharedMaterial = CharacterClothes.PantsMaterials[4];
                break;
            }
            case clothesType.pokemon:
            {
                GetDefaultClothes();
                PantsPositionRenderer.sharedMaterial = CharacterClothes.PantsMaterials[5];
                break;
            }
            case clothesType.rainbow:
            {
                GetDefaultClothes();
                PantsPositionRenderer.sharedMaterial = CharacterClothes.PantsMaterials[6];
                break;
            }
            case clothesType.Skull:
            {
                GetDefaultClothes();
                PantsPositionRenderer.sharedMaterial = CharacterClothes.PantsMaterials[7];
                break;
            }
            case clothesType.Vantim:
            {
                GetDefaultClothes();
                PantsPositionRenderer.sharedMaterial = CharacterClothes.PantsMaterials[8];
                break;
            }
        }
    }

    public void ResetClothes()
    {
        ResetShieldPosition();
        ResetLeftHandPosition();
        ResetHeadPosition();
        GetDefaultClothes();
    }

    public void GetDefaultClothes()
    {
        PantsPositionRenderer.sharedMaterial = CharacterClothes.PantsMaterials[3];      
    }

    public void ResetShieldPosition()
    {
        foreach (Transform item in ShieldPosition)
        {
            Destroy(item.gameObject);
        }
    }

    public void ResetLeftHandPosition()
    {
        foreach (Transform item in LeftHandPosition)
        {
            Destroy(item.gameObject);
        }
    }

    public void ResetHeadPosition()
    {
        foreach (Transform item in HeadPosition)
        {
            Destroy(item.gameObject);
        }
    }
}

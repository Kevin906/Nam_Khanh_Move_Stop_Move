using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : Character, IVariable, IOnhit
{
    public static Player instance;
    public FloatingJoystick _Joystick;
    [SerializeField] private GameObject _Cicle;
    [SerializeField] private GameObject Reticle;
    [SerializeField] private Material[] CupMaterial;
    private Vector3 positionToAttack;
    public int Level;
    public CharacterName KillerName;
    private IStatePlayer currentState;
    // Start is called before the first frame update
    void Start()
    {
        IVariables();
        GameManager.Instance.CharacterList.Add(this);
    }
    // Update is called once per frame
    void Update()
    {
        ShowReticle();
        ObstacleFading();
        if (!IsDeath && GameManager.Instance.gameState == GameManager.GameState.gameStarted)
        {
            if (currentState != null)
            {
                currentState.OnExecute(this);
            }
        }
        else if (GameManager.Instance.gameState == GameManager.GameState.gameWin) OnWin();
        _Cicle.transform.position = transform.position;
        if (GameManager.Instance.IsAliveAmount == 1 && !IsDeath) StartCoroutine(CheckGameVictory());
    }

    public override void move()
    {
        if (GameManager.Instance.gameState==GameManager.GameState.gameStarted)
        {
            if (_Joystick.Horizontal != 0 || _Joystick.Vertical != 0)       //If joystick is Moving then Move Player
            {
                Vector3 temp = transform.position;
                temp.x -= _Joystick.Vertical * Time.deltaTime * MoveSpeed;
                temp.z += _Joystick.Horizontal * Time.deltaTime * MoveSpeed;
                Vector3 moveDirection = new Vector3(temp.x - transform.position.x, 0, temp.z - transform.position.z);
                moveDirection.Normalize();
                Quaternion toRotate = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotate, 720 * Time.deltaTime);
                transform.position = temp;
                enableToAttackFlag = true;      //enable Attack flag when Character stop and there are enemy in range to attack.
            }
        }
    }

    public void CheckIdleToPatrol()
    {
        if ((_Joystick.Horizontal != 0 || _Joystick.Vertical != 0)&&!IsDeath) ChangeState(new StatePlayerPatrol());
    }
    public void CheckPatrolToIdle()
    {
        if ((_Joystick.Horizontal == 0 && _Joystick.Vertical == 0)&&!IsDeath) ChangeState(new StatePlayerIdle());
    }
    public void CheckIdletoAttack()
    {
        
        if (enableToAttackFlag && FindNearistEnemy(AttackRange) != Vector3.zero&&!IsDeath)
        {
            ChangeState(new StatePlayerAttack());
        }
    }
    public override void attack()
    {
        transform.LookAt(positionToAttack);
        enableToAttackFlag = false;
        attackScript.SetID(gameObject.GetInstanceID(), opponentID);
        StartCoroutine(TurntoIdle());
    }
    IEnumerator TurntoIdle()
    {
        yield return new WaitForSeconds(0.5f);
        if(GameManager.Instance.gameState == GameManager.GameState.gameStarted&& _Joystick.Horizontal == 0 && _Joystick.Vertical == 0&&!IsDeath) ChangeState(new StatePlayerIdle());
    }

    void changeAttackRange(float attackRange)
    {
        AttackRange = attackRange;
        _Cicle.transform.localScale = new Vector3(AttackRange, 1f, AttackRange);
    }

    void ShowReticle()
    {
        positionToAttack = FindNearistEnemy(AttackRange);
        if (positionToAttack != Vector3.zero)
        {
            Reticle.transform.position = new Vector3(positionToAttack.x, 0.1f, positionToAttack.z);
            Reticle.SetActive(true);
        }
        else
        {
            Reticle.SetActive(false);
        }
    }

    #region Singleton
    void IInitializeSingleton()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    public void IVariables()
    {
        AttackRange = 5f;
        AttackSpeed = 10;
        MoveSpeed = 6f;
        weaponListCreate();                 
        weaponSwitching(weaponType.Hammer);
        UpdatePlayerItem();
        IInitializeSingleton();
        changeAttackRange(AttackRange);             
        IsDeath = false;
        Level = 0;
        ChangeState(new StatePlayerIdle());
    }

    public void OnHit()
    {
        currentState.OnExit(this);
        OnDeath();
        Reticle.SetActive(false);
        IsDeath = true;
        GameManager.Instance.KilledAmount++;
        GameManager.Instance.gameState = GameManager.GameState.gameOver;
        PlayDieAudio();
    }

    void ObstacleFading()
    {
        foreach (Transform _obstacle in GameManager.Instance.Obstacle)
        {
            if (Vector3.Distance(transform.position, _obstacle.position) < 8f)
            {
                _obstacle.GetComponent<Renderer>().sharedMaterial = CupMaterial[1];
            }
            else
            {
                _obstacle.GetComponent<Renderer>().sharedMaterial = CupMaterial[0];
            }
        }
    }
    public override void AddLevel()                                                                     
    {
        characterCanvasAnim.SetTrigger("AddLevel");                                                     
        Level++;
        transform.localScale = new Vector3(1f + 0.1f * Level, 1f + 0.1f * Level, 1f + 0.1f * Level);    
        MoveSpeed = (1f + 0.05f * Level) * 5f;                                                          
        changeAttackRange(1.05f * AttackRange);                                                         
        PlaySizeUpAudio();
    }

    public void weaponSwitching(weaponType _weaponType)
    {
        AttackRange = 5f;
        AttackSpeed = 10;
        MoveSpeed = 5f;
        for (int i = 0; i < weaponArray.Length; i++)
        {
            if (i == (int)_weaponType)
            {
                weaponArray[i].SetActive(true);
            }
            else
            {
                weaponArray[i].SetActive(false);
            }
        }
        AddWeaponPower();
    }
    
    public void ChangeState(IStatePlayer state)
    {
        if (state != currentState)
        {
            if (currentState != null)
            {
                currentState.OnExit(this);
            }
            currentState = state;
            if (currentState != null)
            {
                currentState.OnEnter(this);
            }
        }
    }

    public void UpdatePlayerItem()
    {
        for (int i = 0; i < 12; i++)
        {
            if (PlayerPrefs.GetInt("WeaponShop" + (weaponType)i) == 4)
            {
                weaponSwitching((weaponType)i);
            }
        }
        for (int i = 0; i < 25; i++)
        {
            if (PlayerPrefs.GetInt("ClothesShop" + (ClothType)i) == 4)
            {
                ChangeClothes((clothesType)i);
            }
        }
    }
    IEnumerator CheckGameVictory()
    {
        yield return new WaitForSeconds(1);
        if (GameManager.Instance.IsAliveAmount == 1 && !IsDeath)
        {
            GameManager.Instance.gameState = GameManager.GameState.gameWin;
            PlayWinAudio();
        }
        else GameManager.Instance.gameState = GameManager.GameState.gameOver;
    }
}

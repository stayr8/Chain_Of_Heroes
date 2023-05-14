using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;
using UnityEngine.UI;


public class AttackActionSystem : MonoBehaviour
{
    public static AttackActionSystem Instance { get; private set; }

    public static event EventHandler OnActionStarted;
    public static event EventHandler OnActionCompleted;

    private Vector3 unitpos;
    private Vector3 enemypos;
    private Vector3 chainpos_1;
    private Vector3 chainpos_2;
    private Quaternion unitrotation;
    private Quaternion enemyrotation;
    private Quaternion chainrotation_1;
    private Quaternion chainrotation_2;

    private CharacterDataManager characterDataManager;
    private MonsterDataManager monsterDataManager;
    private Unit player;
    private Unit chainplayer_1;
    private Unit chainplayer_2;
    private Unit enemy;

    [SerializeField] private CinemachineVirtualCamera ActionVirtualCamera;
    [SerializeField] private CinemachineVirtualCamera ActionVirtualCamera2;

    [Tooltip("체인 공격 시작 체크")]
    private bool ChainStart;
    [Tooltip("공격 장소 이동 체크")]
    private bool OnAttackAtGround;
    [Tooltip("일반 공격")]
    private bool isAtk;
    [Tooltip("체인 공격_1")]
    private bool isChainAtk_1;
    [Tooltip("체인 공격_2")]
    private bool isChainAtk_2;
    [Tooltip("세명 체인 공격 체크")]
    private bool TripleChain;


    public Slider player_bar;
    public Slider enemy_bar;
    [SerializeField] private TextMeshProUGUI PlayerHPText;
    [SerializeField] private TextMeshProUGUI EnemyHPText;
    [SerializeField] private TextMeshProUGUI PlayerName;
    [SerializeField] private TextMeshProUGUI EnemyName;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one AttackActionSystem! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
 
    }

    private void Start()
    {
        ChainStart = false;
        OnAttackAtGround = false;
        isAtk = false;
        isChainAtk_1 = false;
        isChainAtk_2 = false;
        TripleChain = false;
    }

    private void Update()
    {
        if (OnAttackAtGround && player != null)
        {
            player_bar.value = player.GetCharacterDataManager().m_hp / player.GetCharacterDataManager().m_maxhp;
            PlayerHPText.text = "" + (int)player.GetCharacterDataManager().m_hp;
            PlayerName.text = "" + player.GetUnitName();
        }

        if (OnAttackAtGround && enemy != null)
        {
            enemy_bar.value = enemy.GetMonsterDataManager().m_hp / enemy.GetMonsterDataManager().m_maxhp;
            EnemyHPText.text = "" + (int)enemy.GetMonsterDataManager().m_hp;
            EnemyName.text = "" + enemy.GetUnitName();
        }

        AttackView();
    }

    #region 싱글 공격 이동
    public void OnAtLocationMove(Unit unit, Unit target)
    {
        characterDataManager = unit.GetCharacterDataManager();
        player = unit;

        OnEnemyLocationMove(target);

        unit.SetIsGrid(true);

        OnAttackAtGround = true;

        unitpos = unit.GetWorldPosition();
        unitrotation = unit.transform.rotation;

        Vector3 playerlocationMove = new Vector3(0, 150, -3);

        LevelGrid.Instance.RemoveUnitAtGridPosition(unit.GetGridPosition(), unit);

        unit.SetPosition(playerlocationMove);

        unit.transform.rotation = Quaternion.Euler(0, 0, 0);

        OnActionStarted?.Invoke(this, EventArgs.Empty);
        ActionVirtualCamera.Follow = player.GetCameraFollow();
        ActionVirtualCamera.LookAt = player.GetCameraPos();
    }

    public void OffAtLocationMove(Unit unit, Unit target)
    {
        if (unit != null)
        {
            unit.SetPosition(unitpos);
        }

        if(!ChainStart)
        {
            OffEnemyLocationMove(target);
        }

        if (unit.GetHealth() > 0)
        {
            LevelGrid.Instance.AddUnitAtGridPosition(unit.GetGridPosition(), unit);
            unit.transform.rotation = unitrotation;
        }
        
        unit.SetIsGrid(false);

        if (!ChainStart)
        {
            OnAttackAtGround = false;
            OnActionCompleted?.Invoke(this, EventArgs.Empty);
        }
    }
    #endregion

    #region 더블 체인 공격 이동
    public void OnAtChainLocationMove_1(Unit chainunit)
    {
        chainplayer_1 = chainunit;

        chainunit.SetIsGrid(true);

        chainpos_1 = chainunit.GetWorldPosition();
        chainrotation_1 = chainunit.transform.rotation;

        LevelGrid.Instance.RemoveUnitAtGridPosition(chainunit.GetGridPosition(), chainunit);

        Vector3 chainlocationMove = new Vector3(0f, 150f, -4.5f);
        chainunit.SetPosition(chainlocationMove);

        chainunit.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void OffAtChainLocationMove_1(Unit chainunit, Unit target)
    {
        chainunit.SetPosition(chainpos_1);

        if(!TripleChain)
        {
            OffEnemyLocationMove(target);
        }

        LevelGrid.Instance.AddUnitAtGridPosition(chainunit.GetGridPosition(), chainunit);
        chainunit.transform.rotation = chainrotation_1;

        chainunit.SetIsGrid(false);
        chainunit.SetChainfirst(false);

        if (!TripleChain)
        {
            OnAttackAtGround = false;
            OnActionCompleted?.Invoke(this, EventArgs.Empty);
        }
    }
    #endregion

    #region 트리플 체인 공격
    public void OnAtChainLocationMove_2(Unit chainunit)
    {
        chainplayer_2 = chainunit;

        //characterDataManager = chainunit.GetCharacterDataManager();

        chainunit.SetIsGrid(true);

        chainpos_2 = chainunit.GetWorldPosition();
        chainrotation_2 = chainunit.transform.rotation;

        LevelGrid.Instance.RemoveUnitAtGridPosition(chainunit.GetGridPosition(), chainunit);

        Vector3 chainlocationMove = new Vector3(2f, 150, -6f);
        chainunit.SetPosition(chainlocationMove);


        chainunit.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    public void SetTripleChainPosition()
    {
        Vector3 chainlocationMove = new Vector3(0f, 150, -6f);
        chainplayer_2.SetPosition(chainlocationMove);
    }

    public void OffAtChainLocationMove_2(Unit chainunit, Unit target)
    {
        chainunit.SetPosition(chainpos_2);

        OffEnemyLocationMove(target);

        LevelGrid.Instance.AddUnitAtGridPosition(chainunit.GetGridPosition(), chainunit);
        chainunit.transform.rotation = chainrotation_2;

        chainunit.SetIsGrid(false);
        chainunit.SetChaintwo(false);

        OnActionCompleted?.Invoke(this, EventArgs.Empty);
        OnAttackAtGround = false;
    }
    #endregion

    #region 몬스터 이동
    private void OnEnemyLocationMove(Unit enemy)
    {
        monsterDataManager = enemy.GetMonsterDataManager();
        this.enemy = enemy;

        enemy.SetIsGrid(true);

        enemypos = enemy.GetWorldPosition();
        enemyrotation = enemy.transform.rotation;

        Vector3 enemylocationMove = new Vector3(0, 150, 3);

        LevelGrid.Instance.RemoveUnitAtGridPosition(enemy.GetGridPosition(), enemy);

        enemy.SetPosition(enemylocationMove);

        enemy.transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    private void OffEnemyLocationMove(Unit enemy)
    {
        if (enemy != null)
        {
            enemy.SetPosition(enemypos);
        }

        if (enemy.GetHealth() > 0)
        {
            LevelGrid.Instance.AddUnitAtGridPosition(enemy.GetGridPosition(), enemy);
            enemy.transform.rotation = enemyrotation;
        }

        enemy.SetIsGrid(false);
    }
    #endregion

    #region 카메라 시점 이동 및 변경
    private void AttackView()
    {
        // 체인 공격이 아닌 일반 공격일 경우 액션 카메라
        if (!ChainStart)
        {
            if (OnAttackAtGround)
            {
                ActionVirtualCamera.Follow = player.GetCameraFollow();
                ActionVirtualCamera.LookAt = player.GetCameraPos();
            }
            else
            {
                return;
            }
        }
        else
        {
            if (OnAttackAtGround)
            {
                if (isChainAtk_2)
                {
                    ActionVirtualCamera2.Follow = chainplayer_2.GetCameraFollow();
                    ActionVirtualCamera2.LookAt = chainplayer_2.GetCameraPos2();
                    Invoke("Camera2", 0.5f);
                }
                else if (isChainAtk_1)
                {
                    ActionVirtualCamera2.Follow = chainplayer_1.GetCameraFollow();
                    ActionVirtualCamera2.LookAt = chainplayer_1.GetCameraPos2();
                    Invoke("Camera", 0.5f);
                }
                else
                {
                    return;
                }
            }
        }
    }
    #endregion

    #region 변수 관리 및 변환
    public void Camera()
    {
        ActionVirtualCamera.Follow = chainplayer_1.GetCameraFollow();
        ActionVirtualCamera.LookAt = chainplayer_1.GetCameraPos();
    }

    public void Camera2()
    {
        ActionVirtualCamera.Follow = chainplayer_2.GetCameraFollow();
        ActionVirtualCamera.LookAt = chainplayer_2.GetCameraPos();
    }

    public CharacterDataManager GetCharacterDataManager()
    {
        return characterDataManager;
    }

    public void SetCharacterDataManager(CharacterDataManager characterDataManager)
    {
        this.characterDataManager = characterDataManager;
    }

    public MonsterDataManager GetMonsterDataManager()
    {
        return monsterDataManager;
    }

    public void SetMonsterDataManager(MonsterDataManager monsterDataManager)
    {
        this.monsterDataManager = monsterDataManager;
    }

    public Unit GetenemyChainFind()
    {
        return enemy;
    }

    public void SetUnitChainFind(Unit enemy, Unit player)
    {
        this.enemy = enemy;
        this.player = player;
    }



    public Transform GetUnitAttackFind()
    {
        if(TurnSystem.Property.IsPlayerTurn)
        {
            return player.GetCameraPos3();
        }
        else
        {
            return enemy.transform;
        }
    }

    public bool GetIsAtk()
    {
        return isAtk;
    }

    public void SetIsAtk(bool isAtk)
    {
        this.isAtk = isAtk;
    }

    public bool GetIsChainAtk_1()
    {
        return isChainAtk_1;
    }

    public void SetIsChainAtk_1(bool isChainAtk)
    {
        this.isChainAtk_1 = isChainAtk;
    }

    public bool GetIsChainAtk_2()
    {
        return isChainAtk_2;
    }

    public void SetIsChainAtk_2(bool isChainAtk)
    {
        this.isChainAtk_2 = isChainAtk;
    }

    public bool GetTripleChain()
    {
        return TripleChain;
    }

    public void SetTripleChain(bool TripleChain)
    {
        this.TripleChain = TripleChain;
    }

    public bool GetChainStart()
    {
        return ChainStart;
    }

    public void SetChainStart(bool ChainStart)
    {
        this.ChainStart = ChainStart;
    }
    #endregion
}

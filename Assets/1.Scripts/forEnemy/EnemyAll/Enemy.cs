using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyFireAll enemyFireAll;
    public event System.Action onDeath; // 사망시 발동할 이벤트
    //적 상태를 위한 enum
    public enum State
    {
        TRACE,
        ATTACK,
        DIE
    }

    public State state = State.TRACE;//enum 변수를 선언하고 상태를 Trace로 초기화한다
    //플레이어 
    public GameObject targetPlayer;
    public LayerMask Target; // 추적 대상 레이어
    public LivingEntity targetEntity; // 추적할 대상
    //플레이어와 적캐릭터의 Transform을 가져온다.
    public Transform playerTr;
    public Transform enemyTr;
    // Animator 컴포넌트를 저장할 변수
    public Animator animator;
    //공격 거리 쫓아가는 거리 
    public float attackDist;
    //public float traceDist = 10.0f;

    public bool isDie = false;//죽었는지 유무를 판단하는  변수 선언
    //코루틴에서 사용할 지연시간 변수를 선언한다.
    public WaitForSeconds ws;

    //이동을 제어하는 MoveAgent클래스를 저장할 변수생성
    public MoveAgent moveAgent;
    public Itemspawn itemSpawn;
    //애니메이터 컨트롤러에 정의한 파라미터의 해시값을 미리 추출
    public readonly int hashSpeed = Animator.StringToHash("Speed");//스피드

    // 추적할 대상이 존재하는지 알려주는 프로퍼티
    public bool hasTarget
    {
        get
        {
            // 추적할 대상이 존재하고, 대상이 사망하지 않았다면 true
            if (targetEntity != null && !targetEntity.isDead)
            {
                return true;
            }

            // 그렇지 않다면 false
            return false;
        }
    }
    public virtual void Awake()
    {
        //현제 Enemy의 transform을 가져온다.
        enemyTr = GetComponent<Transform>();
        //GetComponent로 moveAgent를 추출한다.
        moveAgent = GetComponent<MoveAgent>();
        //Animator을 추출한다.
        animator = GetComponent<Animator>();
        //Itemspawn 클래스를 추출한다.
        itemSpawn = GetComponent<Itemspawn>();
        //EnemyFire클래스를 추출한다.
        enemyFireAll = GetComponent<EnemyFireAll>();
        //코루틴 주기를 0.3초로 지정한다.
        ws = new WaitForSeconds(0.3f);
    }
    //OnEnable()은 게임 오브젝트OR스크립트가 활성화 됐을 때 실행됩니다.
    public virtual void OnEnable()
    {
        //코루틴을 이용하여 상태를 체크하는 함수를 반복실행한다.
        StartCoroutine(CheckState());
        StartCoroutine(Action());
    }
    //IEnumerator 형으로 CheckState() 함수를 생성합니다.
    IEnumerator CheckState()
    {
        while (!isDie)
        {
            //Die 상태가 아니라면 앞서 정의한 거리에 따라 상태를 분류해준다.
            if (state == State.DIE) yield break;//죽은경우 반복문을 나가준다.
           
                if (!hasTarget)
                {
                    // 20 유닛의 반지름을 가진 가상의 구를 그렸을때, 구와 겹치는 모든 콜라이더를 가져옴
                    // 단, targetLayers에 해당하는 레이어를 가진 콜라이더만 가져오도록 필터링
                    Collider[] colliders =
                        Physics.OverlapSphere(transform.position, 1000f, Target);

                    // 모든 콜라이더들을 순회하면서, 살아있는 플레이어를 찾기
                    for (int i = 0; i < colliders.Length; i++)
                    {
                        // 콜라이더로부터 LivingEntity 컴포넌트 가져오기
                        targetPlayer = colliders[i].gameObject;

                        // LivingEntity 컴포넌트가 존재하며, 해당 LivingEntity가 살아있다면,
                        if (targetPlayer != null && !targetPlayer.GetComponent<LivingEntity>().isDead)
                        {
                            playerTr = targetPlayer.transform;//플레이어의 위치를 찾아내어 추격
                            enemyFireAll.findPlayer(targetPlayer);
                            // for문 루프 즉시 정지
                            break;
                        }
                    }
                


                //Vector3.Distance 함수로 플레이어와 Enemy 사이의 거리를 알아낸다.
                float dist = Vector3.Distance(playerTr.position, enemyTr.position);
                //거리에 따른 동작 설정
                if (dist <= attackDist)//거리가 공격 거리보다 작은경우
                {
                    state = State.ATTACK;
                }
                else
                {
                    state = State.TRACE;
                }
            }
            yield return ws;

        }
    }

    //코루틴을 사용하기위해 IEnumerator 형의 Action 함수를 생성한다.
    public IEnumerator Action()
    {
        //적 캐릭터가 사망할 때 까지 무한루프
        while (!isDie)
        {
            yield return ws;
            //상태에따라 분기처리
            switch (state)
            {
                case State.TRACE:
                    enemyFireAll.isFire = false;//총발사 정지
                    moveAgent.traceTarget = playerTr.position;//플레이어의 위치를 목표위치에 넣는다.
                    break;
                case State.ATTACK:
                    if (enemyFireAll.isFire == false)//isFire이 false 일때만 true로 변경한다.
                        enemyFireAll.isFire = true;//총발사 가능
                    moveAgent.Stop();//움직임을 멈춘다. //null reference 오류
                    break;
                case State.DIE:
                    isDie = true;//죽음을 true로 함
                    enemyFireAll.isFire = false;//총발사 정지
                    moveAgent.Stop();//움직임을 멈춘다.
                    enemyFireAll.Stop();//총쏘는 것을 멈춘다.
                    //콜라이더를 비활성화 하여 죽은후 부딪히거나 총알을 맞지 않게한다.
                    Collider[] enemyColliders = GetComponents<Collider>();
                    for (int i = 0; i < enemyColliders.Length; i++)
                    {
                        enemyColliders[i].enabled = false;
                    }
                    //아이템 스폰 함수 실행
                    itemSpawn.Spawn();
                    
                        onDeath();//EnemySpawn 에서 실행됨
                    break;
            }
        }
    }
    public void Update()
    {
        
        //Speed 파라미터에 이동속도를 전달한다.
        animator.SetFloat(hashSpeed, moveAgent.speed);
    }
}

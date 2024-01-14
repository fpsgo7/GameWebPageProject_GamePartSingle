using System.Collections;
using UnityEngine;
using UnityEngine.AI; // AI, 내비게이션 시스템 관련 코드를 가져오기
// 적 AI를 구현한다
public class EnemyBoss : EnemyHealth
{
    public LayerMask whatIsTarget; // 추적 대상 레이어
    private LivingEntity targetEntity; // 추적할 대상
    private NavMeshAgent pathFinder; // 경로계산 AI 에이전트
   
    private float damage = 20f; // 공격력
    public float timeBetAttack = 0.5f; // 공격 간격
    private float lastAttackTime; // 마지막 공격 시점
    private bool death = false;

    public GameObject explosion;
    //미사일 발사 관리
    public GameObject missile;//오프라인 테스트용
    public GameObject firePos;
    

    // 추적할 대상이 존재하는지 알려주는 프로퍼티
    private bool hasTarget
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

    private void Awake() {
        startingHealth = 500;
        // 게임 오브젝트로부터 사용할 컴포넌트들을 가져오기
        pathFinder = GetComponent<NavMeshAgent>();
    }

    private void Start() {
       
        // 게임 오브젝트 활성화와 동시에 AI의 추적 루틴 시작
        StartCoroutine(UpdatePath());
        StartCoroutine(missileMake());
    }
    private IEnumerator missileMake()
    {
        while (!death )
        {
            GameObject instance = Instantiate(missile, firePos.transform.position, firePos.transform.rotation);
            instance.GetComponent<Rigidbody>().velocity = Vector3.up * 10f;//처음발싸는 위로 7만큼의 힘으로 발싸한다.
            //GameObject instance = Instantiate(missile, firePos.transform.position, firePos.transform.rotation) as GameObject;
            // 2초 주기로 처리 반복
            yield return new WaitForSeconds(8f);
        }
    }
    

    // 주기적으로 추적할 대상의 위치를 찾아 경로를 갱신
    private IEnumerator UpdatePath() {
        // 살아있는 동안 무한 루프
        while (!death)
        {
            if (hasTarget)
            {
                // 추적 대상 존재 : 경로를 갱신하고 AI 이동을 계속 진행
                pathFinder.isStopped = false;
                pathFinder.SetDestination(
                    targetEntity.transform.position);
            }
            else
            {
                // 추적 대상 없음 : AI 이동 중지
                pathFinder.isStopped = true;

                // 20 유닛의 반지름을 가진 가상의 구를 그렸을때, 구와 겹치는 모든 콜라이더를 가져옴
                // 단, whatIsTarget 레이어를 가진 콜라이더만 가져오도록 필터링
                Collider[] colliders =
                    Physics.OverlapSphere(transform.position, 1000f, whatIsTarget);

                // 모든 콜라이더들을 순회하면서, 살아있는 LivingEntity 찾기
                for (int i = 0; i < colliders.Length; i++)
                {
                    // 콜라이더로부터 LivingEntity 컴포넌트 가져오기
                    LivingEntity livingEntity = colliders[i].GetComponent<LivingEntity>();

                    // LivingEntity 컴포넌트가 존재하며, 해당 LivingEntity가 살아있다면,
                    if (livingEntity != null && !livingEntity.isDead)
                    {
                        // 추적 대상을 해당 LivingEntity로 설정
                        targetEntity = livingEntity;

                        // for문 루프 즉시 정지
                        break;
                    }
                }
            }

            // 0.25초 주기로 처리 반복
            yield return new WaitForSeconds(0.25f);
        }
    }

    // 데미지를 입었을때 실행할 처리
    
    public override void OnDamage(float damage,
        Vector3 hitPoint, Vector3 hitNormal) {
       
            // 데미지만큼 체력 감소
            currentHealth -= damage;
         

        //체력이 전부 사라지면
        if (currentHealth <= 0.0f)
        {
            Die();
        }
    }

    // 사망 처리
    public override void Die() {
        death = true;
        // 다른 AI들을 방해하지 않도록 자신의 모든 콜라이더들을 비활성화
        Collider[] enemyColliders = GetComponents<Collider>();
        for (int i = 0; i < enemyColliders.Length; i++)
        {
            enemyColliders[i].enabled = false;
        }

        // AI 추적을 중지하고 내비메쉬 컴포넌트를 비활성화
        pathFinder.isStopped = true;
        pathFinder.enabled = false;

        explosion = (GameObject)Instantiate(explosion, transform.position, transform.rotation);
        Destroy(explosion, 1f);
        Destroy(gameObject);
        GameManager.instance.goalMake();
    }

    private void OnTriggerStay(Collider other) {
        // 자신이 사망하지 않았으며,
        // 최근 공격 시점에서 timeBetAttack 이상 시간이 지났다면 공격 가능
        if (!death && Time.time >= lastAttackTime + timeBetAttack)
        {
            // 상대방으로부터 LivingEntity 타입을 가져오기 시도
            LivingEntity attackTarget
                = other.GetComponent<LivingEntity>();

            // 상대방의 LivingEntity가 자신의 추적 대상이라면 공격 실행
            if (attackTarget != null && attackTarget == targetEntity)
            {
                // 최근 공격 시간을 갱신
                lastAttackTime = Time.time;

                // 상대방의 피격 위치와 피격 방향을 근삿값으로 계산
                Vector3 hitPoint
                    = other.ClosestPoint(transform.position);
                Vector3 hitNormal
                    = transform.position - other.transform.position;

                // 공격 실행
                attackTarget.OnDamage(damage, hitPoint, hitNormal);
            }
        }
    }
}
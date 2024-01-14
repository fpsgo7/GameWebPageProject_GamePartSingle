using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI; // AI, 내비게이션 시스템 관련 코드를 가져오기
using UnityEngine;

public class EnemyMissileB1 : EnemyHealth
{
    Rigidbody missileRigid = null;//리지드바디 선언
    Transform Target = null;//표적 위치 선언
    private float maxSpeed = 0;//최대 속도 변수
    float currentSpeed = 0f; //현제 속도
    public LayerMask layerMask = 0;//레이어 마스크
    private float damage = 20f; // 공격력
    
    Vector3 t_dir;//미사일 추격 각도 변수
    //폭파 효과
    public GameObject explosion;

    private void Awake()
    {
        startingHealth = 100;//체력 조정
        maxSpeed = 10;
    }
    void Start()
    {
        missileRigid = GetComponent<Rigidbody>();
      
        StartCoroutine(LaunchDelay());//해당 코루틴은 시작하자마자 작동한다.
    }


    void Update()
    {
        if (Target != null)
        {
            if (transform.position.y < 5)
            {
                maxSpeed = 5;
                currentSpeed = 5;
            }     
            //현제 속도가 최고 속도가 될때 까지 가속
            if (currentSpeed <= maxSpeed)
                currentSpeed += maxSpeed * Time.deltaTime;
            transform.position += transform.up * currentSpeed * Time.deltaTime;//표적이 있다면 미사일을 위로 가속
            //표적위치 - 미사일 위치 => 방향과 거리 산출 이후 normalized 를 적용하여 방향만 남긴다.
            if (transform.position.y < 1)
            {
                t_dir = (Target.position - transform.position + new Vector3(0, 1, 0)).normalized;
            }
            else
            {
                t_dir = (Target.position - transform.position).normalized;
            }
            
            //미사일 y축(머리를) 해당 방향으로 설정
            transform.up = Vector3.Lerp(transform.up, t_dir, 0.25f);

        }
    }

    void SearchEnemy()
    {
        //반경 거리 1000 내의 특정 레이어 콜라이드 검출
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1000f, layerMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            LivingEntity livingEntity = colliders[i].GetComponent<LivingEntity>();
            if(livingEntity != null && !livingEntity.isDead)
            {
                Target = colliders[i].transform;//검출한 대상들중 랜덤으로 표적을 결정
                break;
            }
        }
    }

    IEnumerator LaunchDelay()
    {
        //해당 대상의 위로가는 속도가 0보다 작거나 같을경우 조건이 참이 될때까지 기다린다.
        yield return new WaitUntil(() => missileRigid.velocity.y <= 0f);
        GetComponent<Rigidbody>().useGravity = false;
        yield return new WaitForSeconds(0.1f);//그리고 덤으로 0.1초 기다린다.

        SearchEnemy();// 함수 실행
        //m_psEffect.Play();//파티클 시스템 실행

        yield return new WaitForSeconds(15f);
        Destroy(gameObject);//5초동안 충돌하지 않으면 자동으로 삭제
    }

    private void OnTriggerEnter(Collider other)
    {
        //적과 충돌하면 작동
        if (other.CompareTag("Player"))
        {
            // 공격 실행
            // 상대방으로부터 LivingEntity 타입을 가져오기 시도
            LivingEntity attackTarget
                = other.GetComponent<LivingEntity>();
            attackTarget.OnDamage(damage);
            explosion = (GameObject)Instantiate(explosion, transform.position, transform.rotation);
            Destroy(explosion, 1f);
            Destroy(gameObject, 0.1f);
        }
    }

    // 데미지를 입었을때 실행할 처리
    public override void OnDamage(float damage,
        Vector3 hitPoint, Vector3 hitNormal)
    {
     
            Debug.Log(currentHealth);
            // 데미지만큼 체력 감소
            currentHealth -= damage;
           

        //체력이 전부 사라지면
        if (currentHealth <= 0.0f)
        {
            Die();
        }
    }

    // 사망 처리
    public override void Die()
    {
        Debug.Log("총맞아 죽음");
        explosion = (GameObject)Instantiate(explosion, transform.position, transform.rotation);
        Destroy(explosion, 1f);
        Destroy(gameObject);
    }
}

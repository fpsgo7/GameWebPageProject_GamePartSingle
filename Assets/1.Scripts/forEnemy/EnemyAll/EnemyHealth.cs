using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    protected float startingHealth; // 자식 클래스에서 hp 넣을것!
    public float currentHealth;// 현재 체력
    public ParticleSystem hitEffect; // 피격시 재생할 파티클 효과
    public AudioClip deathSound; // 사망시 재생할 소리
    public Animator animator;
    public readonly int hashDie = Animator.StringToHash("Die");
    public readonly int hashDieIdx = Animator.StringToHash("DieIdx");

    // 생명체가 활성화될때 상태를 리셋
    protected virtual void OnEnable()//상속을 위한 가상 메소드
    {
        //Animator을 추출한다.
        animator = GetComponent<Animator>();
        // 체력을 시작 체력으로 초기화
        currentHealth = startingHealth;
    }

    // 호스트->모든 클라이언트 방향으로 체력과 사망 상태를 동기화 하는 메서드

    public void ApplyUpdatedHealth(float newHealth)
    {
        currentHealth = newHealth;
    }

    // 데미지를 입었을때 실행할 처리

    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        //죽기전에는 false라 밑의 동작들이 실행된다.
        // 공격 받은 지점과 방향으로 파티클 효과를 재생
        hitEffect.transform.position = hitPoint;
        hitEffect.transform.rotation
            = Quaternion.LookRotation(hitNormal);
        hitEffect.Play();

        //포톤 작업
            // 데미지만큼 체력 감소
            currentHealth -= damage;
        //체력이 전부 사라지면
        if (currentHealth <= 0.0f)
        {
            Die();
        }

    }
    // 데미지를 입었을때 실행할 처리 폭탄일경우
    public void OnDamage(float damage)
    {
            // 데미지만큼 체력 감소
            currentHealth -= damage;


        //체력이 전부 사라지면
        if (currentHealth <= 0.0f)
        {
            Die();
        }

    }

    public virtual void Die()//상속받아 사용
    {
        // 사망 상태를 참으로 변경
        //랜덤으로 0부터 2까지 랜덤으로 선택되게 트리거 설정
        animator.SetInteger(hashDieIdx, Random.Range(0, 2));
        animator.SetTrigger(hashDie);//die 트리거 활성  
        GetComponent<Enemy>().state = Enemy.State.DIE; //적상태를 죽음 상태로 전환
    }
}

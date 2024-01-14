using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : EnemyHealth
{
    protected override void OnEnable()//부모 클래스에서 상속받아 사용
    {
        startingHealth = 100f;//시작 체력값 설정
        base.OnEnable();//위 문장 실행후 부모클래스의 해당 메소드 내용 실행
    }

    public override void Die()
    {
        base.Die();
    }
}

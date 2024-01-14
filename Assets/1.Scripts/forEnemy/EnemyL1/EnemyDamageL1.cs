
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageL1 : EnemyHealth
{
    protected override void OnEnable()//부모 클래스에서 상속받아 사용
    {
        startingHealth = 100f;
        base.OnEnable();//위 문장 실행후 부모클래스의 해당 메소드 내용 실행
    }

    public override void Die()
    {
        base.Die();
    }
}

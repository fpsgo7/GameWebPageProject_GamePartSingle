using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class EnemyAIL2 : Enemy
{
    public override void Awake()
    {
        base.Awake();
        attackDist = 10f;//적의 사격 거리 설정
    }

    public override void OnEnable()
    {
        base.OnEnable();
    }
}

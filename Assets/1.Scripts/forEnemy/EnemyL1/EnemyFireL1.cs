using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFireL1 : EnemyFireAll
{
    protected override void Awake()
    {
        base.Awake();
        //값 설정 상속형식으로 변수 생성없이 값 설정만으로 적들의 위력을 설정가능하다.
        reloadTime = 2.0f;
        maxBullet = 30;
        currBullet = 30;
        damage = 1f;
        wsReload = new WaitForSeconds(reloadTime);
          //총을 발사하고 딜레이를 주기위한 변수들
       nextFire = 1.0f;
       fireRate = 0.1f;
       damping = 10.0f;
}
    //플레이어 추적
    public override void findPlayer(GameObject targetPlayer)
    {
        base.findPlayer(targetPlayer);
    }
    public override void Update()
    {
        //죽음상태가 true인경우 반복문을 실행하지 못하게함
        if (death == true)
        {
            enemyAnimator.SetBool(hashFire, false);
        }
        else
        {
          
                //실질적인발사
                if (!isReload && isFire)
                {
                    //Time.time은 스크립트가 실행됐을때부터 흘러가는 시간이며 nextFire에는 Time.time+딜레이 시간이 들어간다.
                    if (Time.time >= nextFire)
                    {
                        Fire();
                        //Fire트리거를 활성화 하고 총소리를 플레이해준다.
                        enemyAnimator.SetBool(hashFire, true);
                        //랜덤한 딜레이을 위해 Random 함수를 사용하며 상단의 using System;을 지워 사용한다.
                        nextFire = Time.time + fireRate + Random.Range(0.0f, 0.3f);
                    }
                    //쿼터니언 을 이용하여 플레이어를 바라보게 한다.(A벡터 -B 벡터 ) =B 좌표에서 A 좌표로 가는 벡터를 나타낸것이다.
                    Quaternion rot = Quaternion.LookRotation(playerTr.position - enemyTr.position);
                    //보간함수Slerp를 이용해서 점진적으로 회전시킨다 Slerp는 a각도에서 b각도 사이를 시간 t에 따라 점진적으로 반환하는 함수 이다.
                    enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot, Time.deltaTime * damping);
                }
                else
                {
                    enemyAnimator.SetBool(hashFire, false);
                }
            
        }
    }
    public override void Fire()
    {
        base.Fire();
    }
    public override void Stop()
    {
        base.Stop();
    }

    //호스트 에서 실행되는 실제 발사 처리

    public override void ShotProcessOnServer()
    {
        base.ShotProcessOnServer();
    }

    public override void ShotEffectProcessOnClients(Vector3 hitPosition)
    {
        base.ShotEffectProcessOnClients(hitPosition);

    }
    // 발사 이펙트와 소리를 재생하고 총알 궤적을 그린다
    protected override IEnumerator ShotEffect(Vector3 hitPosition)
    {
        return base.ShotEffect(hitPosition);
    }

    //코루틴으로 사용할 ShowMuzzleFlash 함수를 선언한다.
    //총알 발사섬광을 표현한다.
    public override IEnumerator ShowMuzzleFlash()
    {
        return base.ShowMuzzleFlash();
    }

    //제장전 IEnumerator형 메소드

    public override IEnumerator Reloading()
    {
        return base.Reloading();
    }


}
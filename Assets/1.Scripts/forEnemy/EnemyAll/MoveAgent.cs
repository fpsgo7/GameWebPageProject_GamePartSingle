using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveAgent : MonoBehaviour
{
    //NavMeshAgent 컴포넌트를 저장할 변수
    private NavMeshAgent agent;
    //enemy의 Transform을 알아오기위한 변수를 선언한다.
    private Transform enemyTr;
    //추적 속도를 정의한 변수를 선언해줍니다.
    private float traceSpeed = 4.0f;
    private float damping = 1.0f;//회전 속도

    //추적 대상의 위치를 저장하는 변수
    private Vector3 _traceTarget;
    public Vector3 traceTarget
    {
        get { return _traceTarget; }
        set
        {
            _traceTarget = value;
            agent.speed = traceSpeed;
            //추적 상태의 회전계수
            damping = 7.0f;
            TraceTarget(_traceTarget);
        }
    }
    //스피드 프로퍼티
    public float speed
    {
        get { return agent.velocity.magnitude; }
    }
    //TraceTarget()함수 생성
    private void TraceTarget(Vector3 pos)
    {
        if (agent.isPathStale) return;

        agent.destination = pos;
        agent.isStopped = false;
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();//몇몇 문장들은 NULL 값을 허용하지 않아 마스터 클라이언트가 아닌경우에도 값을 줘야한다.***

        enemyTr = GetComponent<Transform>();//적캐릭터 위치 가져오기
        //autoBraking을 활성화하게되면 목적지에 가까워지면 속도가 느려지고
        //다시 출발하면 서서히 속도가 빨라진다.
        //균일한 속도로 이동하기 위해 false 로 변경해줍니다.
        agent.autoBraking = false;
        agent.updateRotation = false;//자동으로 회전하는 기능을 비활성화
    }
    //멈추기위한 스탑 함수
    public void Stop()
    {
        agent.isStopped = true;//nullReference 오류 EnemyAI148과 동시 오류
        //바로 정지하기위해 속도를 0으로 설정
        agent.velocity = Vector3.zero;
    }
    void Update()
    {
        // 호스트가 아니라면 애니메이션의 파라미터를 직접 갱신하지 않음
        // 호스트가 파라미터를 갱신하면 클라이언트들에게 자동으로 전달되기 때문.

        if (agent.isStopped == false)//움직이는 경우
        {

            //NavMeshAgent가 가야할 방향 벡터를 쿼터니언 타입의 각도로 변환
            Quaternion rot = Quaternion.LookRotation(agent.desiredVelocity);
            //보간 함수를 사용해 점진적으로 회전시킴
            enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot,
                Time.deltaTime * damping);

        }
    }
}

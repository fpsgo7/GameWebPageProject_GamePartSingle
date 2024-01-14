using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterMove : MonoBehaviour
{
    public float speed = 1.0f;//이동 속도
    public float damping = 3.0f;//회전 시 회전 속도를 조절하는 계수

    private Transform helicopterTr;//트랜스 폼형 변수
    private Transform[] points; //웨이 포인트를 저장할 배열
    private int nextIdx = 1;//다음에 이동해야 할 위치 인덱스 변수
    private void Start()
    {
        helicopterTr = GetComponent<Transform>();
        points = GameObject.Find("WayPointGroup").GetComponentsInChildren<Transform>();
    }

    private void Update()
    {
        MoveWayPoint();
    }
    private void MoveWayPoint()
    {
        //현재 위치에서 다음 웨이 포인트를 바라보는 벡터를 계산
        Vector3 direction = points[nextIdx].position - helicopterTr.position;
        //산출된 벡터의 회전 각도를 쿼터니언 타입으로 산출
        Quaternion rot = Quaternion.LookRotation(direction);
        //현재 각도에서 회전해야 할 각도까지 부드럽게 회전처리
        helicopterTr.rotation = Quaternion.Slerp(helicopterTr.rotation, rot, Time.deltaTime * damping);

        //전진 방향으로 이동처리 
        helicopterTr.Translate(Vector3.forward * Time.deltaTime * speed);
    }
    private void OnTriggerEnter(Collider other)
    {
        //웨이 포인트 (Point 게임오브젝트)에 충돌여부 판단
        if (other.CompareTag("wayPoint"))
        {
            Debug.Log("충돌함");
            //맨 마지막 웨이 포인트에 도달했을 때 처음 인덱스로 변경
            //시작 인덱스가 1인 이유는 인덱스 0에는 자기
            //오브젝트 Transform 이들어가서이다.
            nextIdx = (++nextIdx >= points.Length) ? 1 : nextIdx;
        }
    }
}

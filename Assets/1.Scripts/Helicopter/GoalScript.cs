using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalScript : MonoBehaviour
{
    //public int count;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" )
        {
            //플레이어 헬스의 함수를 실행하여 탈출 관련 동작과 탈출 카운트 증가시키고
            //탈출 로프를 활성화한다.
            other.GetComponent<PlayerHealth>().escapeAreaIn();
            other.GetComponent<PlayerHealth>().escapeRope.SetActive(true);
        }
    }
}

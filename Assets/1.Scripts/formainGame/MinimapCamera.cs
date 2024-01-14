using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
   
    public GameObject myPlayer;//추적할 플레이어 객체
    private bool once = true;

    private void LateUpdate()
    {
        if(myPlayer == null && once == true)
        {
            //게임시작후 자신의 플레이어에 Player1이란 테그가 주어지므로
            //해당 테그를 찾아 적용할 플레이어를 찾는다.
            myPlayer = GameObject.FindWithTag("Player1");
            if (myPlayer != null)
                once = false;
        }
        if(myPlayer != null)
        {
            //캐릭터가 이동하는 위치를 실시간으로 받아서 해당위치로 따라간다.
            Vector3 FixedPos = new Vector3(myPlayer.transform.position.x,
                myPlayer.transform.position.y + 50,
                myPlayer.transform.position.z);
            transform.position = FixedPos;
        }
    }
}

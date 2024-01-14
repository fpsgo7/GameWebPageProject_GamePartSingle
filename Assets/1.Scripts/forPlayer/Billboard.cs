using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour {
    //대상을 따라가는 형식으로 구현
    //public 형태로 유니티 에디터에서 플레이어와 연결
    public GameObject myPlayer;

    public float offsetX;
    public float offsetY;
    public float offsetZ;


    private void LateUpdate()
    {
        Vector3 FixedPos = new Vector3(myPlayer.transform.position.x + offsetX,
            myPlayer.transform.position.y + offsetY+3 ,
            myPlayer.transform.position.z + offsetZ-0.3f);
        transform.position = FixedPos;
    }

}

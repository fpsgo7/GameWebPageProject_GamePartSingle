using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EndingScroll : MonoBehaviour
{
    [SerializeField]
    private GameObject endScrollTextObject;//게임 클리어 텍스트를 연결한다.

    private float moveSpeed = 100f;//스피트
    private Vector3 v3;//스크롤을 이동하기위한 백터값변수

    private bool once= true; //게임나가기를 한번만 작동하기위한 변수

    //각 이름들과 점수들을 넣어 출력하기위한 변수들 
    private string masterName;
    private string remoteName;
    private string winnerName;
    private string masterPointNameInfo;
    private string remotePointNameInfo;
    //점수용 변수
    private int masterScore;
    private int remoteScroe;
    
    private void Start()
    {
        //방장 점수와 손님 점수 구하기
        //UIManager에서 적을 쓰러트리면 나오는 기본점수와
        //GameManager에서 사격 포인트를 얻어온다.
        masterScore = GameManager.instance.score +
               GameManager.instance.hitScorePlayerMaster;
      
       
            //GameManager에서 자신의 이름과 상대 플레이어의 이름이
            //이미 입력되 있어 가져오기만 하면된다.
            masterName = GameManager.instance.myPlayerName;
           
          
      
        //택스트 합쳐서 정보 출력문 용 변수 완성하기
        masterPointNameInfo =
               masterName +
               "님의 점수는" +
               masterScore +
               "점\n\n";
      

        //출력문 
        endScrollTextObject.GetComponent<Text>().text= (
            masterPointNameInfo+
            "게임 기획           박종원\n\n" +
            "게임 프로그래밍 박종원\n\n"+
            "Ready For Launch!     를\n\n" +
            "끝까지   플레이 해주셔서\n\n"+
            "감사드립니다.\n\n"+
            "Thanks for playing\n\n"+
            "this game.\n\n"+
            "gg\n\n"
        ) ;
        //백터에 미리 초기 스크롤 위치를 넣어준다.
        v3 = endScrollTextObject.transform.position;
    }
    private void Update()
    {
        //Time.deltaTime으로 컴퓨터 사양별로 달라지는 속도를 방지한다.
        v3.y += moveSpeed * Time.deltaTime;//y축으로 이동
        //앤딩스크롤에 v3의 달라진 백터값을넣어 움직이게함
        endScrollTextObject.transform.position = v3;
        //해당 조건만큼 움직이면
        if (v3.y >= 2300 && once == true)
        {
            once = false;
            //게임을 나가기 함수를 실행하여 자동으로 나가게함
            GameManager.instance.OnExitClick();
        }
    }
}

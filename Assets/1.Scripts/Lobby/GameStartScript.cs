using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameStartScript : MonoBehaviour
{
     public static GameStartScript instance
    {
        get
        {
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (m_instance == null)
            {
                // 씬에서 GameManager 오브젝트를 찾아 할당
                m_instance = FindObjectOfType<GameStartScript>();
            }

            // 싱글톤 오브젝트를 반환
            return m_instance;
        }
    }
    private static GameStartScript m_instance; // 싱글톤이 할당될 static 변수
    //게임 상태
    public enum GameState
    {
        LOGIN = 0,
        ROOMS = 1
    }
    //게임 상태 시작을 로그인으로 함
    public GameState gameState = GameState.LOGIN;

    private string gameVersion = "1.0";//게임 버전
    public string userId = "user";//유저아이디
    public byte maxPlayer = 2;//최대 플레이어수

    public Text titleText; // 네트워크 정보를 표시할 타이틀

    public TMP_InputField tInputUserid;//유저아이디
    public TMP_InputField tInputRoomName;//방이름

    public GameObject[] panels;//로그인 패널과 방 패널 

    public GameObject room;//방
    public Transform roomGridTr;//방의 레이아웃 

    public static string playerName=null;
    void Start()
    {
        // 작성을 하지않으면 랜덤으로 방이름과 유저아이디가 입력된다.
        tInputUserid.text = PlayerPrefs.GetString("USER_ID", "USER_" + Random.Range(1, 999));
        //tInputRoomName.text = PlayerPrefs.GetString("ROOM_NAME", "ROOM_" + Random.Range(1, 999));
        //StartCoroutine(Disconnect());
    }
    //로그인 시작하기 
   
    public void OnLogin()//1.로그인 버튼 클릭
    {
        /*
        PhotonNetwork.GameVersion = this.gameVersion;//게임 버전 적용
        PhotonNetwork.NickName = tInputUserid.text;//포톤닉네임에 이름 넣기 
        PhotonNetwork.ConnectUsingSettings();  // 설정한 정보를 가지고 마스터 서버 접속 시도
        titleText.text = "<color=red>Ready For</color>";//타이틀 텍스트 수정하여 마스터 서버에 접속 중임을 표시
        PlayerPrefs.SetString("USER_ID", PhotonNetwork.NickName);//게임자체에 이름저장
        playerName = PhotonNetwork.NickName;//playerName에 포톤 네트워크 닉네임 넣기
        ChangePanel(GameState.ROOMS);//패널 변경
        */
        playerName = tInputUserid.text;
        SceneManager.LoadScene("MainGame");
    }
}

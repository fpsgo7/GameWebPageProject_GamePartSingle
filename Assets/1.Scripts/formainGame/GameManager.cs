
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

// 점수와 게임 오버 여부, 게임 UI를 관리하는 게임 매니저
public class GameManager : MonoBehaviour
{
    // 외부에서 싱글톤 오브젝트를 가져올때 사용할 프로퍼티
    public static GameManager instance
    {
        get
        {
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (m_instance == null)
            {
                // 씬에서 GameManager 오브젝트를 찾아 할당
                m_instance = FindObjectOfType<GameManager>();
            }

            // 싱글톤 오브젝트를 반환
            return m_instance;
        }
    }
    private static GameManager m_instance; // 싱글톤이 할당될 static 변수

    public GameObject playerPrefab; // 생성할 플레이어 캐릭터 프리팹

    private int score = 0; // 현재 게임 점수
    public bool isGameover { get; private set; } // 게임 오버 상태
    //플레이어 기다리기
    public int playerCount = 0;//플레이어 수세기
    private bool isPlayerWait = true;//플레이어 기다리는 상태
    public GameObject enemySpawn; //적 생성기에 접근하여 setvisible로 게임 시작 조절하기
    public GameObject playerWaitView;// 플레이어를 기다리는 동안 ui를 띄워줌
    public GameObject mobilePlayerWaitView;//플레이어 기다리는 모바일 ui

    //죽음 관련변수
    public int deathCount = 0;
    //일시정지 구현용 변수
    public bool isPaused = false;
    public GameObject pausePanel;//일시정지 페널을 화면에 보여주기위한변수
    public GameObject mobilePausePanel;//모바일버전
    //게임클리어
    public int escapeCount;
    public GameObject goalArea;
    public GameObject clearPanel;//pc버전
    public GameObject mobileClearPanel;//모바일 버전
    public bool isClear=false;
    //플레이어 이름과 추가 점수 엔딩 스클롤 관련
    public int hitScorePlayerMaster = 0;//호스트 플레이어추가점수
    public int hitScorePlayerRemote = 0;//게스트 플레이어 추가점수
    public string myPlayerName;
    public string otherPlayerName;
    //매인 게임 UI 선택하기
    public GameObject pcUI;
    public GameObject mobileUI;
    //탈출 관련 변수 
    public GameObject helicopterCamera;
    public GameObject helicopter;
    private void Awake()
    {
        //로컬플레이어의 이름을 변수에 넣기
        myPlayerName = GameCharacterInfo.Nickname;
    }

    // 게임 시작과 동시에 플레이어가 될 게임 오브젝트를 생성
    private void Start()
    {
        // 생성할 랜덤 위치 지정
        Vector3 randomSpawnPos = Random.insideUnitSphere * 5f;
        // 위치 y값은 0으로 변경
        randomSpawnPos.y = 0f;

        // 네트워크 상의 모든 클라이언트들에서 생성 실행
        // 단, 해당 게임 오브젝트의 주도권은, 생성 메서드를 직접 실행한 클라이언트에게 있음
        Instantiate(playerPrefab, randomSpawnPos, Quaternion.identity);

        //모바일 과 pc UI중 하나 선택하기
#if UNITY_ANDROID
        pcUI.SetActive(false);//모바일 버전일경우 pc용 ui 비활성화
#endif
#if UNITY_STANDALONE
        mobileUI.SetActive(false);//pc버전일 경우 모바일 ui 비활성화
#endif
    }

    // 점수를 추가하고 UI 갱신
    public void AddScore(int newScore)
    {
        // 게임 오버가 아닌 상태에서만 점수 증가 가능
        if (!isGameover)
        {
            // 점수 추가
            score += newScore;
            // 점수 UI 텍스트 갱신
            UIManager.instance.UpdateScoreText(score);
        }
    }

    // 게임 오버 처리
    public void GameOver()
    {
        // 게임 오버 상태를 참으로 변경
        isGameover = true;
        // 게임 오버 UI를 활성화
        UIManager.instance.SetActiveGameoverUI(true);
    }

    // 키보드 입력을 감지하고 룸을 나가게 함
    private void Update()
    {
       
        /*
        if (Input.GetKeyDown(KeyCode.P))//테스트 용 게임 클리어
        {
            photonView.RPC("GameClear", RpcTarget.All);
        }
        */
        //플레이어가 다죽어있으면 실행
        if (deathCount >= 1 && isClear==false)
        {
            GameOver();
        }
        //플레이어가 다 탈출지점에 도착하면 실행
        if(escapeCount >= 1)
        {
            GameClear();
        }
    }

    //일시정지 함수
    public void OnPauseClick()
    {
        //일시 정지값을 토글시킴
        isPaused = !isPaused;//true일경우 false로 false 일 경우 true로
        //조건식으로 시간 속도 선택
        Time.timeScale = (isPaused) ? 0.01f : 1.0f;//시간속도를 느리게하여 멈추는 효과 사용
        //플레이어 스크립트에 접근하여 움직임을 비활성화시킴
        var playerObj = GameObject.FindGameObjectWithTag("Player");
        var scripts = playerObj.GetComponents<MonoBehaviour>();
        foreach (var script in scripts)
        {
            script.enabled = !isPaused;//isPaused 의 값이 false일 경우엔 true로 활성화 ture일경우 false로 비활성화한다.
        }
        //pause 패널 활성화
#if UNITY_ANDROID
        mobilePausePanel.SetActive(isPaused);
#endif
#if UNITY_STANDALONE
        pausePanel.SetActive(isPaused);
#endif
    }
    //일시정지 화면에서 게임 나가기 함수
    public void OnExitClick()
    {
        Time.timeScale = 1.0f;//일시정지 화면에서 나갈경우 시간 속도가 유지되기 때문에 미리 시간 속도를 초기화 시키고 나간다.
        SceneManager.LoadScene("Lobby");
    }

    //게임클리어 
    //탈출지점 시작
    //EnemyBoss 스크립트에서 호출된다.
    public void goalMake()//보스가 죽으면 실행되는 함수 
    {
        goalArea.SetActive(true);
    }
    //탈출지점 도착
    public void GameClear()
    {
        isClear = true;//클리어 상태를 true 로 함
#if UNITY_ANDROID
        mobileClearPanel.SetActive(true);
#endif
#if UNITY_STANDALONE
        clearPanel.SetActive(true);
#endif
    }
    //게임 시작
    private void GameStart()
    {
        enemySpawn.SetActive(true);//적생성 실행
        isPlayerWait = false;//한번만 실행되게함
#if UNITY_ANDROID
            mobilePlayerWaitView.SetActive(false);
#endif
#if UNITY_STANDALONE
        playerWaitView.SetActive(false);//ui 삭제
#endif
    }
}
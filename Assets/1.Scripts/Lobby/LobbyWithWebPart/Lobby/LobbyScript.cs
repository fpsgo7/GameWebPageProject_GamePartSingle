 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScript : MonoBehaviour
{
    private UserHttpRequest userHttpRequest;
    private GameCharacterHttpRequest gameCharacterHttpRequest;
    private GameHighScoreHttpRequest gameHighScoreHttpRequest;

    private CharacterPanelScript characterPanelScript;
    private MakeGameCharacterPanelScript makeGameCharacterPanelScript;
    private LoginPanelScript loginPanelScript;
    private CharacterRankScreenPanelScript characterRankScreenPanelScript;
    private ChattingPanelScript chattingPanelScript;

    private GameStartScript gameStartScript;

    private void Awake()
    {
        userHttpRequest = GameObject.Find("WebRequestScript").GetComponent<UserHttpRequest>();
        gameCharacterHttpRequest = GameObject.Find("WebRequestScript").GetComponent<GameCharacterHttpRequest>();
        gameHighScoreHttpRequest = GameObject.Find("WebRequestScript").GetComponent<GameHighScoreHttpRequest>();
        characterPanelScript = GameObject.Find("LobbyScript").GetComponent<CharacterPanelScript>();
        makeGameCharacterPanelScript = GameObject.Find("LobbyScript").GetComponent<MakeGameCharacterPanelScript>();
        loginPanelScript = GameObject.Find("LobbyScript").GetComponent<LoginPanelScript>();
        characterRankScreenPanelScript = GameObject.Find("LobbyScript").GetComponent<CharacterRankScreenPanelScript>();
        chattingPanelScript = GameObject.Find("LobbyScript").GetComponent<ChattingPanelScript>();
        gameStartScript = GameObject.Find("GameStartScript").GetComponent<GameStartScript>();
    }

    private void Start()
    {
        // 게임이 끝난이후 로비 씬에서  로그인 되어 있기에 로그인 창은 넘어간다.
        if (UserInfo.Email != null)
        {
            loginPanelScript.LoginActive(true, true);
            characterPanelScript.SetActive(true);
            gameHighScoreHttpRequest.GetGameRanks(); // 게임 랭크 패널은 랭크 불러오기 작업후 가져온다.
        }
    }

    /// <summary>
    /// 로그인 버튼 클릭
    /// </summary>
    public void LoginButton_Click()
    {
        string email = loginPanelScript.GetEmailText();
        string password = loginPanelScript.GetPasswordText();
        userHttpRequest.login(email, password);
    }
    /// <summary>
    /// 캐릭터 생성 버튼 클릭
    /// </summary>
    public void CreateCharacterButton_Click()
    {
        string nickname = makeGameCharacterPanelScript.GetNewNickNAme();
        gameCharacterHttpRequest.CreateGameCharacter(UserInfo.Email, nickname);
    }
    /// <summary>
    /// 캐릭터 화면에서 게임룸 입장하기
    /// </summary>
    public void InterGameLoomsButton_Click()
    {
        characterPanelScript.SetActive(false);
        gameStartScript.OnLogin();
    }

    /// <summary>
    /// 게임 랭크 리프레시 버튼 클릭
    /// </summary>
    public void GameRankRefreshButton_Click()
    {
        gameHighScoreHttpRequest.GetGameRanks();
    }

    /// <summary>
    /// 게임 랭크 화면 오픈 버튼
    /// </summary>
    public void GameRankOpenButton_Click()
    {
        chattingPanelScript.SetActive(false);
        gameHighScoreHttpRequest.GetGameRanks(); // 게임 랭크 패널은 랭크 불러오기 작업후 가져온다.
    }
    /// <summary>
    /// 게임 채팅 화면 오픈 버튼
    /// </summary>
    public void GameChatOpenButton_Click()
    {
        characterRankScreenPanelScript.SetActive(false, null);
        chattingPanelScript.SetActive(true);
    }
}

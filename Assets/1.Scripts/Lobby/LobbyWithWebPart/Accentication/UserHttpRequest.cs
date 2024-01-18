using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

[System.Serializable]
public class LoginInfo
{
    public string email;
    public string password;

    public LoginInfo(string email, string password)
    {
        this.email = email;
        this.password = password;
    }
}
/// <summary>
/// 유저 관련 http 작업을 관리한다.
/// </summary>
public class UserHttpRequest : MonoBehaviour 
{
    private LoginPanelScript loginPanelScript;
    private MakeGameCharacterPanelScript makeGameCharacterPanelScript;
    private CharacterPanelScript characterPanelScript;
    private GameHighScoreHttpRequest gameHighScoreHttpRequest;

    private void Awake()
    {
        loginPanelScript = GameObject.Find("LobbyScript").GetComponent<LoginPanelScript>();
        makeGameCharacterPanelScript = GameObject.Find("LobbyScript").GetComponent<MakeGameCharacterPanelScript>();
        characterPanelScript = GameObject.Find("LobbyScript").GetComponent<CharacterPanelScript>();
        gameHighScoreHttpRequest = GameObject.Find("WebRequestScript").GetComponent<GameHighScoreHttpRequest>();
    }

    public void login(string email, string password)
    {
        LoginInfo loginInfo = new LoginInfo(email, password);
        string json = JsonUtility.ToJson(loginInfo);

        // 회원 정보와 캐릭터 정보를 가져와 
        // 정적 클래스에 각 저장한다.
        StartCoroutine(WebRequestScript.WebRequestPost("/game/user", json, (answer) =>
        {
            try
            {
                JObject jObject = JObject.Parse(answer);
                if (jObject["isUser"].ToString().Equals("true"))
                {
                    UserInfo.Email = jObject["userEmail"].ToString();
                    UserInfo.Nickname = jObject["userNickname"].ToString();
                    UnityAuthentication.UnityAuthenticationPw = jObject["UNITY_AUTHENTICATION"].ToString();
                    gameHighScoreHttpRequest.GetGameRanks();

                    if (jObject["isGameCharacter"].ToString().Equals("true"))
                    {
                        GameCharacterInfo.Email = jObject["userEmail"].ToString();
                        GameCharacterInfo.Nickname = jObject["gameCharacterNickname"].ToString();
                        GameCharacterInfo.HighScore = (long)jObject["gameCharacterHighScore"];

                        loginPanelScript.LoginActive(true, true);
                        characterPanelScript.SetActive(true);
                    }
                    else
                    {
                        loginPanelScript.LoginActive(true, true);
                        makeGameCharacterPanelScript.SetActive(true);
                    }
                }
                else if (jObject["isUser"].ToString().Equals("false"))
                {
                    Debug.Log("비밀번호 또는 아이디가 잘못 되었습니다.");
                    loginPanelScript.LoginActive(true, false);
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                loginPanelScript.LoginActive(false, false);
            }
        }));
    }
}

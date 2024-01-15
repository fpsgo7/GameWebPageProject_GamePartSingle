using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UpdateGameHighScore
{
    public string email;
    public long newScore;

    public UpdateGameHighScore(string email, long newScore)
    {
        this.email = email;
        this.newScore = newScore;
    }
}
public class GameHighScoreHttpRequest : MonoBehaviour
{
    private List<GameCharacterRankInfo> gameCharacterRankInfos = new List<GameCharacterRankInfo>();
    private CharacterRankScreenPanelScript characterRankScreenPanelScript;
    private CharacterPanelScript characterPanelScript;
    void Awake()
    {
        characterRankScreenPanelScript = GameObject.Find("LobbyScript").GetComponent<CharacterRankScreenPanelScript>();
        characterPanelScript = GameObject.Find("LobbyScript").GetComponent<CharacterPanelScript>();
    }

    public void GetGameRanks()
    {

        // 회원 정보와 캐릭터 정보를 가져와 
        // 정적 클래스에 각 저장한다.
        StartCoroutine(WebRequestScript.WebRequestGet("/game/gameHighScores", (answer) =>
        {
            try
            {
                gameCharacterRankInfos.Clear();
                JArray jArray = JArray.Parse(answer);
                for (int i = 0; i < jArray.Count; i++)
                {
                    GameCharacterRankInfo gameCharacterRankInfo = new GameCharacterRankInfo();
                    gameCharacterRankInfo.Rank = i + 1;
                    gameCharacterRankInfo.Email = jArray[i]["email"].ToString();
                    gameCharacterRankInfo.Nickname = jArray[i]["gameCharacterNickname"].ToString();
                    gameCharacterRankInfo.HighScore = (long)jArray[i]["highScore"];
                    gameCharacterRankInfo.LastedTime = (DateTime)jArray[i]["lastedTime"];
                    gameCharacterRankInfos.Add(gameCharacterRankInfo);
                }
                characterRankScreenPanelScript.SetActive(true, gameCharacterRankInfos);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);

            }
        }));
    }
    // 게임 최고 점수 설정은 향후 게임이 종료될 때 기능 사용하기 
    public void SetGameHighScore(long newScore)
    {
        UpdateGameHighScore updateGameHighScore = new UpdateGameHighScore(UserInfo.Email, newScore);
        string json = JsonUtility.ToJson(updateGameHighScore);

        StartCoroutine(WebRequestScript.WebRequestPost("/game/gameHighScore",json, (answer) =>
        {
            try
            {
                JObject jObject = JObject.Parse(answer);

                if (jObject["highScore"] != null)
                {
                    Debug.Log("게임 캐릭터 정보 업데이트가 성공하였습니다..");
                    GameCharacterInfo.HighScore = (long)jObject["highScore"];
                    characterPanelScript.SetActive(true);
                    GetGameRanks();
                }
                else
                {
                    Debug.Log("게임 캐릭터 정보 업데이트가 실패하였습니다.");
                }
            }
            catch (Exception e)
            {
                Debug.Log("웹과 통신에 실패하였습니다. \n" +e.Message);

            }
        }));
    }
}

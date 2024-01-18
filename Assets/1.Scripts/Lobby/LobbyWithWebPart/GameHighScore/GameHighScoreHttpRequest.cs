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
}

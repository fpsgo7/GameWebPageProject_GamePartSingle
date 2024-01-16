using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ForMainGameHttp : MonoBehaviour
{
    private bool once = true;
    // 게임이 끝나면 실행되며 점수를 업데이트한다.
    public void SetGameHighScore(long newScore)
    {
        
        // GameHighScoreHttpRequest 스크립트에 있는 클래스를 사용하여 객체를 생성한다.
        UpdateGameHighScore updateGameHighScore = new UpdateGameHighScore(UserInfo.Email, newScore);
        string json = JsonUtility.ToJson(updateGameHighScore);
        if (once)
        {
            StartCoroutine(WebRequestScript.WebRequestPost("/game/gameHighScore", json, (answer) =>
            {
                try
                {
                    JObject jObject = JObject.Parse(answer);

                    if (jObject["highScore"] != null)
                    {
                        Debug.Log("게임 캐릭터 정보 업데이트가 성공하였습니다..");
                        GameCharacterInfo.HighScore = (long)jObject["highScore"];
                    }
                    else
                    {
                        Debug.Log("게임 캐릭터 정보 업데이트가 실패하였습니다.");
                    }
                    once = false;
                }
                catch (Exception e)
                {
                    Debug.Log("웹과 통신에 실패하였습니다. \n" + e.Message);

                }
            }));
        }
    }
}

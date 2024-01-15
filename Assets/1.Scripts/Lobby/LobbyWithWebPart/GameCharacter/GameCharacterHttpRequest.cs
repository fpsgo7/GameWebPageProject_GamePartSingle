using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CreateGameCharacterinfo
{
    public string email;
    public string nickname;

    public CreateGameCharacterinfo(string email, string nickname)
    {
        this.email = email;
        this.nickname = nickname;
    }
}
public class GameCharacterHttpRequest : MonoBehaviour
{
    private MakeGameCharacterPanelScript makeGameCharacterPanelScript;
    private CharacterPanelScript characterPanelScript;

    void Awake()
    {
        makeGameCharacterPanelScript = GameObject.Find("LobbyScript").GetComponent<MakeGameCharacterPanelScript>();
        characterPanelScript = GameObject.Find("LobbyScript").GetComponent<CharacterPanelScript>();
    }

    public void CreateGameCharacter(string email,string nickname)
    {
        // INSERT INTO `mywebgameproject`.`gamecharacters` (`email`, `nickname`) ;
        CreateGameCharacterinfo createGameCharacterInfo 
            = new CreateGameCharacterinfo(email, nickname);
        string json = JsonUtility.ToJson(createGameCharacterInfo);

        // 회원 정보와 캐릭터 정보를 가져와 
        // 정적 클래스에 각 저장한다.
        StartCoroutine(WebRequestScript.WebRequestPost("/game/gameCharacter", json, (answer) =>
        {
            try
            {
                JObject jObject = JObject.Parse(answer);
               
                if (jObject["isGameCharacter"].ToString().Equals("true"))
                {
                    GameCharacterInfo.Email = jObject["userEmail"].ToString();
                    GameCharacterInfo.Nickname = jObject["gameCharacterNickname"].ToString();
                    GameCharacterInfo.HighScore = (long)jObject["gameCharacterHighScore"];
                    makeGameCharacterPanelScript.SetActive(false);
                    characterPanelScript.SetActive(true);
                }
                else
                {
                    Debug.Log("게임 생성이 원활하지 않습니다.");
                    makeGameCharacterPanelScript.CreateFail(true);
                }
               
            }
            catch (Exception e)
            {
                Debug.Log("게임 생성이 원활하지 않습니다.\n"+e.Message);
                makeGameCharacterPanelScript.CreateFail(true);
            }
        }));
    }
}

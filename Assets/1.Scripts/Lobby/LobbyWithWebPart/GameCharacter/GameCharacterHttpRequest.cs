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

        // ȸ�� ������ ĳ���� ������ ������ 
        // ���� Ŭ������ �� �����Ѵ�.
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
                    Debug.Log("���� ������ ��Ȱ���� �ʽ��ϴ�.");
                    makeGameCharacterPanelScript.CreateFail(true);
                }
               
            }
            catch (Exception e)
            {
                Debug.Log("���� ������ ��Ȱ���� �ʽ��ϴ�.\n"+e.Message);
                makeGameCharacterPanelScript.CreateFail(true);
            }
        }));
    }
}
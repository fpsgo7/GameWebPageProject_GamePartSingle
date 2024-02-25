using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GoogleChatManager : MonoBehaviour
{
    //구글의 스크립트 주소와 스프레드 시트의 주소를 변수에 미리입력
    const string spreadsheetURL = "https://docs.google.com/spreadsheets/d/1a7_wuIyY2IG426_mnyN1dvFX5yXZRgxejdRHvYxWBd8/export?format=tsv&range=B:B";
    const string spreadsheetAppScriptURL = "https://script.google.com/macros/s/AKfycbwj5dMeWGgsLKY2BD_JqgGCFhyv-c2jLmROKHsXMMlvkfdLvYoBzbKwd4PKzF4KPRWQ/exec";


    public Text chatText;//체팅 리스트를 올릴 공간
    public InputField  chatInput;//체팅을 입력하는 공간
    public string playerName;//플레잉어이름 가져오기

    private GameStartScript gameStartScript;

    void Awake()
    {
        gameStartScript = GameObject.Find("GameStartScript").GetComponent<GameStartScript>();
    }

    void Start()
    {
        StartCoroutine(GetAllChatText());//아닐경우 그대로
    }

    //HTTP 서버에 데이터 가져오기 보내기
    IEnumerator GetAllChatText()
    {
        UnityWebRequest unityWebRequest = UnityWebRequest.Get(spreadsheetURL);
        yield return unityWebRequest.SendWebRequest();//응답이 올때 까지 기다린다.

        string data = unityWebRequest.downloadHandler.text;//택스트를 가져옴
        chatText.text = data;//택스트를 텍스트창에 넣어줌

        StartCoroutine(GetAllChatText());//해당 함수를 계속 실행하게함
    }

    public void PostChat()
    {
        playerName = gameStartScript.GetPlayerName();
        WWWForm wwwForm = new WWWForm();//페이로드에 넣을 곳을 만든다.
        wwwForm.AddField("nickname", playerName);//페이로드에 정보를 넣음(필드명, 데이터)
        wwwForm.AddField("chat", chatInput.text);

        StartCoroutine(Post(wwwForm));//페이로드를 매계변수로 하는 함수를 사용함
    }


    IEnumerator Post(WWWForm wwwform)
    {
        //실질적으로 보낼주소로 매계변수로 받은 페이로드를 넣는다.
        using (UnityWebRequest unityWebRequest = UnityWebRequest.Post(spreadsheetAppScriptURL, wwwform)) // 반드시 using을 써야한다
        {
            yield return unityWebRequest.SendWebRequest();//응답 대기 시간
        }
    }
}

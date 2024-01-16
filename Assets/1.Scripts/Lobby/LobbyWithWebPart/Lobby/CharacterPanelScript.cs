using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPanelScript : MonoBehaviour
{
    private UICharacterScreenPanel uICharacterScreenPanel;
    private GameStartScript gameStartScript;

    void Awake()
    {
        uICharacterScreenPanel = GameObject.Find("PanelObjectScript").GetComponent<UICharacterScreenPanel>();
        gameStartScript = GameObject.Find("GameStartScript").GetComponent<GameStartScript>();
    }

    public void SetActive(bool isBool)
    {
        // 패널 비활성화 시에도 실행되도 문제 없기에 따로 if문 구분을 하지 않는다.
        uICharacterScreenPanel.userNicknameText.text = UserInfo.Nickname;
        uICharacterScreenPanel.gameCharacterNicknameText.text = GameCharacterInfo.Nickname;
        uICharacterScreenPanel.scoreText.text = GameCharacterInfo.HighScore.ToString();

        gameStartScript.SetPlayerName(GameCharacterInfo.Nickname);

        uICharacterScreenPanel.SetActive(isBool);
    }
}

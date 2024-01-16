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
        // �г� ��Ȱ��ȭ �ÿ��� ����ǵ� ���� ���⿡ ���� if�� ������ ���� �ʴ´�.
        uICharacterScreenPanel.userNicknameText.text = UserInfo.Nickname;
        uICharacterScreenPanel.gameCharacterNicknameText.text = GameCharacterInfo.Nickname;
        uICharacterScreenPanel.scoreText.text = GameCharacterInfo.HighScore.ToString();

        gameStartScript.SetPlayerName(GameCharacterInfo.Nickname);

        uICharacterScreenPanel.SetActive(isBool);
    }
}

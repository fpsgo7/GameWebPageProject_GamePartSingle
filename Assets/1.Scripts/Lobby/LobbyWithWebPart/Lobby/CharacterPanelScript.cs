using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPanelScript : MonoBehaviour
{
    private UICharacterScreenPanel uICharacterScreenPanel;

    void Awake()
    {
        uICharacterScreenPanel = GameObject.Find("PanelObjectScript").GetComponent<UICharacterScreenPanel>();
    }

    public void SetActive(bool isBool)
    {
        // �г� ��Ȱ��ȭ �ÿ��� ����ǵ� ���� ���⿡ ���� if�� ������ ���� �ʴ´�.
        uICharacterScreenPanel.userNicknameText.text = UserInfo.Nickname;
        uICharacterScreenPanel.gameCharacterNicknameText.text = GameCharacterInfo.Nickname;
        uICharacterScreenPanel.scoreText.text = GameCharacterInfo.HighScore.ToString();

        uICharacterScreenPanel.SetActive(isBool);
    }
}

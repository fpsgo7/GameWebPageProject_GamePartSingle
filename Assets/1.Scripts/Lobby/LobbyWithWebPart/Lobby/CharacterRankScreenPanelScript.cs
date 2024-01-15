using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRankScreenPanelScript : MonoBehaviour
{
    private UICharacterRankScreenPanel uICharacterRankScreenPanel;
    private List<GameObject> gameCharacterRankObjects = new List<GameObject>();

    void Awake()
    {
        uICharacterRankScreenPanel = GameObject.Find("PanelObjectScript").GetComponent<UICharacterRankScreenPanel>();
    }

    public void SetActive(bool isBool, List<GameCharacterRankInfo> gameCharacterRankInfos)
    {
        // true 즉 패널을 활성화 하는 경우에만 랭크 불러오기 작업을 실행하게한다.
        if (isBool)
        {
            for (int i = 0; i < gameCharacterRankInfos.Count; i++)
            {
                if (i >= gameCharacterRankObjects.Count)
                {
                    GameObject rankGameObject
                    = Instantiate<GameObject>(uICharacterRankScreenPanel.gameCharacterRankPrefab, uICharacterRankScreenPanel.gridSetting.transform);
                    gameCharacterRankObjects.Add(rankGameObject);
                }
                UICharacterRankPanel rankItem = gameCharacterRankObjects[i].GetComponent<UICharacterRankPanel>();
                rankItem.Init(
                    gameCharacterRankInfos[i].Rank + "",
                    gameCharacterRankInfos[i].Email,
                    gameCharacterRankInfos[i].Nickname,
                    gameCharacterRankInfos[i].HighScore + ""
                    );
            }
        }

        uICharacterRankScreenPanel.SetActive(isBool);

    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 캐릭터들의 순위를 보여주기위한 패널의 오브젝트를 연결해주는
/// 스크립트
/// </summary>
public class UICharacterRankScreenPanel : MonoBehaviour
{
    public GameObject charactersScreenPanel;
    public GameObject gridSetting;
    public GameObject gameCharacterRankPrefab;

    public void SetActive(bool isBool)
    {
        charactersScreenPanel.SetActive(isBool);
    }
}

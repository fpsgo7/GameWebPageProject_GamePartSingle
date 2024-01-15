using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 자신의 캐릭터를 보여주기위한 
/// 패널의 오브젝트들을 접근하기위한 
/// 스크립트
/// </summary>
public class UICharacterScreenPanel : MonoBehaviour
{
    public Text userNicknameText;
    public Text gameCharacterNicknameText;
    public Text scoreText;
    public GameObject characterScreenPanel;

    public void SetActive(bool isBool)
    {
        characterScreenPanel.SetActive(isBool);
    }
}

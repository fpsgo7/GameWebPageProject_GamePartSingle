using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// ĳ���͵��� ������ �����ֱ����� �г��� ������Ʈ�� �������ִ�
/// ��ũ��Ʈ
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
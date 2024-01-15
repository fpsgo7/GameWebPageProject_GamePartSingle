using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 캐릭터 생성 패널 오브젝트에 접근하기위한 공간
/// </summary>
public class UIMakeGameCharacterPanel : MonoBehaviour
{
    public Button createGameCharacterButton;
    public InputField nicknameInputField;
    public GameObject makeGameCharacterPanel;
    public GameObject createFaildPanel;

    public void SetActive(bool isBool)
    {
        makeGameCharacterPanel.SetActive(isBool);
    }
    public string getNicknameInputField_Text()
    {
        return nicknameInputField.text;
    }
}

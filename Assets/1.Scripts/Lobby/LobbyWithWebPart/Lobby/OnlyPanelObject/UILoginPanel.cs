using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///  로그인 패널의 UI에 접근하기위한 공간
/// </summary>
public class UILoginPanel : MonoBehaviour
{
    public Button loginButton;
    public InputField emailInpuField;
    public InputField passwordInputField;
    public GameObject loginServerErrorPanel;
    public GameObject loginInfoErrorPanel;
    public GameObject loginPanel;

    public void SetActive(bool isBool)
    {
        loginPanel.SetActive(isBool);
    }
    public string GetEmailInputField_Text()
    {
        return emailInpuField.text;
    }

    public string GetPasswordInputField_Text()
    {
        return passwordInputField.text;
    }

    
}

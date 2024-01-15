using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 로그인 패널을 다룬다.
/// </summary>
public class LoginPanelScript : MonoBehaviour
{
    private UILoginPanel loginPanel;

    void Awake()
    {
        loginPanel = GameObject.Find("PanelObjectScript").GetComponent<UILoginPanel>();
    }

    public void LoginActive(bool isActive, bool isLogin)
    {
        if (isActive)
        {
            if (isLogin)
            {
                loginPanel.SetActive(false);
            }
            else
            {
                loginPanel.loginInfoErrorPanel.SetActive(true);
            }
        }
        else
        {
            loginPanel.loginServerErrorPanel.SetActive(true);
        }
    }

    public string GetEmailText()
    {
        return loginPanel.GetEmailInputField_Text();

    }

    public string GetPasswordText()
    {
        return loginPanel.GetPasswordInputField_Text();
    }

}

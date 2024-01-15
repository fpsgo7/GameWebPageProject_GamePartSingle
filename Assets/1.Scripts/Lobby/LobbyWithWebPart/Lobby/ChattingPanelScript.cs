using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChattingPanelScript : MonoBehaviour
{
    private UIChattingPanel uIChattingPanel;
    void Awake()
    {
        uIChattingPanel = GameObject.Find("PanelObjectScript").GetComponent<UIChattingPanel>();
    }
    
    public void SetActive(bool isBool)
    {
        uIChattingPanel.SetActive(isBool);
    }
}

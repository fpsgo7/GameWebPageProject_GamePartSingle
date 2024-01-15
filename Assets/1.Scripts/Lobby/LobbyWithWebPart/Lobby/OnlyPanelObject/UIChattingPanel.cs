using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIChattingPanel : MonoBehaviour
{
    public GameObject charttingPanel;

    public void SetActive(bool isBool)
    {
        charttingPanel.SetActive(isBool);
    }
}

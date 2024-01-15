using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameRoomsPanel : MonoBehaviour
{
    public GameObject gameRoomsPanel;
    public GameObject gameRankOpenButton;
    public GameObject gameChatOpenButton;


    public void SetActive(bool isBool)
    {
        gameRoomsPanel.SetActive(isBool);
    }
}

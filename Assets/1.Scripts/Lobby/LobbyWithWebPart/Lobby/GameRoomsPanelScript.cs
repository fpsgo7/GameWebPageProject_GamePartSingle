using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameRoomsPanelScript : MonoBehaviour
{
    private UIGameRoomsPanel uIGameRoomsPanel;

    void Awake()
    {
        uIGameRoomsPanel = GameObject.Find("PanelObjectScript").GetComponent<UIGameRoomsPanel>();
    }

    public void SetActive(bool isBool)
    {
        uIGameRoomsPanel.SetActive(isBool);
    }

    public void GameRankOpenButtonInteractable(bool isBool)
    {
        uIGameRoomsPanel.gameRankOpenButton.GetComponent<Button>().interactable = isBool;
    }
    public void GameChatOpenButtonInteractable(bool isBool)
    {
        uIGameRoomsPanel.gameChatOpenButton.GetComponent<Button>().interactable = isBool;
    }
}

using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameStartScript : MonoBehaviour
{
    //게임 상태
    public string playerName=null;

    public void GameStart()
    {
        SceneManager.LoadScene("MainGame");
    }

    public void SetPlayerName(string playerName)
    {
        this.playerName = playerName;
    }
    public string GetPlayerName()
    {
        return playerName;
    }
}

using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameStartScript : MonoBehaviour
{
     public static GameStartScript instance
    {
        get
        {
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (m_instance == null)
            {
                // 씬에서 GameManager 오브젝트를 찾아 할당
                m_instance = FindObjectOfType<GameStartScript>();
            }

            // 싱글톤 오브젝트를 반환
            return m_instance;
        }
    }
    private static GameStartScript m_instance; // 싱글톤이 할당될 static 변수
    //게임 상태
    public string userId = "user";//유저아이디
    public static string playerName=null;

    //로그인 시작하기 
   
    public void OnLogin()//1.로그인 버튼 클릭
    {
        playerName = null;
        SceneManager.LoadScene("MainGame");
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // 씬 관리자 관련 코드
using UnityEngine.UI; // UI 관련 코드

// 필요한 UI에 즉시 접근하고 변경할 수 있도록 허용하는 UI 매니저
public class UIManager : MonoBehaviour { 
    // 싱글톤 접근용 프로퍼티
    public static UIManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<UIManager>();
            }

            return m_instance;
        }
    }
    private static UIManager m_instance; // 싱글톤이 할당될 변수

    public Text ammoText; // 탄약 표시용 텍스트
    public Text scoreText; // 점수 표시용 텍스트
    public Text waveText; // 적 웨이브 표시용 텍스트
    public Text grenadeText;//수류탄 표시용 텍스트
    public GameObject gameoverUI; // 게임 오버시 활성화할 UI 
    public GameObject restartButton;//재시작 버튼

    // 탄약 텍스트 갱신 PlayerShooter에서 호출되어 사용됨
    public void UpdateAmmoText(int magAmmo, int remainAmmo) {
        ammoText.text = magAmmo + "/" + remainAmmo;
    }
    //수류탄 텍스트 갱신 FireGrenade에서 호출되 사용됨
    public void UpdateGremadeText(int grenadeAmmo)
    {
        grenadeText.text = ""+grenadeAmmo;
    }
    // 점수 텍스트 갱신
    public void UpdateScoreText(int newScore) {
        scoreText.text = "Score : " + (newScore+ GameManager.instance.hitScorePlayerMaster);
    }

    // 적 웨이브 텍스트 갱신 SpawnManager에서 호출되어 사용됨
    public void UpdateWaveText(int waves, int count) {
        waveText.text = "Round : " + waves + "\nEnemy Left : " + count;
    }

    // 게임 오버 UI 활성화 gameManager에서 호출됨
    public void SetActiveGameoverUI(bool active) {
        gameoverUI.SetActive(active);
        
            //마스터 클라이언트에서만 버튼이 나오게함
            restartButton.SetActive(true);
        
    }
    //유니티 버튼에서 이벤트 로 작동함
    public void GameRestart()
    {
        ReloaSceneCor();
        
    }
    //게임 제시작 함수
    private void ReloaSceneCor()
    {
        SceneManager.LoadScene("MainGame");
    }
    
}
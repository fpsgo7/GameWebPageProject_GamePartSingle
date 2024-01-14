using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpawnManager : MonoBehaviour
{
    public int wave=1; // 현재 웨이브
    public int enemyCount = 0; // 남은 적의 수

    public bool createOnce=false;

    public void Update()
    {
        if (enemyCount <= 0 && createOnce == true)
        {
            wave++;
            createOnce = false;
        }
        // UI 갱신
        UpdateUI();
        //해당 스크립트를 비활성화하여 2라운드 까지만 생성해주는 역할을 해준다.
        if (wave == 2)
        {
            SpawnChange1();
        }
    }
    //다른 플레이어도 저용하기위하여 PunRPC사용
    
    private void SpawnChange1()
    {
        GetComponent<EnemySpawn>().enabled = false;
        GetComponent<EnemySpawnL1>().enabled = true;
    }
    // 웨이브 정보를 UI로 표시
    
    private void UpdateUI()
    {
        UIManager.instance.UpdateWaveText(wave, enemyCount);
    }
}

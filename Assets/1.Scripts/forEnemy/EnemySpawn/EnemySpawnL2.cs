using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ExitGames.Client.Photon;
using System.Collections;

// 적 게임 오브젝트를 주기적으로 생성
// 여기서는 적을 생성하기만하고 적 표시 등 여러 기능은 EnemySpawnL1에 위임한다.
public class EnemySpawnL2 : MonoBehaviour
{
    public EnemyAIL2 enemyPrefab; // 생성할 적 AI

    public Transform[] spawnPoints; // 적 AI를 소환할 위치들

    private List<EnemyAIL2> enemies = new List<EnemyAIL2>(); // 생성된 적들을 담는 리스트

    public int enemyCount;
    
    //각 라운드별로 적 2명 생성
    public void SpawnWave()
    {
        //각 라운드 마다 적 2명 생성
        int spawnCount = 1;

        // spawnCount 만큼 적을 생성
        for (int i = 0; i <= spawnCount; i++)
        {
            CreateEnemy();
        }
    }

    // 적을 생성하고 생성한 적에게 추적할 대상을 할당
    private void CreateEnemy()
    {
        // 생성할 위치를 랜덤으로 결정
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // 적 프리팹으로부터 적 생성
        GameObject createdEnemy = Instantiate(enemyPrefab.gameObject, spawnPoint.position, spawnPoint.rotation);
        EnemyAIL2 enemy = createdEnemy.GetComponent<EnemyAIL2>();
        // 생성된 적을 리스트에 추가
        enemies.Add(enemy);
        //생성된 적수 넣기
        enemyCount = enemies.Count;

        // 적의 onDeath 이벤트에 익명 메서드 등록
        // 사망한 적을 리스트에서 제거
        enemy.onDeath += () => enemies.Remove(enemy);
        // 사망한 적을 3초 뒤에 파괴
        enemy.onDeath += () => StartCoroutine(DestroyAfter(enemy.gameObject, 3f));
        // 적 사망시 점수 상승
        enemy.onDeath += () => GameManager.instance.AddScore(100);
        //적사망시 enemyCount의 숫자를 1줄여 적이 죽은 걸표현함
        enemy.onDeath += () => enemyCount = enemies.Count;
    }
    
    // 포톤의 Network.Destroy()는 지연 파괴를 지원하지 않으므로 지연 파괴를 직접 구현함
    IEnumerator DestroyAfter(GameObject target, float delay)
    {
        // delay 만큼 쉬고
        yield return new WaitForSeconds(delay);

        // target이 아직 파괴되지 않았다면
        if (target != null)
        {
                // target을 모든 네트워크 상에서 파괴
                Destroy(target);
        }
    }
    
}
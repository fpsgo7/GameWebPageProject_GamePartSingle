using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.Collections;

// 적 게임 오브젝트를 주기적으로 생성
public class EnemySpawn : MonoBehaviour//, IPunObservable
{
    public EnemyAI enemyPrefab; // 생성할 적 AI
    public Transform[] spawnPoints; // 적 AI를 소환할 위치들
    private List<EnemyAI> enemies = new List<EnemyAI>(); // 생성된 적들을 담는 리스트
    
    private SpawnManager spawnManager;
    public void OnEnable()
    {
        spawnManager=GetComponent<SpawnManager>();
    }
    private void Update()
    {
        
            spawnManager.enemyCount = enemies.Count;//적들남은수를 공유하기위하여 사용
            // 게임 오버 상태일때는 생성하지 않음
            if (GameManager.instance != null && GameManager.instance.isGameover)
            {
                return;
            }
           
            // 적을 모두 물리친 경우 
            if (spawnManager.enemyCount <= 0 && spawnManager.createOnce==false)
            {
                SpawnWave();
            }
        
    }
    // 현재 웨이브에 맞춰 적을 생성
    private void SpawnWave()
    {
        // 현재 웨이브 에 +1한만큼 적생성
        int spawnCount = Mathf.RoundToInt(spawnManager.wave + 1);
        // spawnCount 만큼 적을 생성
        for (int i = 0; i < spawnCount; i++)
        {
            CreateEnemy();
        }
        spawnManager.enemyCount = enemies.Count;//밑의 조건 문제로 먼저 사용
        spawnManager.createOnce = true;
    }

    // 적을 생성하고 생성한 적에게 추적할 대상을 할당
    private void CreateEnemy()
    {
        // 생성할 위치를 랜덤으로 결정
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // 적 프리팹으로부터 적 생성
        GameObject createdEnemy = Instantiate(enemyPrefab.gameObject, spawnPoint.position, spawnPoint.rotation);
        EnemyAI enemy = createdEnemy.GetComponent<EnemyAI>();
        // 생성된 적을 리스트에 추가
        enemies.Add(enemy);

        // 적의 onDeath 이벤트에 익명 메서드 등록
        // 사망한 적을 리스트에서 제거
        //람다식 방식으로 자주호출되는 곳에서는 사용하지 말것!
        enemy.onDeath += () => enemies.Remove(enemy);
        // 사망한 적을 3초 뒤에 파괴
        enemy.onDeath += () => StartCoroutine(DestroyAfter(enemy.gameObject, 3f));
        // 적 사망시 점수 상승
        enemy.onDeath += () => GameManager.instance.AddScore(100);
    }
    
    // 포톤의 Network.Destroy()는 지연 파괴를 지원하지 않으므로 지연 파괴를 직접 구현함
    IEnumerator DestroyAfter(GameObject target, float delay)
    {
        // delay 만큼 쉬고
        yield return new WaitForSeconds(delay);//r객체를 만듬

        // target이 아직 파괴되지 않았다면
        if (target != null)
        {
                // target을 모든 네트워크 상에서 파괴
                Destroy(target);
        }
    }
  
}
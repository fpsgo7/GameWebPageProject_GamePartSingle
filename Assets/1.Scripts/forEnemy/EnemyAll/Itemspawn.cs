using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//적이 죽으면 아이템을 떨군다.
public class Itemspawn : MonoBehaviour
{
    public GameObject[] items; 
    public Transform enemyTr;
    public bool isSpawned = true;

    // Start is called before the first frame update
    private void Start()
    {
        enemyTr = GetComponent<Transform>();//스크립트를 가진 대상의 위치값을 가져옴
    }
    public void Spawn()//다른 스크립트에서 접근하므로 public 필요
    {
       
        // 생성할 아이템을 무작위로 하나 선택
        GameObject itemToCreate = items[Random.Range(0, items.Length)];
        // 아이템 중 하나를 무작위로 골라 랜덤 위치에 포톤 형식으로 생성합니다.
        GameObject item = Instantiate(itemToCreate, enemyTr.position, Quaternion.identity);
    }
}

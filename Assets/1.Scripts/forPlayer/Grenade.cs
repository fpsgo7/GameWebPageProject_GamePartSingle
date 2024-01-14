using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float speed = 50.0f;//수류탄 속도
    public GameObject explosion;
    private int damage = 50;
    private RaycastHit[] rayHits;
    private void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * speed);
        Invoke("ExplosionGrenade",1.0f);
    }

    private void ExplosionGrenade()
    {
        
        //폭발범위에 들어간 적들을 찾아냄
        rayHits = Physics.SphereCastAll(transform.position,
            10,
            Vector3.up, 0f,
            LayerMask.GetMask("enemy"));
        ExplosionAttack();
        GameObject obj = (GameObject)Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(obj, 1.0f);
        Destroy(this.gameObject,1.0f);
    }
    private void ExplosionAttack()
    {
       
        //폭파 범위안의 있는 적들을 반복문으로 접근함
        foreach(RaycastHit hitobj in rayHits)
        {
                GameManager.instance.hitScorePlayerMaster += 5;
            if (hitobj.transform.GetComponent<EnemyHealth>())
                hitobj.transform.GetComponent<EnemyHealth>().OnDamage(damage);
        }
    }
}

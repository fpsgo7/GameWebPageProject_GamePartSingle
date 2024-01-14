using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGrenade : MonoBehaviour
{
    public Transform firePos;
    public int grenadeAmmo =10;
    public float timeBetFire = 3f; // 수류탄 발사 간격
    private float lastFireTime; // 수류탄 마지막으로 발사한 시점
    private Animator playerAnimator; // 플레이어의 애니메이터                                
    private readonly int hashGrenade = Animator.StringToHash("Grenade");//Animator의 Grenade 트리거를 가져온다. 
    private GameObject grenade;
    public GameObject grenadeBullet;

    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        grenadeAmmo = 10;
    }
    public void Fire()
    {
        //수류탄 발사 딜레이와 발사 조건
        if (grenadeAmmo >= 1 &&Time.time >=lastFireTime +timeBetFire)
        {
            FireAnimation();
            --grenadeAmmo;
            UIManager.instance.UpdateGremadeText(grenadeAmmo);
            lastFireTime = Time.time;
        }
    }
    private void FireAnimation()
    {
        PlayerShooter.instance.ikActive = false;//iKActive를 수정하여 총을 놓게함
        playerAnimator.SetTrigger(hashGrenade);
    }
    public void FireProcess()//애니메이션 동작 이벤트에서 작동
    {
            grenade = Instantiate(grenadeBullet, 
                firePos.transform.position, firePos.transform.rotation);
    }
    public void FireEnd()//애니메이션 동작 이벤트에서 작동
    {
            PlayerShooter.instance.ikActive = true;
    }
    
}

using UnityEngine;
using UnityEngine.UI; // UI 관련 코드
using Cinemachine;
using System.Collections;
using TMPro;

// 플레이어 캐릭터의 생명체로서의 동작을 담당
public class PlayerHealth : LivingEntity {
    public GameObject healthSlider; // 체력을 표시할 UI 슬라이더
    public GameObject otherPlayerHealthSlider;//다른 플레이어의 체력을 표현할 슬라이더
    public GameObject myBody;//자신의 몸체의 테그를 접근하기위해 사용할 변수
    
    public Slider recoverSlider;//부활 차는것을 보여준다.

    public AudioClip deathClip; // 사망 소리
    //public AudioClip hitClip; // 피격 소리
    public AudioClip itemPickupClip; // 아이템 습득 소리

    private AudioSource playerAudioPlayer; // 플레이어 소리 재생기
    private Animator playerAnimator; // 플레이어의 애니메이터

    private PlayerMovement playerMovement; // 플레이어 움직임 컴포넌트
    private PlayerShooter playerShooter; // 플레이어 슈터 컴포넌트
    private PlayerInput playerInput;

    public TextMeshProUGUI nickName;//플레이어의 닉네임 연결
    //public GameObject otherPlayerNickName;//다른 플레이어의 닉네임 연결

    //부활을 위한 변수
    public float healTime;
    public bool once =false;// 한번만 부활하기위한 bool
    //탈출을 위한 변수
    public bool escape = false;
    public bool escapeOnce = true;
    public GameObject escapeRope;
    public GameObject gun;
    public GameObject escapeCamera;
    public GameObject playerNamePanel;
    public Billboard billboard;
    public LayerSetting layerSetting;
    public Transform gunForEscape;
    private float upRopeSpeed = 3f;
    private Vector3 playerV3;
    
    private void Awake() {
        // 사용할 컴포넌트를 가져오기
        playerAnimator = GetComponent<Animator>();
        playerAudioPlayer = GetComponent<AudioSource>();

        playerMovement = GetComponent<PlayerMovement>();
        playerShooter = GetComponent<PlayerShooter>();
        playerInput = GetComponent<PlayerInput>();
        layerSetting = GetComponent<LayerSetting>();

        nickName.text = GameManager.instance.myPlayerName;//닉네임 연결
       
        // 나의 헬스바는 로컬 플레이어만 사용가능하게하여 곂치는 현상을 예방한다.
      
#if UNITY_ANDROID
            healthSlider = GameObject.Find("MyMobileHealthBar");
#endif
#if UNITY_STANDALONE
            healthSlider = GameObject.Find("MyHealthBar");
#endif

            myBody.gameObject.tag = "Player1";

    }
    private void Update()
    {
        if (GameManager.instance.isClear == true )
        {
            
            if (playerV3.y >= 40)
            {
                //특정 위치에 올라가면 작동
                //LayerSetting문제때문에 사용

                GetComponent<Rigidbody>().useGravity = false;
                //renderer을 비활성화해서 캐릭터를 안보이게 한다.
                myBody.GetComponent<SkinnedMeshRenderer>().enabled = false;
                //총을 안보이게함
                gun.transform.GetChild(5).gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = false;
                gun.transform.GetChild(5).gameObject.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().enabled = false;
                gun.transform.GetChild(5).gameObject.transform.GetChild(2).gameObject.GetComponent<MeshRenderer>().enabled = false;
                escapeRope.SetActive(false);
               
                    escapeCamera.SetActive(false);
                    GameManager.instance.helicopterCamera.SetActive(true);
                
                GameManager.instance.helicopter.GetComponent<HelicopterMove>().enabled = true;
                GameManager.instance.helicopter.GetComponent<BoxCollider>().enabled = true;
                
            }
            else
            {
                if (escapeOnce)
                {
                    playerNamePanel.SetActive(false);
                    //오류를 일으키는 안쓰는 스크립트는 꺼둔다.
                    layerSetting.enabled = false;
                    billboard.enabled = false;
                   
                        //메인카메라 종료
                        Camera.main.GetComponent<Camera>().enabled = false;
                        //탈출카메라 작동
                        escapeCamera.SetActive(true);
                    
                    escapeOnce = false;
                }
                //올라가기
                
                playerV3.y += upRopeSpeed * Time.deltaTime;
                playerV3.x = transform.position.x;
                playerV3.z = transform.position.z;
                transform.position = playerV3;
            }
           
        }
        

    }
    protected override void OnEnable() {
        // LivingEntity의 OnEnable() 실행 (상태 초기화)
        base.OnEnable();
            // 체력 슬라이더 활성화
            healthSlider.gameObject.SetActive(true);
            // 체력 슬라이더의 최대값을 기본 체력값으로 변경
            healthSlider.GetComponent<Slider>().maxValue = startingHealth;
            // 체력 슬라이더의 값을 현재 체력값으로 변경
            healthSlider.GetComponent<Slider>().value = currentHealth;

        // 플레이어 조작을 받는 컴포넌트들 활성화
        playerMovement.enabled = true;
        playerShooter.enabled = true;
    }

    // 체력 회복
    public override void RestoreHealth(float newHealth) {
        // LivingEntity의 RestoreHealth() 실행 (체력 증가)
        base.RestoreHealth(newHealth);
        healthSlider.GetComponent<Slider>().value = currentHealth;
      
    }


    // 데미지 처리
    public override void OnDamage(float damage, Vector3 hitPoint,
        Vector3 hitDirection) {
        if (!isDead)
        {
            // 사망하지 않은 경우에만 효과음을 재생
            //playerAudioPlayer.PlayOneShot(hitClip);
        }

        // LivingEntity의 OnDamage() 실행(데미지 적용)
        base.OnDamage(damage, hitPoint, hitDirection);
            //Debug.Log(damage+" "+currentHealth);//hp 출력 필요할 경우 
            // 갱신된 체력을 체력 슬라이더에 반영
            healthSlider.GetComponent<Slider>().value = currentHealth;
    }
    //  총알이 아닌 데미지 처리
    public override void OnDamage(float damage)
    {
        // LivingEntity의 OnDamage() 실행(데미지 적용)
        base.OnDamage(damage);
            Debug.Log(damage+" "+currentHealth);//hp 출력 필요할 경우 
            // 갱신된 체력을 체력 슬라이더에 반영
            healthSlider.GetComponent<Slider>().value = currentHealth;
    }

    // 사망 처리
    public override void Die() {
        // LivingEntity의 Die() 실행(사망 적용)
        base.Die();

        // 사망음 재생
        playerAudioPlayer.PlayOneShot(deathClip);
        // 애니메이터의 Die 트리거를 발동시켜 사망 애니메이션 재생
        playerAnimator.SetTrigger("Die");

        // 플레이어 조작을 받는 컴포넌트들 비활성화
        playerMovement.enabled = false;
        playerShooter.enabled = false;
        //치료 슬라이더 활성화
        recoverSlider.gameObject.SetActive(true);
        GameManager.instance.deathCount++;
        once = true;//죽으면 true로 하여 부활 을 사용할수 있게한다.
    }
    // 부활 처리
    public void Respawn()
    {
        recoverSlider.gameObject.SetActive(false);//치료 슬라이더 비활성화
        healTime = 0;//부활이 되면 부활 시간 초기화

        // 컴포넌트들을 리셋하기 위해 게임 오브젝트를 잠시 껐다가 다시 켜기
        // 컴포넌트들의 OnDisable(), OnEnable() 메서드가 실행됨
        gameObject.SetActive(false);
        gameObject.SetActive(true);
        
        isDead = false;//완전히 부활하기 전까지는 대미지를 받으면 안되기에 dead를 여기서 false로 해준다.
    }
    private void OnTriggerEnter(Collider other) {
        // 아이템과 충돌한 경우 해당 아이템을 사용하는 처리
        // 사망하지 않은 경우에만 아이템 사용가능
        //사망하지 않은 경우에만 팀원 살리기 가능
        if (!isDead)
        {
            // 충돌한 상대방으로 부터 Item 컴포넌트를 가져오기 시도
            IItem item = other.GetComponent<IItem>();

            // 충돌한 상대방으로부터 Item 컴포넌트가 가져오는데 성공했다면
            if (item != null)
            {
                
                    item.Use(gameObject);
                

                // 아이템 습득 소리 재생
                playerAudioPlayer.PlayOneShot(itemPickupClip);
            }

           
        }
    }
    
    //탈출구역에 도착하면 탈출 카운트 증가
    public void escapeAreaIn()
    {
        if (!escape)
        {
            escape = true;
            GameManager.instance.escapeCount++;
            gun.transform.position = gunForEscape.position;
            gun.transform.rotation = gunForEscape.rotation;
            playerShooter.escape = true;//IK값을 수정
            playerAnimator.SetTrigger("Escape");//줄잡기 동작 실행
            playerMovement.moveSpeed = 0;//모바일 버전
            playerInput.moveSpeed = 0;//pc 버전 속도 
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFireAll : MonoBehaviour
{
    //컴포넌트를 추출하기위한 변수 선언
    protected AudioSource enemyAudio;
    protected Animator enemyAnimator;
    protected Transform playerTr;
    protected Transform enemyTr;

    //Animator의 Fire 트리거를 가져온다. 
    protected readonly int hashFire = Animator.StringToHash("Fire");
    //Animator의 Reload 트리거를 가져온다.
    protected readonly int hashReload = Animator.StringToHash("Reload");
     //MuzzleFlash 를 저장할 MeshRenderer를 선언한다.
    public MeshRenderer muzzleFlash;
    //총을 발사하고 딜레이를 주기위한 변수들
    protected float nextFire;
    protected float fireRate;
    protected float damping;//적의 회전속도

    protected float reloadTime;//제장전시간
    public int maxBullet;//최대 탄수
    public int currBullet;//현제 탄수
    public bool isReload = false;//재장전상태
    public bool death = false;//죽음상탠
    protected float fireDistance = 50f; // 사정거리
    protected float damage;//데미지

    //코루틴 시간을 저장할 변수
    protected WaitForSeconds wsReload;

    //isFire 가 활성화 되있으면 총을 발사한다.
    public bool isFire = false;

    public AudioClip fireSfx;//총소리를 저장할 변수(Audio Clip)
    public AudioClip reloadSfx;//장전 사운드를 저장할 변수

    protected LineRenderer bulletLineRenderer; // 총알 궤적을 그리기 위한 렌더러
    public Transform firePos;// 총알 발사위치를 위한변수

    protected  virtual void Awake()
    {
        enemyTr = GetComponent<Transform>();
        enemyAnimator = GetComponent<Animator>();
        enemyAudio = GetComponent<AudioSource>();
        //Start에서 총알 발사 위치를 연결하면 값이 천천히 들어가
        //null 오류가 발생하므로 Awake를 사용한다.
        //총알 발사위치 연결
        bulletLineRenderer = GetComponent<LineRenderer>();
        // 사용할 점을 두개로 변경
        bulletLineRenderer.positionCount = 2;
        // 라인 렌더러를 비활성화
        bulletLineRenderer.enabled = false;
       
    }
    //추적 대상을 각각 다른스크립트에서 찾으면 오류날수있어 
    //다른 추적대상에서 얻은 추적대상을 가져와 적용한다.
    public virtual void findPlayer(GameObject targetPlayer)
    {   //playerTr의 값을 구하여 적이 플레이어를 조준할수 있게함
        //playerTr의 값은 Enemy 스크립트를 상속받은 대상에서 실행한다.
        playerTr = targetPlayer.transform;
    }

    public virtual void Update()
    {
        //죽음상태가 true인경우 반복문을 실행하지 못하게함
        if (death == true)
        {
            enemyAnimator.SetBool(hashFire, false);
        }
        else
        {
           
                //실질적인발사
                if (!isReload && isFire)
                {
                    //Time.time은 스크립트가 실행됐을때부터 흘러가는 시간이며 nextFire에는 Time.time+딜레이 시간이 들어간다.
                    if (Time.time >= nextFire)
                    {
                        Fire();
                        //Fire트리거를 활성화 하고 총소리를 플레이해준다.
                        enemyAnimator.SetBool(hashFire, true);
                        //랜덤한 딜레이을 위해 Random 함수를 사용하며 상단의 using System;을 지워 사용한다.
                        nextFire = Time.time + fireRate + Random.Range(0.0f, 0.3f);
                    }
                    //쿼터니언 을 이용하여 플레이어를 바라보게 한다.(A벡터 -B 벡터 ) =B 좌표에서 A 좌표로 가는 벡터를 나타낸것이다.
                    Quaternion rot = Quaternion.LookRotation(playerTr.position - enemyTr.position);
                    //보간함수Slerp를 이용해서 점진적으로 회전시킨다 Slerp는 a각도에서 b각도 사이를 시간 t에 따라 점진적으로 반환하는 함수 이다.
                    enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot, Time.deltaTime * damping);
                }
                else
                {
                    enemyAnimator.SetBool(hashFire, false);
                }
            
        }
    }

    public virtual void Fire()
    {
        // 실제 발사 처리는 호스트에게 대리
       
        ShotProcessOnServer();
        //현재 총알에서 1을 뺀 수를 최대 총알수로 하며
        //최대 총알수로 나눈 나머지가 0이면 true 아니라면 false로 같게한다.
        isReload = (--currBullet % maxBullet == 0);//0에서 숫자를 나눠야 나머지가 0이된다.
        //해당 조건이 충족하면 코루틴을 실행하고 장전 애니메이션을 시작한다.
        if (isReload)
        {
            StartCoroutine(Reloading());
        }
    }
    //EnemyAI1에서 실행하며 총사격을 멈추게한다.
    public virtual void Stop()
    {
        death = true;
    }
    public virtual void ShotProcessOnServer()
    {
        enemyAudio.PlayOneShot(fireSfx, 0.3f);
        // 레이캐스트에 의한 충돌 정보를 저장하는 컨테이너
        RaycastHit hit;
        // 총알이 맞은 곳을 저장할 변수
        Vector3 hitPosition = Vector3.zero;

        // 레이캐스트(시작지점, 방향, 충돌 정보 컨테이너, 사정거리)
        if (Physics.Raycast(firePos.position,
            firePos.forward, out hit, fireDistance))
        {
            // 레이가 어떤 물체와 충돌한 경우

            // 충돌한 상대방으로부터 IDamageable 오브젝트를 가져오기 시도
            IDamageable target =
                hit.collider.GetComponent<IDamageable>();

            // 상대방으로 부터 IDamageable 오브젝트를 가져오는데 성공했다면
            if (target != null)
            {
                // 상대방의 OnDamage 함수를 실행시켜서 상대방에게 데미지 주기
                target.OnDamage(damage, hit.point, hit.normal);
            }

            // 레이가 충돌한 위치 저장
            hitPosition = hit.point;
        }
        else
        {
            // 레이가 다른 물체와 충돌하지 않았다면
            // 총알이 최대 사정거리까지 날아갔을때의 위치를 충돌 위치로 사용
            hitPosition = firePos.position +
                          firePos.forward * fireDistance;
        }

        ShotEffectProcessOnClients(hitPosition);

    }
    // 이펙트 재생 코루틴을 랩핑하는 메서드
    public virtual void ShotEffectProcessOnClients(Vector3 hitPosition)
    {
        // 총알 라인 메소드 재생
        StartCoroutine(ShotEffect(hitPosition));//NullReference 오류
    }
    // 발사 이펙트와 소리를 재생하고 총알 궤적을 그린다
    protected virtual IEnumerator ShotEffect(Vector3 hitPosition)
    {


        StartCoroutine(ShowMuzzleFlash());//Fire 함수에서 코루틴 함수를 실행해줍니다.
        // 선의 시작점은 총구의 위치
        bulletLineRenderer.SetPosition(0, firePos.position);//nullReference 오류해결
        bulletLineRenderer.SetPosition(1, hitPosition);
        // 라인 렌더러를 활성화하여 총알 궤적을 그린다
        bulletLineRenderer.enabled = true;

        // 0.03초 동안 잠시 처리를 대기
        yield return new WaitForSeconds(0.03f);

        // 라인 렌더러를 비활성화하여 총알 궤적을 지운다
        bulletLineRenderer.enabled = false;
    }
    //코루틴으로 사용할 ShowMuzzleFlash 함수를 선언한다.
    //총알 발사섬광을 표현한다.
    public virtual IEnumerator ShowMuzzleFlash()
    {
        muzzleFlash.enabled = true;//화염이 나타나야 하므로 true로한다.
        //Euler함수를 이용해서 z축으로 랜덤하게 회전하게 한다.
        Quaternion rot = Quaternion.Euler(Vector3.forward * Random.Range(0, 360));
        //회전값을 muzzleFlash 의 localRotation 에 넣어준다.
        muzzleFlash.transform.localRotation = rot;
        //크기 스케일도 렌덤하게 지정해서 1~2 배 사이로 하여 localScale에 넣는다.
        muzzleFlash.transform.localScale = Vector3.one * Random.Range(1.0f, 2.0f);
        //랜덤한 좌표값을 muzzleFlash의 material offset 에 넣어준다.
        Vector2 offset = new Vector2(Random.Range(0, 2), Random.Range(0, 2)) * 0.5f;//0,0 부터 0.5,0.5까지 랜덤 좌표값 생성
        muzzleFlash.material.SetTextureOffset("_MainTex", offset);//_MainTex는 미리 지정되어 있는 Property Name으로 Diffuse를 나타낸다.

        //0.05초 부터 0.2초 까지 랜덤하게 코루틴 함수를 호출한다. 즉 잠시동안 멈춰준다.
        yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
        muzzleFlash.enabled = false;

    }
    //제장전 IEnumerator형 메소드
    public virtual IEnumerator Reloading()
    {
        muzzleFlash.enabled = false;//장전하는 동안 총구 화염이 안보이게 한다.
        enemyAnimator.SetTrigger(hashReload);//Reload 트리거를 활성화
        enemyAudio.PlayOneShot(reloadSfx, 0.3f);//사운드를 재생한다.
        //ReloadTime 만큼 대기한다.
        yield return wsReload;
        //총알을 채워준다.
        currBullet = maxBullet;
        isReload = false;//false로 변경하며 재장전 끝
    }
}

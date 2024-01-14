using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;//키보드 마우스 터치 이벤트를 지원

public class VirtualJoyStick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]//에디터에서 접근할수 있음
    private RectTransform lever;//레버이미지의 트랜스폼을 가질 변수
    private RectTransform rectTransform;//조이스틱을 엑트 트랜스폼을 가지고 있을  f 트랜스폼 변수
    [SerializeField, Range(10,250)]
    private float leverRange;//레버의 이동 범위

    private Vector2 inputDirection;//케릭터 이동값을 위하여 사용
    private bool isInput;//래버를 만지고 있는지 위한 불 변수
    //플레이어 오브젝트 접근
    public GameObject myPlayer = null;
    //조작하기
    public PlayerMovement playerMovement;
    //총발사하기
    private float leverDistance;
    public Gun gun;
    //수류탄 발사하기
    public FireGrenade fireGrenade;

    public enum JoystickType//조이스틱 2개라서 2가지로 나누어 작동
    {
        Move,
        Rotate
    }
    public JoystickType joystickType;//enum형 변수 사용
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    //플레이어와 컨트롤러 연결
    private void FixedUpdate()
    {
        //플레이어 케릭터와 스크립트들을 연결한다.
        //1p일경우 자신의 클라이언트 일 경우
        if(GameObject.FindWithTag("Player").transform.GetChild(0).gameObject.tag =="Player1")
            myPlayer = GameObject.FindWithTag("Player");//나의 플레이어에 내플레이어 넣기
        //각 스크립트들 찾아 연결
        playerMovement = myPlayer.GetComponent<PlayerMovement>();
        gun = myPlayer.transform.Find("GunPivot").gameObject.transform.Find("M4A1").GetComponent<Gun>();
        fireGrenade = myPlayer.GetComponent<FireGrenade>();

    }
    private void Update()
    {
        if (isInput)//누르고 있는 경우
        {
            InputControlVector();//움직임 관련함수 실행
            if (leverDistance > 250 && joystickType == JoystickType.Rotate)//발사조건
            {
                //발사
                gun.Fire();
            }
        }
    }

    //오브젝트를 클랙해서 드래그 하는 도중에 들어오는 이벤트
    //하지만 클릭을 유지한 상태로 마우스를 멈추면 이벤트가 들어오지 않음
    //드래그를 시작함
    public void OnBeginDrag(PointerEventData eventData)
    {
        ControlJoystickLever(eventData);//데이타를받아 해당 함수 실행
        isInput = true;//입력시작
    }
    //드래그 중일때
    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log(eventData);
        ControlJoystickLever(eventData);
    }
    //드래그가 끝날때
    public void OnEndDrag(PointerEventData eventData)
    {
        lever.anchoredPosition = Vector2.zero;//위치 초기화
        leverDistance = 0;//레버거리 초기화
        isInput = false;//입력중이 아니기에 false
        switch (joystickType)
        {
            case JoystickType.Move:
                playerMovement.Move(Vector2.zero);// 움직임 요소 초기화
                break;
            case JoystickType.Rotate:
                break;
        }
    }

    //Drag 관련 함수에서 실행
    //OnBeginDrag와 OnDrag의 내용이 사실상 중복되서 함수로 통일하여 사용한다.
    private void ControlJoystickLever(PointerEventData eventData)
    {
        //레버가 있어야할 위치를 구한다.
        var inputPos = eventData.position - rectTransform.anchoredPosition;
        //래버의 위치를 바로 하지 않고 범위보다 클경우 계산하여 레버가 튀어 나오지 않게한다.
        var inputVetor = inputPos.magnitude < leverRange ? inputPos : inputPos.normalized * leverRange;
        lever.anchoredPosition = inputVetor;//레버의 실제 위치를 적용
        inputDirection = inputVetor / leverRange;//이동값이 너무 커지는 걸 방지하기위해 leverRange를 나눈다.
        //레버와 중심과의 거리구하기
        leverDistance = Vector2.Distance(transform.position, eventData.position);
        //Debug.Log(leverDistance);
    }

    //update문에서 실행
    //컨트롤 값들을 캐릭터에 적용시키기위하여 사용한다.
    private void InputControlVector()
    {
        switch(joystickType)
        {
            case JoystickType.Move:
                //캐릭터에게 ControlJoystickLever에서 얻어온 입력벡터를 전달한다.
                playerMovement.Move(inputDirection);//이동함수 실행
                break;
            case JoystickType.Rotate:
                playerMovement.LookAround(inputDirection);
                break;
        }
    }

    //event trigger로 실행
    //수류탄 던지는 함수
    public void FireGrenade()
    {
        fireGrenade.Fire();
    }
    //장전하는 함수
    public void Reload()
    {
        gun.Reload();
    }
}

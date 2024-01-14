
using UnityEngine;

// 플레이어 캐릭터를 사용자 입력에 따라 움직이는 스크립트
public class PlayerMovement : MonoBehaviour {
    public float moveSpeed = 5f; // 앞뒤 움직임의 속도
    private Vector3 velocity;//pc 버전을위한 이동값
    private Vector2 moveInput;//모바일 버전을위한 이동값

    private Animator playerAnimator; // 플레이어 캐릭터의 애니메이터
    private PlayerInput playerInput; // 플레이어 입력을 알려주는 컴포넌트
    private Rigidbody playerRigidbody; // 플레이어 캐릭터의 리지드바디

    private void Start() {
        // 사용할 컴포넌트들의 참조를 가져오기
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
    }

    // FixedUpdate는 물리 갱신 주기에 맞춰 실행됨
    private void FixedUpdate() {
#if UNITY_STANDALONE
        // 입력값에 따라 애니메이터의 Move 파라미터 값을 변경
        playerAnimator.SetFloat("Move", playerInput.move);
        if(playerInput.move ==0)
            playerAnimator.SetFloat("Move", playerInput.moveSide);
        //MovePosition(현재위치 +이동할 속력 * 단위시간)
        playerRigidbody.MovePosition(playerRigidbody.position + velocity * Time.deltaTime);
#endif
#if UNITY_ANDROID
        // 이동 방향키 입력 판정 : 이동 방향 벡터가 0보다 크면 입력이 발생하고 있는 중
        bool isMove = moveInput.magnitude != 0;
        // 입력이 발생하는 중이라면 이동 애니메이션 재생
        playerAnimator.SetBool("isMove", isMove);
        if (isMove)
        {/*
            //캐릭터가 바라보는 방향 대로 이동하는 코드
            Vector3 lookForward = new Vector3(transform.forward.x, 0, transform.forward.z);
            //캐릭터의 오른쪽 방향
            Vector3 lookRight = new Vector3(transform.right.x, 0f, transform.right.z);
            // 이동 방향
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;
            // 이동
            transform.position += moveDir * Time.deltaTime * moveSpeed;
        */
            //상하좌우로 이동하는 코드
            Vector3 moveDir = new Vector3(moveInput.x, 0, moveInput.y);
            Vector3 moveNormal = moveDir.normalized * moveSpeed;
            playerRigidbody.MovePosition(playerRigidbody.position + moveNormal * Time.deltaTime);
        }
#endif
    }
    //Move 동작은 스크립트 비활성화로 멈추기 위하여 둘다 FixedUpdate 함수에서 실행한다.
    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }
    //보는위치값 가져오기 플레이어 회전시키기
    public void LookAt(Vector3 lookPoint)
    {
        Vector3 heightCorrectedPoint =
            new Vector3(lookPoint.x, transform.position.y, lookPoint.z);
        //마우스 커서를 바라봄
        transform.LookAt(heightCorrectedPoint);
    
    }

    /*모바일 버전*/
    //모바일 버전 이동방식
    public void Move(Vector2 inputDirection)
    {
        // 조이스틱의 값을 가져온다.
        moveInput = inputDirection;
    }
    //모바일 버전 플레이어회전
    public void LookAround(Vector2 inputDirection)
    {
        // 조이스틱의 값을 가져온다.
        Vector2 moveInput = inputDirection;
        //캐릭터가 바라보는 방향 값 결정
        Vector3 lookForward = new Vector3(moveInput.x, 0, moveInput.y);
        transform.forward = lookForward;
    }
}
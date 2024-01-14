using UnityEngine;

// 플레이어 캐릭터를 조작하기 위한 사용자 입력을 감지
// 감지된 입력값을 다른 컴포넌트들이 사용할 수 있도록 제공
//플레이어 이동에서 방향키로 이동하고 마우스로 방향을 정한다.
public class PlayerInput : MonoBehaviour {
    Camera viewCamera;//카메라
    public PlayerMovement playerMovement;
    public float moveSpeed = 5f;

    // 값 할당은 내부에서만 가능
    public float move { get; private set; } // 감지된 움직임 입력값
    public float moveSide { get; private set; } // 감지된 회전 입력값
    public bool fire { get; private set; } // 감지된 발사 입력값
    public bool reload { get; private set; } // 감지된 재장전 입력값
    public bool grenade { get; private set; }//감지된 수류탄 투척 입력값

    private void Start()
    {
        viewCamera = Camera.main;//캄메라를 카메라 변수에 연결
    }
 

    // 매프레임 사용자 입력을 감지
    private void Update() {
        // 게임오버 상태에서는 사용자 입력을 감지하지 않는다
        if (GameManager.instance != null
            && GameManager.instance.isGameover)
        {
            moveSpeed = 0;
            move = 0;
            moveSide = 0;
            fire = false;
            reload = false;
            grenade = false;
            return;
        }
#if UNITY_STANDALONE


        //화면상에서 마우스의 위치를반환
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            Debug.DrawLine(ray.origin, point, Color.red);
            playerMovement.LookAt(point);
        }
        
        //수평과 수직 방향에 대한 입력을 받음
        Vector3 moveInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        //normalized: 백터의 방햫값을 가져옴
        Vector3 moveVelocity = moveInput.normalized * moveSpeed;
        playerMovement.Move(moveVelocity);
        //move 와 moveSide 애니메이션 감지를 위해 사용한다.
        // move에 관한 입력 감지
        move = Input.GetAxis("Vertical");
        // rotate에 관한 입력 감지
        moveSide = Input.GetAxis("Horizontal");
        // fire에 관한 입력 감지
        fire = Input.GetButton("Fire1");
        // reload에 관한 입력 감지
        reload = Input.GetButtonDown("Reload");
        //grenade에 관한 입력 감지
        grenade = Input.GetButtonDown("Grenade");
#endif
    }
}
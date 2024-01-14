using Cinemachine; // 시네머신 관련 코드
using UnityEngine;

// 시네머신 카메라가 로컬 플레이어를 추적하도록 설정
public class CameraSetup : MonoBehaviour {
    void Start() {
       
            // 씬에 있는 시네머신 가상 카메라를 찾고
            CinemachineVirtualCamera followCam =
                FindObjectOfType<CinemachineVirtualCamera>();
            // 가상 카메라의 추적 대상을 자신의 트랜스폼으로 변경
            followCam.Follow = transform;
            followCam.LookAt = transform;
        
    }
}
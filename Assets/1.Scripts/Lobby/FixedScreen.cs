using System;
using UnityEngine;

public class FixedScreen : MonoBehaviour
{
    int setWidth;
    int setHeight;
    private void Start()
    {
        SetResolution();
    }
    //해상도 적용 함수 
    //전역변수 형태로 하여 어떤
    //함수를 사용함에따라 해상도가 달라지게한다.
    private void SetResolution()
    {
        setWidth = 1920;
        setHeight = 1080;
        Screen.SetResolution(setWidth, setHeight, true);
    }
}

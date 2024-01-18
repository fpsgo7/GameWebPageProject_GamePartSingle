using UnityEngine;
using TMPro;

public class TimerSimple : MonoBehaviour
{
    private float time = 3600;
    [SerializeField]
    private TextMeshProUGUI timeWatchTMP;
    char[] charArray = new char[7];

    void Update()
    {
        if (time > 0.1)
        {
            time -= Time.deltaTime;
            int userTime = (int)(time * 100);
            charArray[4] = '.';
            for (int i = charArray.Length - 1; i >= 5; i--)
            {
                // 배열의 마지막 부터 값이 들어간다.
                charArray[i] = (char)((userTime % 10) + 48);// 아스키 코드로 숫자를 찾기위해 + 48
                userTime /= 10;//10을 나눔으로써 가장 낮은 자리수 가 사라진다.
            }
            for (int i = charArray.Length-4 ; i >= 0; i--)
            {
                // 배열의 마지막 부터 값이 들어간다.
                charArray[i] = (char)((userTime % 10) + 48);// 아스키 코드로 숫자를 찾기위해 + 48
                userTime /= 10;//10을 나눔으로써 가장 낮은 자리수 가 사라진다.
            }
            timeWatchTMP.SetCharArray(charArray, 0, charArray.Length);
        }
    }

    public int GetTime()
    {
        float answerTime = time * 100;
        return (int)answerTime;
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterRankPanel : MonoBehaviour
{
    public Text rankText;
    public Text emailText;
    public Text characterNicknameText;
    public Text highScoreText;

    public void Init(string rankText, string emailText,
        string characterNicknameText, string highScoreText)
    {
        this.rankText.text = rankText;
        this.emailText.text = emailText;
        this.characterNicknameText.text = characterNicknameText;
        this.highScoreText.text = highScoreText;
    }
}

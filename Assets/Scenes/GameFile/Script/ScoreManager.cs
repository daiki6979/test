using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;//他のスクリプトからPopupManager.Instanceでアクセス
    public Text scoreText;

    int score = 0;//開始時のスコア

    void Awake()
    {
        Instance = this;
        scoreText.text = "Score : 0";//表示されるテキスト
    }

    public void AddScore(int value)
    {
        score += value;
        scoreText.text = "Score : " + score;//画面表示を更新
    }

}

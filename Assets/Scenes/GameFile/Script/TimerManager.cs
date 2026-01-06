using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{

    public float timeLimit = 180f;//ゲームの制限時間設定（秒単位）
    public Text timerText;

    public static bool isGameOver = false;//ゲーム終了のフラグ


    // Update is called once per frame
    void Update()
    {
        if (isGameOver) return;//ゲームが終了していたら、以下の処理は行わない

        //タイマー処理
        timeLimit -= Time.deltaTime;
        if (timeLimit <= 0)
        {
            timeLimit = 0;
            isGameOver = true;
        }

        int min = Mathf.FloorToInt(timeLimit / 60);
        int sec = Mathf.FloorToInt(timeLimit % 60);
        timerText.text = $"{min:00}:{sec:00}";
    }
}

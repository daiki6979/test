using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI scoreText;
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration = 1.0f;


    bool isShown = false;

    public void Show(GameEndType endType)
    {
        if (isShown) return;//二重呼び出し防止
        isShown = true;

        gameOverPanel.SetActive(true);

        //終了理由で表示を変える
        switch (endType)
        {
            case GameEndType.TimeUp:
                titleText.text = "TIME UP";
                break;
            case GameEndType.GameOver:
                titleText.text = "GAME OVER";
                break;
        }

        scoreText.text = "SCORE : " + ScoreManager.Instance.Score;
        Time.timeScale = 0f;
    }

    //リトライ
    public void Retry()
    {
        Time.timeScale = 1f;
        StartCoroutine(FadeOutAndLoad(
            SceneManager.GetActiveScene().name
        ));
    }


    //タイトル画面遷移
    public void BackToTitle()
    {
        Time.timeScale = 1f;
        StartCoroutine(FadeOutAndLoad("野菜引っこ抜き"));
    }


    //フェードアウト処理
    IEnumerator FadeOutAndLoad(string sceneName)
    {
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.unscaledDeltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(0f, 1f, time / fadeDuration);
            yield return null;
        }

        fadeCanvasGroup.alpha = 1f;
        Time.timeScale = 1f;
        SceneManager.LoadScene("TItleScene");
    }


}


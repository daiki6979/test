using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TItleManager : MonoBehaviour
{
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration = 1f;

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float time = 0f;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(1f, 0f, time / fadeDuration);
            yield return null;
        }
        fadeCanvasGroup.alpha = 0f;
    }

    public void StartGame()
    {
        StartCoroutine(FadeOutAndLoad("野菜引っこ抜き"));
    }

    IEnumerator FadeOutAndLoad(string sceneName)
    {
        float time = 0f;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(0f, 1f, time / fadeDuration);
            yield return null;
        }
        SceneManager.LoadScene("SampleScene");
    }
}

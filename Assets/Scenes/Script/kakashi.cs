using System.Collections;
using UnityEngine;

public class kakashi : MonoBehaviour
{
    public int point = 5;

    public float baseY = 0.5f;

    public float liftHeight = 1.2f;

    public float rotateSpeed = 360f;

    public float glowIntensity = 2f;   // 発光量
    public float glowTime = 1.2f;

    public float returnTime = 1.5f;    //戻る時間

    Vector3 basePos;
    Light cdLight;

    //かかしのフラグを立てる
    public static bool isCdActive = false;

    //発光させる
    void Awake()
    {
        basePos = new Vector3(transform.position.x, baseY, transform.position.z);
        transform.position = basePos;

        cdLight = gameObject.AddComponent<Light>();
        cdLight.type = LightType.Point;
        cdLight.range = 3f;
        cdLight.intensity = 0;
    }

    //上げて下げる
    public IEnumerator Activate()
    {
        if (isCdActive) yield break;

        isCdActive = true;

        Vector3 upPos = basePos + Vector3.up * liftHeight;

        float t = 0;

        // ===== 上昇＋回転＋発光 =====
        while (t < glowTime)
        {
            t += Time.deltaTime;
            float rate = t / glowTime;

            transform.position = Vector3.Lerp(basePos, upPos, rate);
            transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
            cdLight.intensity = Mathf.Lerp(0, glowIntensity, rate);

            yield return null;
        }

        // ===== ゆっくり戻る =====
        t = 0;
        Vector3 startPos = transform.position;

        while (t < returnTime)
        {
            t += Time.deltaTime;
            float rate = t / returnTime;

            transform.position = Vector3.Lerp(startPos, basePos, rate);
            cdLight.intensity = Mathf.Lerp(glowIntensity, 0, rate);

            yield return null;
        }

        transform.position = basePos;
        cdLight.intensity = 0;


        //フラグを下げる
        isCdActive = false;

        /*フラグの使い方
         * void Update()
{
            if (TimerManager.isGameOver) return;
            if (Cd.isCdActive) return; // ← 入力ロック
         */
    }
}

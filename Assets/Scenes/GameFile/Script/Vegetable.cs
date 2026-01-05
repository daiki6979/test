using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class Vegetable : MonoBehaviour
{
    public int point;//ポイント
    public float baseY;//スポーンY軸

    //引っこ抜きの処理
    public IEnumerator PullOut()
    {
        Vector3 start = transform.position;//現在位置を保存

        Vector3 up = start + Vector3.up * 1.5f;//引っこ抜き（上に移動）

        float t = 0;

        //引っこ抜く
        while (t < 1)
        {
            t += Time.deltaTime;//スピード
            transform.position = Vector3.Lerp(start, up, t);
            yield return null;
        }

        Vector3 right = up + Vector3.right * 10f;//画面外へ飛ばす（右に移動）

        t = 0;

        //画面外へ飛ばす
        while (t < 1)
        {
            t += Time.deltaTime * 2f;//スピード
            transform.position = Vector3.Lerp(up, right, t);
            yield return null;
        }

        Destroy(gameObject);
    }
}

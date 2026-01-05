using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor_Frame : MonoBehaviour
{
    Renderer[] rends;

    //元の色
    Color baseColor;

    //点滅
    Coroutine blinkCoroutine;


    // Start is called before the first frame update
    void Start()
    {
        rends = GetComponentsInChildren<Renderer>();
        baseColor = rends[0].material.color;
    }

    //抜けるか抜けないか
    public void SetCanPull(bool canPull)
    {
        if (canPull)
        {
            //点滅中
            if (blinkCoroutine == null)
            {
                blinkCoroutine = StartCoroutine(Blink());
            }
        }
        else
        {
            //点滅終了
            if (blinkCoroutine != null)
            {
                StopCoroutine(blinkCoroutine);
                blinkCoroutine = null;
            }
            SetColor(baseColor);
        }
    }

    //子オブジェクトすべてに
    void SetColor(Color c)
    {
        foreach (Renderer r in rends)
        {
            r.material.color = c;
        }
    }

    //点滅処理
    IEnumerator Blink()
    {
        while(true)
        {
            SetColor(Color.white);
            yield return new WaitForSeconds(0.2f);

            SetColor(baseColor);
            yield return new WaitForSeconds(0.2f);
        }
    }

}

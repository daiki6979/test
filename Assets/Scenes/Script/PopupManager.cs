using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{

    public static PopupManager Instance;//他のスクリプトからPopupManager.Instanceでアクセス
    public Text popupText;

    void Awake()
    {
        Instance = this;
        popupText.gameObject.SetActive(false);//ゲーム開始時は非表示
    }

    public void Show(string msg)
    {
        StartCoroutine(Popup(msg));//ポップアップ表示
    }

    IEnumerator Popup(string msg)
    {
        popupText.text = msg;
        popupText.gameObject.SetActive(true);//画面に表示

        yield return new WaitForSeconds(1f);//何秒表示させるか
        
        popupText.gameObject.SetActive(false);//非表示にする
    }

}

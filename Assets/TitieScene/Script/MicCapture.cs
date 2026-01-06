/*
using UnityEngine;
using CriWare;
using TMPro;

public class MicCapture : MonoBehaviour
{
    //マイク用変数。マイクの機能はここに格納されます。
    CriAtomExMic mic;

    //マイクの入力内容が入るFloatの配列。
    //入力内容は各配列内容毎に-1.0~1.0まで。
    float[] micdata = new float[512];

    //最大値
    public float peakLevel { get; private set; }
    //波形平均値
    public float RMSLevel { get; private set; }


    //最大音量を表示するText
    [SerializeField]
    TextMeshProUGUI peakText;
    //音声の平均音量を表示するためのText
    [SerializeField]
    TextMeshProUGUI RMSText;


    //実装例：角度を変更するオブジェクト
    [SerializeField]
    GameObject testObject;
    //実装例：オブジェクトを動かす最小音量。これより小さい値は０とみなす。
    [SerializeField]
    float threshold = 0.02f;

    void Start()
    {
        //マイクモジュール初期化
        CriAtomExMic.InitializeModule();

        //デフォルトマイクを取得する※マイクが接続されている必要があります！
        //「？」は変数名ではなく、Null許容型の演算子
        CriAtomExMic.DeviceInfo? defaultDevice = CriAtomExMic.GetDefaultDevice();
        if (!defaultDevice.HasValue)
        {
            Debug.Log("デバイスが見つかりませんでした。");
        }

        //デバッグログ：取得されているマイクデバイスの内容をすべて表示
        foreach (var device in CriAtomExMic.GetDevices())
        {
            //もし対象デバイスがDefaultデバイスと同じであれば…？
            if (device.deviceName == defaultDevice.Value.deviceName)
            {
                //…Debug.Logに(has set as default)を追加
                Debug.Log($"{device.deviceName} (has set as default)");
            }
            else
            {
                //…それ以外は普通にデバイス名を出す
                Debug.Log($"{device.deviceName}");
            }
        }

        //デフォルトデバイスが取れていたら
        if (defaultDevice.HasValue)
        {
            //マイク入力を開始
            StartMic();
        }

    }


    /// <summary>
    /// マイク取得を作成し、入力を開始します。
    /// </summary>
    public void StartMic()
    {
        //マイクの設定用Configを作成。
        CriAtomExMic.Config config = CriAtomExMic.Config.Default;
        //1フレームに取得するサンプリング数
        config.frameSize = (uint)micdata.Length;

        //マイク生成
        mic = CriAtomExMic.Create(config);
        if (mic != null)
        {
            mic.Start();
        }

    }

    /// <summary>
    /// マイクの処理を停止し、削除します。
    /// 実行しても、Start処理で行ったDefaultDeviceの取得はそのまま残っているため、
    /// 再開するためにはStartMic()を再度実行して下さい
    /// </summary>
    public void StopAndDestroyMic()
    {
        //マイクを削除
        if (mic != null)
        {
            //マイクを停止
            mic.Stop();
            //マイク自体の処理を削除
            mic.Dispose();
            //参照エラー回避のため、nullで上書き
            mic = null;
        }
    }


    // Update is called once per frame
    void Update()
    {
        //マイクが設定されているときだけ入力
        if (mic != null)
        {
            //マイクの入力取得
            //マイクの内容は、厳密には必ず毎フレーム取得されるわけではなく、指定のサンプリング数が取得できたタイミングでマイクの入力を返します。
            //そのため、擬似的に毎フレーム入力されているような挙動を作るためには「マイクの入力があったときのみ」入力を扱うようにします
            //マイクの入力があったかどうかは、mic.ReadData(micData)を入力した際の値で判別できます
            //マイク入力あり→1以上の値を返す（通常、ReadData()に入力した配列のLengthの数が入力されます）
            //マイク入力なし→0が入力を返す
            uint numSamples = mic.ReadData(micdata);

            //取得できたサンプル数が0以上の場合(=マイク入力があった場合）
            if (numSamples > 0)
            {

                //最大音量
                float maxPeak = 0f;
                //平均音量
                float sample_total = 0f;

                for (uint i = 0; i < numSamples; i++)
                {
                    //最大音量
                    if (maxPeak < Mathf.Abs(micdata[i]))
                    {
                        maxPeak = Mathf.Abs(micdata[i]);
                    }

                    //波形全体の平均音量=>まずすべての二乗数を取る
                    sample_total += micdata[i] * micdata[i];
                }

                //最大音量値
                peakLevel = maxPeak;
                //波形全体の平均音量=>for文内で入力したすべての値を取得サンプル数で割り、平方根を取得する
                RMSLevel = Mathf.Sqrt(sample_total / numSamples);

                //Textの内容を更新
                //Tips:ToStringに(N+桁数)を与えると、小数点の表示桁数を指定できます。 
                peakText.text = "PEAK : " + peakLevel.ToString("N2");
                RMSText.text = "RMS : " + RMSLevel.ToString("N2");

            }
            Debug.Log("Get Sample Num = " + numSamples + " : Frame Count （" + 1f / Time.deltaTime + "fps）　: " + Time.frameCount);
        }

        //==============================================活用例==============================================
        //マイクの入力音量（RMS）に合わせて、オブジェクトの回転角を変える。
        float deg;
        //非常に小さい音量が入力された際にガタガタするのを防ぐため、しきい値で正規化
        if (Mathf.Abs(RMSLevel) < threshold)
        {
            deg = 0;
        }
        else
        {
            deg = RMSLevel;
        }
        //オブジェクトの角度を変更
        testObject.transform.localEulerAngles = new Vector3(0, 0, deg * -120f);
    }
    //オブジェクトが消える際、またはゲーム終了時
    private void OnDestroy()
    {
        //マイクをオフにする
        StopAndDestroyMic();
    }
}

*/
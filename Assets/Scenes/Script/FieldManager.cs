using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Channels;
using UnityEngine;

public class FieldManager : MonoBehaviour
{
    //6×4マス
    public int width = 6;
    public int height = 4;

    //使う野菜たち
    public GameObject daikonPrefab;
    public GameObject ninjinPrefab;
    public GameObject kakashiPrefab;
    //選択カーソル
    public GameObject SelectCursol;

    //加速度センサZ軸の閾値(変化量)
    public float pullThresholdZ = -0.5f;

    float baseZ;//初期のZの値（基準値）
    bool isBaseSet = false;

    //連続で振りすぎてエラーが起こるのを防ぐ
    bool canPullByAcc = true;

    GameObject[,] field;//畑の状態の保持

    int x = 0, z = 0;

    // Start is called before the first frame update
    void Start()
    {
        field = new GameObject[width, height];
        AddHeight();
        //最初はすべて大根にする
        for (int x = 0; x < width; x++)
        {
            for (int z = 1; z < height; z++)
            {
                SpawnDaikon(x, z);
            }
        }
        
        kakashiSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (TimerManager.isGameOver) return;//ゲーム終了後は受け付けない

        // WASD移動
        if (Input.GetKeyDown(KeyCode.A)) x = Mathf.Max(0, x - 1);
        if (Input.GetKeyDown(KeyCode.D)) x = Mathf.Min(width - 1, x + 1);
        if (Input.GetKeyDown(KeyCode.W)) z = Mathf.Min(height - 1, z + 1);
        if (Input.GetKeyDown(KeyCode.S)) z = Mathf.Max(0, z - 1);

        //選択範囲を四角で表示
        Cursol(x, z);

        CheckPullByZValue();

        // Gキーで引っこ抜く
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (field[x, z] != null)
            {
                // かかしか？
                kakashi kakashi = field[x, z].GetComponent<kakashi>();
                if (kakashi != null)
                {
                    StartCoroutine(kakashi.Activate());
                    return;
                }

                Vegetable v = field[x, z].GetComponent<Vegetable>();//Vegetableスクリプトを取得
                if (v != null)
                {
                    //引っこ抜きのアニメーション開始（上に移動させ、画面外へ飛ばす）
                    StartCoroutine(v.PullOut());

                    ScoreManager.Instance.AddScore(v.point);//スコアを加算

                    PopupManager.Instance.Show("+" + v.point + " Point");//画面にポップアップ表示

                    field[x, z] = null;//抜いた個所の野菜の情報を一度消去

                    StartCoroutine(Respawn(x, z));//野菜をスポーンさせる
                }
            }
        }
    }

    void CheckPullByZValue()
    {
        if(Recelver.Instance==null) return;

        float zAcc = Recelver.Instance.acc.z;

        //初期のZの値を取得（基準値）
        if (!isBaseSet)
        {
            baseZ = zAcc;
            isBaseSet = true;
            return;
        }

        float deltaZ = zAcc - baseZ;

        //閾値を超えたら
        if ((deltaZ < pullThresholdZ) && canPullByAcc)
        {
            PullCurrent();
            canPullByAcc = false;
        }

        //加速度の閾値が戻る
        if(zAcc > 1.0f)
        {
            canPullByAcc=true;
        }

    }

    //引っこ抜き処理
    void PullCurrent()
    {
        Debug.Log("Pulled!");
        if (field[x, z] != null)
        {
            Vegetable v = field[x, z].GetComponent<Vegetable>();//Vegetableスクリプトを取得

            //引っこ抜きのアニメーション開始（上に移動させ、画面外へ飛ばす）
            StartCoroutine(v.PullOut());

            ScoreManager.Instance.AddScore(v.point);//スコアを加算

            PopupManager.Instance.Show("+" + v.point + " Point");//画面にポップアップ表示

            field[x, z] = null;//抜いた個所の野菜の情報を一度消去

            StartCoroutine(Respawn(x, z));//野菜をスポーンさせる
        }
    }

    //大根のスポーン（初期が大根のため作成）
    void SpawnDaikon(int x, int z)
    {
        Vector3 pos = new Vector3(x * 2, -1.8f, z * 2);//大根の配置（Y軸は固定）
        GameObject obj = Instantiate(daikonPrefab, pos, Quaternion.identity);
        Vegetable v = obj.GetComponent<Vegetable>();
        v.point = 1;//大根のポイント数
        v.baseY = -1.8f;
        field[x, z] = obj;
    }

    //ランダムスポーン
    void SpawnRandom(int x, int z)
    {
        bool ninjin = UnityEngine.Random.value > 0.5f;

        if (ninjin)
        {
            Vector3 pos = new Vector3(x * 2, -3.8f, z * 2);//ニンジンの配置（Y軸は固定）
            GameObject obj = Instantiate(ninjinPrefab, pos, Quaternion.identity);
            Vegetable v = obj.GetComponent<Vegetable>();
            v.point = 2;//ニンジンのポイント数
            v.baseY = -3.9f;
            field[x, z] = obj;
        }
        else
        {
            SpawnDaikon(x, z);
        }

 

        /*
        もし野菜を追加するならのプログラム例
        // 0.0 ～ 1.0 の乱数を取得
        float r = UnityEngine.Random.value;

        // 50% 
        if (r < 0.5f)
        {
            SpawnDaikon(x, z);
        }
        // 次の30% 
        else if (r < 0.8f)
        {
            Vector3 pos = new Vector3(x * 2, -3.8f, z * 2);
            GameObject obj = Instantiate(ninjinPrefab, pos, Quaternion.identity);

            Vegetable v = obj.GetComponent<Vegetable>();
            v.point = 2;
            v.baseY = -3.8f;

            field[x, z] = obj;
        }
        // 残り20% 
        else
        {
            Vector3 pos = new Vector3(x * 2, -2.5f, z * 2);
            GameObject obj = Instantiate(tomatoPrefab, pos, Quaternion.identity);

            Vegetable v = obj.GetComponent<Vegetable>();
            v.point = 3;
            v.baseY = -2.5f;

            field[x, z] = obj;
        }*/
    }
    //選択カーソルの表示
    void Cursol(int x, int z)
    {
        Vector3 pos = new Vector3(x * 2,0.1f, z * 2);

        SelectCursol.transform.position = pos;
    }
    //リスポーン
    IEnumerator Respawn(int x, int z)
    {
        float t = UnityEngine.Random.Range(1f, 5f);//何秒後にリスポーンするか指定
        yield return new WaitForSeconds(t);
        SpawnRandom(x, z);//ランダムでスポーンさせる
    }

    //畑の拡張(縦)
    void AddHeight()
    {
        int newHeight = height + 1;
        GameObject[,] newField = new GameObject[width, newHeight];

        // 既存データをコピー
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                newField[x, z] = field[x, z];
            }
        }

        field = newField;
        height = newHeight;
    }
    //かかしのスポーン
    void kakashiSpawn()
    {
        Vector3 pos = new Vector3(x * 2, 0f, z * 2);
        GameObject obj = Instantiate(kakashiPrefab, pos, Quaternion.identity);
        kakashi kakashi = obj.GetComponent<kakashi>();
        kakashi.point = 5;
        field[0, 0] = obj;
    }

}

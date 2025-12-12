using UnityEngine;

public class MoleMovement : MonoBehaviour
{
    public float moveRange = 5f; // モグラが移動する範囲
    public float moveSpeed = 2f; // モグラの移動速度
    public float hideTime = 1f; // モグラが隠れる時間
    public float showTime = 1f; // モグラが出ている時間

    private Vector3 startPos;
    private bool isMovingUp = false;
    private float timer = 0f;

    void Start()
    {
        startPos = transform.position; // モグラの初期位置を保存
        MoveToRandomPosition(); // 最初の移動を実行
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (isMovingUp) // モグラが出てくるとき
        {
            if (timer >= showTime)
            {
                isMovingUp = false;
                timer = 0f;
            }
        }
        else // モグラが隠れるとき
        {
            if (timer >= hideTime)
            {
                MoveToRandomPosition();
                isMovingUp = true;
                timer = 0f;
            }
        }

        // 上下の動き
        float newY = isMovingUp ? startPos.y + 1f : startPos.y;
        transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, newY, Time.deltaTime * moveSpeed), transform.position.z);
    }

    void MoveToRandomPosition()
    {
        float randomX = Random.Range(-moveRange, moveRange);
        float randomZ = Random.Range(-moveRange, moveRange);
        Vector3 newPosition = new Vector3(randomX, startPos.y, randomZ);
        transform.position = newPosition;
    }
}
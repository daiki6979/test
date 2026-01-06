using UnityEngine;

public class NeckController : MonoBehaviour
{
    public MicCapture micCapture;   // MicCapture の参照
    public float maxAngle = 30f;    // 最大回転角度
    public float smoothSpeed = 5f;  // 補間速度

    void Update()
    {
        if (micCapture == null) return;

        // rms を 0-1 に正規化（感度調整用）
        float normalized = Mathf.Clamp01(micCapture.rms * 20f);

        // 目標角度
        float targetAngle = normalized * maxAngle;

        // 現在の角度を補間
        Vector3 euler = transform.localEulerAngles;
        float newX = Mathf.LerpAngle(euler.x, targetAngle, Time.deltaTime * smoothSpeed);

        transform.localEulerAngles = new Vector3(newX, euler.y, euler.z);
    }
}

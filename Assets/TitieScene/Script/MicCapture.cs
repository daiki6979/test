using UnityEngine;

public class MicCapture : MonoBehaviour
{
    AudioClip micClip;
    public int sampleLength = 1024;
    public float rms; // 音量（RMS値）

    void Start()
    {
        if (Microphone.devices.Length == 0)
        {
            Debug.LogError("マイクが見つかりません");
            return;
        }

        // デフォルトマイクを1秒長でループ録音
        micClip = Microphone.Start(null, true, 1, 44100);
    }

    void Update()
    {
        if (micClip == null) return;

        float[] samples = new float[sampleLength];
        int micPos = Microphone.GetPosition(null) - sampleLength;
        if (micPos < 0) return;

        micClip.GetData(samples, micPos);

        // RMS計算
        float sum = 0f;
        for (int i = 0; i < samples.Length; i++)
        {
            sum += samples[i] * samples[i];
        }
        rms = Mathf.Sqrt(sum / samples.Length);
    }
}

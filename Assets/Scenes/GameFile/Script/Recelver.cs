using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class Recelver : MonoBehaviour
{

    SerialPort serial;

    public float absAcc;
    public Vector3 acc;

    public static Recelver Instance;

    // Start is called before the first frame update
    void Start()
    {
        serial = new SerialPort("COM3", 115200); // ← COM番号注意
        serial.Open();
        serial.ReadTimeout = 50;
    }

    //追加部分
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (serial != null && serial.IsOpen)
        {
            try
            {
                string line = serial.ReadLine();
                string[] data = line.Split(',');

                if (data.Length == 4)
                {
                    absAcc = float.Parse(data[0]);
                    acc.x = float.Parse(data[1]);
                    acc.y = float.Parse(data[2]);
                    acc.z = float.Parse(data[3]);
                }
            }
            catch { }
        }
    }

    void OnDestroy()
    {
        if (serial != null && serial.IsOpen)
        {
            serial.Close();
        }
    }
}

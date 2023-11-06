using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Linq;
using UnityEngine.XR.ARFoundation;

//This program is Andriod Side 

//送信側
public class ClientManager : SingletonMonoBehaviour<ClientManager>
{
    /*ブロードキャストアドレス
    //133.42.155.255
    IPv4 アドレス . . . . . . . . . . . .: 133.42.155.246
    サブネット マスク . . . . . . . . . .: 255.255.255.0
    デフォルト ゲートウェイ . . . . . . .: 133.42.155.254
     */

    //デスクトップWIFI 192.168.0.209
    //ノートPC 133.42.155.160
    //ノートWIFI 192.168.0.85
    //スマホ 192.168.0.164

    //A804
    //デスクトップWIFI 192.168.0.210
    //スマホ 192.168.0.157


    //送信Bit
    // int - 0
    // float - 1
    // doublu - 2
    // string -3
    // char - 4
    //vector -5
    //rightVectorPos -6
    //rightVectorRot -7
    //leftVectorPos -8
    //leftVectorRot -9
    //positionAdjust -10
    //objectHighLight -11
    //objectSelect -12
    //ARrightHand -13
    //drawLine -14
    //drawOnOff -15

    private string host = "192.168.0.210";
    private int port = 9000;
    private UdpClient client;

    [SerializeField] Text text;

    [SerializeField] GameObject roomOb;
  
    [SerializeField] Text comformText;
    [SerializeField] Text moveFlagText;
    int castFlag = 0;
    ObjectManager objectManager;
    DrawManager drawManager;

    // Start is called before the first frame update
    void Start()
    {
        client = new UdpClient();
        client.Connect(host, port);
        objectManager = ObjectManager.Instance;
        drawManager = DrawManager.Instance;
        text.text = host;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {                
                text.text = "押した";
                drawManager.DrawFlagSwitch(true);
                byte[] vecByte = new byte[] { 15, (byte)1 };
                Send(vecByte);
            }

            if (touch.phase == TouchPhase.Ended)
            {
                text.text = "離しyon\nto "+host;

                //MoveSwitch();
                drawManager.DrawFlagSwitch(false);
                byte[] vecByte = new byte[] { 15, (byte)0 };
            }

            //if (touch.phase == TouchPhase.Moved)
            //{                
            //    text.text = "押しっぱ";
            //}
        }             
    }

    void MoveSwitch()
    {
        Debug.Log("Send Touch --12");
        if (castFlag == 0)
        {
            castFlag = 1;
            moveFlagText.text = "MoveFlag ON";
        }
        else
        {
            castFlag = 0;
            moveFlagText.text = "MoveFlag OFF";
            objectManager.EmphasisReset();
        }

        byte[] vecByte = new byte[] { 12, (byte)castFlag };
        Send(vecByte);
    }

    void DrawOn()
    {

    }

    void DrawOff()
    {

    }

    //送信Bit
    // int - 0
    // float - 1
    // doublu - 2
    // string -3
    // char - 4
    //vector -5
    //rightVectorPos -6
    //rightVectorRot -7
    //leftVectorPos -8
    //leftVectorRot -9
    //positionAdjust -10
    //objectHighLight -11
    //objectSelect -12
    //ARrightHand -13
    //drawLine -14

    private void Send(byte[] bytes)
    {
        client.Send(bytes, bytes.Length);
    }

    public void SendString(string message)
    {
        var byteMessage = Encoding.UTF8.GetBytes(message);
        byteMessage = new byte[] { 3 }.Concat(byteMessage).ToArray();
        Send(byteMessage);
    }

    public void SendVector(Vector3 vec)
    {        
        float x = vec.x;
        float y = vec.y;
        float z = vec.z;

        byte[] xByte = BitConverter.GetBytes(x);
        byte[] yByte = BitConverter.GetBytes(y);
        byte[] zByte = BitConverter.GetBytes(z);
        
        byte[] vecByte = xByte.Concat(yByte.Concat(zByte).ToArray()).ToArray();
        vecByte = vecByte.Concat(new byte[] { 5 }).ToArray();

        vecByte = new byte[] { 5 }.Concat(vecByte).ToArray();
        Send(vecByte);
    }


    private void OnDestroy()
    {
        client.Close();
    }
}

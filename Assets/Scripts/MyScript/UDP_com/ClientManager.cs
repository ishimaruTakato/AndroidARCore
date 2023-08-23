using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Linq;

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

    //デスクトップPC 192.168.0.209
    //ノートPC 133.42.155.160
    //ノートWIFI 192.168.0.85
    //スマホ 192.168.0.164

    //送信Bit
    // int - 0
    // float - 1
    // doublu - 2
    // string -3
    // char - 4
    //vector -5

    private string host = "192.168.0.209";
    private int port = 9000;
    private UdpClient client;

    [SerializeField] Text text;

    // Start is called before the first frame update
    void Start()
    {
        client = new UdpClient();
        client.Connect(host, port);

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
                Debug.Log("押した瞬間");
                text.text = "押した";


            }

            if (touch.phase == TouchPhase.Ended)
            {
                Debug.Log("離した瞬間");
                text.text = "離しyon\nto "+host;

                Debug.Log("Send");
                SendString("Hello Touch World!");       


            }

            if (touch.phase == TouchPhase.Moved)
            {
                Debug.Log("押しっぱなし");
                text.text = "押しっぱ";
            }
        }             
    }

    //送信Bit
    // int - 0
    // float - 1
    // doublu - 2
    // string -3
    // char - 4
    //vector -5

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

        Debug.Log("Client--- x=" + x + ", y=" + y + ",z=" + z);

        byte[] xByte = BitConverter.GetBytes(x);
        byte[] yByte = BitConverter.GetBytes(y);
        byte[] zByte = BitConverter.GetBytes(z);

        string test = "";
        foreach (var i in xByte)
        {
            test += i + ", ";
        }

        Debug.Log(test);

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

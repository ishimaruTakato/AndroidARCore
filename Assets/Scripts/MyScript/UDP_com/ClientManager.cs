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
    //drawSwitch -15
    //drawUndo -16
    //drawAllDelete -17

    private string host = "192.168.0.210";
    private int port = 9000;
    private UdpClient client;

    [SerializeField] Text text;

    [SerializeField] GameObject roomOb;
  
    [SerializeField] Text comformText;
    [SerializeField] Text moveFlagText;
    [SerializeField] Image moveFlagPanel;
    int castFlag = 0;
    ObjectManager objectManager;
    DrawManager drawManager;
    [SerializeField] bool emphasisFlag;
    [SerializeField] bool drawFlag;

    [SerializeField] Canvas canvas;
    [SerializeField] RectTransform AllDeleteButton;
    [SerializeField] RectTransform UndoButton;

    // Start is called before the first frame update
    void Start()
    {
        client = new UdpClient();
        client.Connect(host, port);
        objectManager = ObjectManager.Instance;
        drawManager = DrawManager.Instance;
        text.text = host;

        if (emphasisFlag) objectManager.ARHandOn();
    }

    // Update is called once per frame
    void Update()
    {
        if (!canvas.enabled) return;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                //押した
                text.text = "押した";
                bool tmpFlag = true;

                if (drawFlag)
                {
                    if (RectTransformUtility.RectangleContainsScreenPoint(UndoButton, Input.mousePosition))
                    {
                        tmpFlag = false;
                        drawManager.LineUndo();

                        byte[] vecByte;
                        vecByte = new byte[] { 16, (byte)0 };
                        Send(vecByte);
                    }

                    if (RectTransformUtility.RectangleContainsScreenPoint(AllDeleteButton, Input.mousePosition))
                    {
                        tmpFlag = false;
                        drawManager.LineAllDelete();

                        byte[] vecByte;
                        vecByte = new byte[] { 17, (byte)0 };
                        Send(vecByte);
                    }

                    DrawSwitch(tmpFlag);
                }

                if(emphasisFlag)
                {

                }
            }

            if (touch.phase == TouchPhase.Ended)
            {
                //離した
                text.text = "離しyon\nto "+host;
                if (drawFlag) DrawSwitch(false);
                if (emphasisFlag) MoveSwitch();
                
            }

            if (touch.phase == TouchPhase.Stationary)
            {
                //押しっぱ
                
            }

                if (touch.phase == TouchPhase.Moved)
            {
                //押して動かす
                text.text = "押しっぱ";
            }
        }             
    }

    void DrawSwitch(bool flag)
    {
        drawManager.DrawFlagSwitch(flag);
        byte[] vecByte;

        if (flag) vecByte = new byte[] { 15, (byte)1 };
        else vecByte = new byte[] { 15, (byte)0 };

        Send(vecByte);
    }

    void MoveSwitch()
    {
        Debug.Log("Send Touch --12");
        if (castFlag == 0)
        {
            castFlag = 1;
            moveFlagText.text = "強調 ON";
            moveFlagPanel.color = new Color32(0, 156, 24, 255);
        }
        else
        {
            castFlag = 0;
            moveFlagText.text = "強調 OFF";
            moveFlagPanel.color = new Color32(166,0,37,255);
            objectManager.EmphasisReset();
        }

        byte[] vecByte = new byte[] { 12, (byte)castFlag };
        Send(vecByte);

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
    //drawSwitch -15
    //drawUndo -16
    //drawAllDelete -17

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

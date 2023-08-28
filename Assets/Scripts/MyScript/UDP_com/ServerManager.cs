using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine.UI;

//This program is Andriod Side

//受け取った側の記載
public class ServerManager : SingletonMonoBehaviour<ServerManager> 
{
    HandByte handByte;


    private UdpClient udpClient;
    private Subject<string> subject = new Subject<string>();
    [SerializeField] Text message;   

    //[SerializeField] GameObject cube;
    private Vector3 cubePosition;


    // Start is called before the first frame update
    void Start()
    {
        handByte = HandByte.Instance;

        udpClient = new UdpClient(9000);
        udpClient.BeginReceive(OnReceived, udpClient);

        subject
            .ObserveOnMainThread()
            .Subscribe(msg =>
            {
                //ここやったらunityの機能可能
                var command = msg.Split(':');

                switch (command[0])
                {
                    case "M":
                        message.text = msg;
                        break;
                    case "V":
                        message.text = "Position Change";
                        //cube.transform.position = cubePosition;
                        break;

                    case "Pos":
                        Debug.Log("Receive Pos3");
                        handByte.TestReceivePos(IndexTipPos);
                        message.text = "Position Get";
                        break;

                    case "Rot":
                        Debug.Log("Receive Rot3");
                        message.text = "Rotation Get";
                        break;
                }



            }).AddTo(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnReceived(System.IAsyncResult result)
    {
        UdpClient getUdp = (UdpClient)result.AsyncState;
        IPEndPoint ipEnd = null;

        byte[] getByte = getUdp.EndReceive(result, ref ipEnd);
        byte byteType = getByte[0];
        getByte = getByte.Skip(1).ToArray();

        //送信Bit
        // int - 0
        // float - 1
        // doublu - 2
        // string -3
        // char - 4
        //vector -5
        //vectorPos -6
        //vectorRot -7

        switch (byteType)
        {
            case 3:
                var message = Encoding.UTF8.GetString(getByte);
                Debug.Log("mes");
                subject.OnNext("M:" + message);

                break;

            case 5:
                
                ReceiveVec(getByte);             
                subject.OnNext("V");
                break;

            case 6:
                Debug.Log("Receive Pos");
                this.ReceivePos(getByte);
                subject.OnNext("Pos");
                break;

            case 7:
                Debug.Log("Receive Rot");
                handByte.ReceiveRot(getByte);
                subject.OnNext("Rot");
                break;
        }

        getUdp.BeginReceive(OnReceived, getUdp);
    }

    //case5
    private void ReceiveVec(byte[] bytes)
    {
        cubePosition.x = BitConverter.ToSingle(bytes, 0);
        cubePosition.y = BitConverter.ToSingle(bytes, 4);
        cubePosition.z = BitConverter.ToSingle(bytes, 8);

        //return vec;
    }

    private Vector3 ByteToVec(byte[] bytes)
    {
        Vector3 vec;
        vec.x = BitConverter.ToSingle(bytes, 0);
        vec.y = BitConverter.ToSingle(bytes, 4);
        vec.z = BitConverter.ToSingle(bytes, 8);

        return vec;
    }

    //case6
    Vector3 IndexTipPos;
    private void ReceivePos(byte[] bytes)
    {
        //Debug.Log("Receive Pos2");
        //byte byteType = bytes[0];
        //bytes = bytes.Skip(0).ToArray();

        IndexTipPos = ByteToVec(bytes);
         //bornPos[(int)byteType] = positionVec;
    }

    private void OnDestroy()
    {
        udpClient.Close();
    }
}

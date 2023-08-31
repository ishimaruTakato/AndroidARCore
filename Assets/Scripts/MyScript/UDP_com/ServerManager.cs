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

                    case "RightPos":
                        handByte.ReceiveRightPos(rightBornPos);
                        //handByte.TestReceivePos(IndexTipPos);
                        //message.text = "Position Get";
                        break;

                    case "RightRot":
                        handByte.ReceiveRightRot(rightBornRot);
                        //message.text = "Rotation Get";
                        break;

                    case "LeftPos":
                        handByte.ReceiveLeftPos(leftBornPos);
                        //handByte.TestReceivePos(IndexTipPos);
                        //message.text = "Position Get";
                        break;

                    case "LeftRot":
                        handByte.ReceiveLeftRot(leftBornRot);
                        //message.text = "Rotation Get";
                        break;

                        //case "TestPos":
                        //    handByte.TestReceivePos(IndexTipPosByte);
                        //    message.text = "Test Pos Get";
                        //    break;
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
        //rightVectorPos -6
        //rightVectorRot -7
        //leftVectorPos -8
        //leftVectorRot -9

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
                this.ReceiveRightPos(getByte);
                subject.OnNext("RightPos");
                break;

            case 7:                
                this.ReceiveRightRot(getByte);
                subject.OnNext("RightRot");
                break;

            case 8:
                this.ReceiveLeftPos(getByte);
                subject.OnNext("LeftPos");
                break;

            case 9:
                this.ReceiveLeftRot(getByte);
                subject.OnNext("LeftRot");
                break;

                //case 8:
                //    this.TestReceivePos(getByte);
                //    subject.OnNext("TestPos");
                //    break;
        }

        getUdp.BeginReceive(OnReceived, getUdp);
    }

    private Vector3 ByteToVec(byte[] bytes)
    {
        Vector3 vec;
        vec.x = BitConverter.ToSingle(bytes, 0);
        vec.y = BitConverter.ToSingle(bytes, 4);
        vec.z = BitConverter.ToSingle(bytes, 8);

        return vec;
    }

    //case5
    private void ReceiveVec(byte[] bytes)
    {
        Vector3 cameraCubePos = ByteToVec(bytes);

        cubePosition.x = cameraCubePos.x;
        cubePosition.y = cameraCubePos.y;
        cubePosition.z = cameraCubePos.z;
    }    

    //case6    
    private Vector3[] rightBornPos = new Vector3[24];
    private Vector3[] rightBornRot = new Vector3[24];

    private void ReceiveRightPos(byte[] bytes)
    {
        byte index = bytes[0];
        bytes = bytes.Skip(1).ToArray();

        this.rightBornPos[index] = ByteToVec(bytes);
    }    

    //case7
    private void ReceiveRightRot(byte[] bytes)
    {        
        byte index = bytes[0];        
        bytes = bytes.Skip(1).ToArray();

        this.rightBornRot[index] = ByteToVec(bytes);
    }

    //case8    
    private Vector3[] leftBornPos = new Vector3[24];
    private Vector3[] leftBornRot = new Vector3[24];

    private void ReceiveLeftPos(byte[] bytes)
    {
        byte index = bytes[0];
        bytes = bytes.Skip(1).ToArray();

        this.leftBornPos[index] = ByteToVec(bytes);
    }

    //case9
    private void ReceiveLeftRot(byte[] bytes)
    {
        byte index = bytes[0];
        bytes = bytes.Skip(1).ToArray();

        this.leftBornRot[index] = ByteToVec(bytes);
    }



    //case8
    //Vector3 IndexTipPos;
    //byte[] IndexTipPosByte;
    //private void TestReceivePos(byte[] bytes)
    //{
    //    //int index = ((int)bytes[0] * 10) + (int)bytes[1];
    //    //bytes = bytes.Skip(2).ToArray();

    //    //Debug.Log("Test Receive Pos Before:" + string.Join("", bytes.Select(n => n.ToString())));
    //    //Debug.Log("Test Receive Pos After:" + index + "\n" + string.Join("", bytes.Select(n => n.ToString())));

    //    IndexTipPosByte = bytes;
    //}

    private void OnDestroy()
    {
        udpClient.Close();
    }
}

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
    AndroidManager androidManager;
    ObjectManager objectManager;

    private UdpClient udpClient;
    private Subject<string> subject = new Subject<string>();
    [SerializeField] Text message;   

    //[SerializeField] GameObject cube;
    private Vector3 cubePosition;


    // Start is called before the first frame update
    void Start()
    {
        handByte = HandByte.Instance;
        androidManager = AndroidManager.Instance;
        objectManager = ObjectManager.Instance;

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

                    case "VR":
                        //message.text = "Position Change";
                        //cube.transform.position = cubePosition;
                        androidManager.ReceiveVR(VRPos, VRRot);
                        break;

                    case "RightPos":
                        handByte.ReceiveRightPos(rightBornPos);                        
                        break;

                    case "RightRot":
                        handByte.ReceiveRightRot(rightBornRot);
                        break;

                    case "LeftPos":
                        handByte.ReceiveLeftPos(leftBornPos);
                        break;

                    case "LeftRot":
                        handByte.ReceiveLeftRot(leftBornRot);
                        break;

                    case "PositionAdjust":
                        androidManager.PositionAdjust(realAndroidPos,androidEularY);
                        break;

                    case "ObjectHighLight":
                        objectManager.ChangeObjectHighLight(highLightObjectIndex,highLightOnOff);
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
        //positionAdjust -10
        //objectHighLight -11

        switch (byteType)
        {
            case 3:
                var message = Encoding.UTF8.GetString(getByte);
                Debug.Log("mes");
                subject.OnNext("M:" + message);

                break;

            case 5:                
                this.ReceiveVR(getByte);             
                subject.OnNext("VR");
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

            case 10:
                this.PositionAdjust(getByte);
                subject.OnNext("PositionAdjust");
                break;

            case 11:
                this.ReceiveObjectHighLight(getByte);
                subject.OnNext("ObjectHighLight");
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
    private Vector3 VRPos;
    private Vector3 VRRot;
    private void ReceiveVR(byte[] bytes)
    {
        
        this.VRPos.x = BitConverter.ToSingle(bytes, 0);
        this.VRPos.y = BitConverter.ToSingle(bytes, 4);
        this.VRPos.z = BitConverter.ToSingle(bytes, 8);

        this.VRRot.x = BitConverter.ToSingle(bytes, 12);
        this.VRRot.y = BitConverter.ToSingle(bytes, 16);
        this.VRRot.z = BitConverter.ToSingle(bytes, 20);
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


    //case10
    Vector3 AndroidToVR;
    Vector3 AndroidRot;
    //Vector3 VRRot;

    Vector3 realAndroidPos;
    float androidEularY;

    private void PositionAdjust(byte[] bytes)
    {
        Vector3 vec;
        vec.x = BitConverter.ToSingle(bytes, 0);
        vec.y = BitConverter.ToSingle(bytes, 4);
        vec.z = BitConverter.ToSingle(bytes, 8);
        this.realAndroidPos = vec;

        this.androidEularY = BitConverter.ToSingle(bytes, 12);
    }   


    //case 11
    int highLightOnOff;
    int highLightObjectIndex;
    private void ReceiveObjectHighLight(byte[] bytes)
    {
        highLightOnOff = (int)bytes[0];
        bytes = bytes.Skip(1).ToArray();

        highLightObjectIndex = (int)bytes[0];
    }

    //caseXX
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

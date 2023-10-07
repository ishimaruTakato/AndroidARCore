using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class HandByte : SingletonMonoBehaviour<HandByte>
{
    //[SerializeField] OVRSkeleton toSkeleton;
    //[SerializeField] OVRCustomSkeleton toCSkeleton;

    [SerializeField] GameObject testHandCube;

    //[SerializeField] Transform parentVR;
    [SerializeField] Transform Adjust;
    [SerializeField] Transform roomObject;
    

    [SerializeField] Transform[] myBones = new Transform[24];
    [SerializeField] Transform[] myLeftBones = new Transform[24];

    private Vector3[] rightBornPos = new Vector3[24];
    private Vector3[] rightBornRot = new Vector3[24];
    private Vector3[] leftBornPos = new Vector3[24];
    private Vector3[] leftBornRot = new Vector3[24];

    private Vector3 VRHandDiff;
    private Vector3 VRHandReferenceEular;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdataHand();
    }

    void UpdataHand()
    {
        Vector3 posVec = Quaternion.Euler(0, roomObject.transform.eulerAngles.y, 0) * (Adjust.localPosition);
        Vector3 rotVec = new Vector3(0, Adjust.transform.localEulerAngles.y, 0);
        

        for (int i=0; i<24; i++)
        {
            

            //Bornの位置更新
            myBones[i].position = rightBornPos[i] +posVec;
            myBones[i].eulerAngles = rightBornRot[i] +rotVec;

            //位置補正
            Vector3 missPosVec = myBones[i].position - Adjust.transform.position;
            Vector3 realPosVec = Quaternion.Euler(0, Adjust.transform.localEulerAngles.y, 0) * missPosVec;
            myBones[i].position += realPosVec - missPosVec;

            myLeftBones[i].position = leftBornPos[i] +posVec;
            myLeftBones[i].eulerAngles = leftBornRot[i]  +rotVec;

            //位置補正
            missPosVec = myLeftBones[i].position - Adjust.transform.position;
            realPosVec = Quaternion.Euler(0, Adjust.transform.localEulerAngles.y, 0) * missPosVec;
            myLeftBones[i].position += realPosVec - missPosVec;
        }
    }

    private Vector3 ByteToVec(byte[] bytes)
    {
        Vector3 vec;
        vec.x = BitConverter.ToSingle(bytes, 0);
        vec.y = BitConverter.ToSingle(bytes, 4);
        vec.z = BitConverter.ToSingle(bytes, 8);

        return vec;
    }

    public void ReceiveRightPos(Vector3[] bornPosVec)
    {        
        rightBornPos = bornPosVec;
    }

    public void ReceiveRightRot(Vector3[] bornRotVec)
    {        
        rightBornRot = bornRotVec;        
    }

    public void ReceiveLeftPos(Vector3[] bornPosVec)
    {
        leftBornPos = bornPosVec;
    }

    public void ReceiveLeftRot(Vector3[] bornRotVec)
    {
        leftBornRot = bornRotVec;
    }

    public void TestReceivePos(byte[] bytes)
    {
        //Debug.Log("Receive Pos2");
        //byte byteType = bytes[0];
        //bytes = bytes.Skip(0).ToArray();

        //Debug.Log("Receive Index");
        //testHandCube.transform.position = IndexPos;


        //bornPos[(int)byteType] = positionVec;

        int index = ((int)bytes[0] * 10) + (int)bytes[1];
        bytes = bytes.Skip(2).ToArray();

        Debug.Log("Test Receive Pos Before:" + string.Join("", bytes.Select(n => n.ToString())));
        Debug.Log("Test Receive Pos After:" + index + "\n" + string.Join("", bytes.Select(n => n.ToString())));

        Vector3 vec = ByteToVec(bytes);
        testHandCube.transform.position = vec;
    }    
}

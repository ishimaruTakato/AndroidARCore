using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class HandByte : SingletonMonoBehaviour<HandByte>
{
    [SerializeField] OVRSkeleton toSkeleton;
    [SerializeField] OVRCustomSkeleton toCSkeleton;
    [SerializeField] Transform[] myBones = new Transform[24];

    [SerializeField] GameObject testHandCube;
    [SerializeField] Text comformMessage;

    //List<byte[]> bornPosByte = new List<byte[]>();
    //List<byte[]> bornRotByte = new List<byte[]>();
    Vector3[] bornPos = new Vector3[24];
    Vector3[] bornRot = new Vector3[24];

    // Start is called before the first frame update
    void Start()
    {
        //for (int i = 0; i < 24; i++)
        //{
        //    bornPosByte.Add(new Byte[] { });
        //    bornRotByte.Add(new Byte[] { });
        //}
    }

    // Update is called once per frame
    void Update()
    {
        UpdataHand();
    }

    void UpdataHand()
    {
        Vector3 veve;
        for(int i=0; i<24; i++)
        {
            //toCSkeleton.Bones[i].Transform.position = bornPos[i];
            myBones[i].position = bornPos[i];
            myBones[i].eulerAngles = bornRot[i];            
            
        }
    }

    public void ReceivePos(byte[] bytes)
    {
        //Debug.Log("Receive Pos2");
        //byte byteType = bytes[0];
        //bytes = bytes.Skip(0).ToArray();

        Vector3 positionVec = GetByteVec(bytes);
        testHandCube.transform.position = positionVec;
        //bornPos[(int)byteType] = positionVec;
    }

    public void TestReceivePos(Vector3 IndexPos)
    {
        //Debug.Log("Receive Pos2");
        //byte byteType = bytes[0];
        //bytes = bytes.Skip(0).ToArray();

        Debug.Log("Receive Index");
        testHandCube.transform.position = IndexPos;
        //bornPos[(int)byteType] = positionVec;
    }



    public void ReceiveRot(byte[] bytes)
    {
        Debug.Log("Receive Rot2");
        byte byteType = bytes[0];
        bytes = bytes.Skip(0).ToArray();

        Vector3 rotationVec = GetByteVec(bytes);
        bornRot[(int)byteType] = rotationVec;
    }

    //void ReceiveHand()
    //{
    //    for (int i = 0; i < 24; i++)
    //    {
    //        toCSkeleton.Bones[i].Transform.position = GetByteVec(bornPosByte[i]);
    //        toCSkeleton.Bones[i].Transform.eulerAngles = GetByteVec(bornRotByte[i]);
    //    }
    //}

    Vector3 GetByteVec(byte[] bytes)
    {
        Vector3 vector3;

        vector3.x = BitConverter.ToSingle(bytes, 0);
        vector3.y = BitConverter.ToSingle(bytes, 4);
        vector3.z = BitConverter.ToSingle(bytes, 8);

        return vector3;
    }
}

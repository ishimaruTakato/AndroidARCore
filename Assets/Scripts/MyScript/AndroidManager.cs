using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AndroidManager : SingletonMonoBehaviour<AndroidManager>
{
    //TEST用
    ClientManager clientManager;
    ServerManager serverManager;
    HandByte handByte;

    [SerializeField] Camera androidCamera;
    [SerializeField] Text comformMessage;
    [SerializeField] Text comformMessage2;

    [SerializeField] GameObject TestParentVR;
    [SerializeField] GameObject TestParentAndroid;

    [SerializeField] GameObject ARSessionOrigin;
    [SerializeField] GameObject roomObject;

    // Start is called before the first frame update
    void Start()
    {
        clientManager = ClientManager.Instance;
        serverManager = ServerManager.Instance;
        handByte = HandByte.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        SendCameraPos();

        TestParentAndroid.transform.position = androidCamera.transform.position;
        TestParentAndroid.transform.eulerAngles = androidCamera.transform.eulerAngles;
    }    

    private void SendCameraPos()
    {
        clientManager.SendVector(androidCamera.transform.position);        
    }

    //case 5
    public void ReceiveVR(Vector3 VRPos , Vector3 VRRot)
    {
        //ユーザの顔位置更新
        TestParentVR.transform.position = VRPos;
        TestParentVR.transform.eulerAngles = VRRot;
    }

    //case 10
    //Vector3 VRDiff;
    //Vector3 VRReferencePoint;
    //Vector3 VRReferenceEular;
    //Vector3 beforeVRPos;
    //Vector3 beforeVRRot;

    public void PositionAdjust(Vector3 realAndroidPos, float androidEularY)
    {
        //realAndroidPosはVRからAndroidに移動するベクトル
        Debug.Log("Position Adjust");        
        
        Vector3 moveVec = Quaternion.Euler(0,TestParentVR.transform.eulerAngles.y,0) * realAndroidPos;
        ARSessionOrigin.transform.position = TestParentVR.transform.position + moveVec;
        ARSessionOrigin.transform.eulerAngles = new Vector3(0, TestParentVR.transform.eulerAngles.y + androidEularY, 0);
        comformMessage2.text = "ARCamera: x=" + androidCamera.transform.localPosition.x + "--y=" + androidCamera.transform.localPosition.y + "--z=" + androidCamera.transform.localPosition.z;

        roomObject.transform.position = TestParentVR.transform.position;
        roomObject.transform.eulerAngles = new Vector3(0,TestParentVR.transform.eulerAngles.y ,0);
    }
}

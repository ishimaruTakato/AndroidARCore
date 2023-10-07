using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;


public class AndroidManager : MonoBehaviour
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
    [SerializeField] GameObject Adjust;
    [SerializeField] GameObject VRAdjust;

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
        //SendCameraPos();
        //TestParentAndroid.transform.position = androidCamera.transform.position;
        //TestParentAndroid.transform.eulerAngles = androidCamera.transform.eulerAngles;
    }    

    private void SendCameraPos()
    {
        clientManager.SendVector(androidCamera.transform.position);        
    }

    //case 5
    public void ReceiveVR(Vector3 VRPos , Vector3 VRRot)
    {
        //Debug.Log("Adjust Y =" + Adjust.transform.localEulerAngles.y);
        //comformMessage.text = "Adjust Y =" + Adjust.transform.localEulerAngles.y;


        //ユーザの顔位置更新
        TestParentVR.transform.position = VRPos + Quaternion.Euler(0, roomObject.transform.eulerAngles.y , 0) * (Adjust.transform.localPosition);
        TestParentVR.transform.eulerAngles = VRRot + new Vector3(0, Adjust.transform.localEulerAngles.y,0);

        //
        Vector3 missPosVec = TestParentVR.transform.position - Adjust.transform.position;
        Vector3 realPosVec = Quaternion.Euler(0, Adjust.transform.localEulerAngles.y, 0) * missPosVec;
        TestParentVR.transform.position += realPosVec - missPosVec; 

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
        comformMessage.text = "Position OK!";

        //ARカメラからのベクトルで移動
        //Vector3 pos = androidCamera.transform.position;
        //Vector3 moveTesVec = Quaternion.Euler(0, androidCamera.transform.eulerAngles.y, 0) * realAndroidPos;


        Vector3 moveVec = Quaternion.Euler(0,TestParentVR.transform.eulerAngles.y,0) * (realAndroidPos);
        ARSessionOrigin.transform.position = TestParentVR.transform.position + moveVec;
        ARSessionOrigin.transform.eulerAngles = new Vector3(0, TestParentVR.transform.eulerAngles.y + androidEularY, 0);

        roomObject.transform.position = TestParentVR.transform.position;
        roomObject.transform.eulerAngles = new Vector3(0,TestParentVR.transform.eulerAngles.y, 0);

        Adjust.GetComponent<ARAnchor>().enabled = true;
        //VRAdjust.GetComponent<ARAnchor>().enabled = true;
    }
}

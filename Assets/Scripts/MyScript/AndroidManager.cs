using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AndroidManager : SingletonMonoBehaviour<AndroidManager>
{
    //TEST—p
    ClientManager clientManager;
    ServerManager serverManager;
    HandByte handByte;

    [SerializeField] Camera androidCamera;
    [SerializeField] Text posMessage;

    [SerializeField] GameObject parentVR;

    [SerializeField] GameObject TestParentVR;
    [SerializeField] GameObject TestParentAndroid;

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

    /*
         [SerializeField] OVRSkeleton skeleton;

      transform.position = skeleton.Bones[(int)OVRSkeleton.BoneId.Hand_IndexTip].Transform.position;
        positionText.text = "position: x-"+transform.position.x+
                            ", y-"+transform.position.y+", z-"+transform.position.z;
      
      
     */

    private void SendCameraPos()
    {
        clientManager.SendVector(androidCamera.transform.position);
        //posMessage.text = "This.CameraPos: x=" +androidCamera.transform.position.x+
        //                  "\ny=" + androidCamera.transform.position.y +
        //                  "\nz=" + androidCamera.transform.position.z;
    }

    //case 5
    Vector3 tenDiff = new Vector3(0,-0.4f,-0.06f);
    public void ReceiveVR(Vector3 VRPos , Vector3 VRRot)
    {
        Vector3 tenDiffQ = Quaternion.Euler(parentVR.transform.eulerAngles) * tenDiff;
        TestParentVR.transform.position = VRPos + parentVR.transform.position + tenDiffQ;
        TestParentVR.transform.eulerAngles = VRRot + parentVR.transform.eulerAngles;
    }

    //case 10
    Vector3 Diff;
    Vector3 beforeVRPos;
    Vector3 beforeVRRot;

    public void PositionAdjust(Vector3 realAndroidPos)
    {
        Debug.Log("Position Adjust");
        //Vector3 AndroidDiff = androidCamera.transform.eulerAngles - AndroidRot;
        //Vector3 VRDiff = VRRot + AndroidDiff; 
        //parentVR.transform.position = androidCamera.transform.position + AndroidToVR;

        beforeVRPos = TestParentVR.transform.position;
        beforeVRRot = TestParentVR.transform.eulerAngles;
        
        
        Vector3 tempPos= androidCamera.transform.position - realAndroidPos;
        tempPos.y = 1.15f;
        TestParentVR.transform.position = tempPos;
        TestParentVR.transform.eulerAngles = androidCamera.transform.eulerAngles;

        
        parentVR.transform.position = tempPos - new Vector3(0,1.15f,0);
        Vector3 tempRot = new Vector3(0, androidCamera.transform.eulerAngles.y, 0);
        parentVR.transform.eulerAngles = tempRot;

        //TestParentVR.transform.position = 
        //    androidCamera.transform.position + AndroidToVR;
        //TestParentVR.transform.eulerAngles = VRDiff;

        //TestParentAndroid.transform.position = androidCamera.transform.position;
        //TestParentAndroid.transform.eulerAngles = androidCamera.transform.eulerAngles;
        
    }
}

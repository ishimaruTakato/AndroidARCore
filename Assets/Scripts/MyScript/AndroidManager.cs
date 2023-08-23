using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AndroidManager : MonoBehaviour
{
    //master
    ClientManager clientManager;
    ServerManager serverManager;

    [SerializeField] Camera androidCamera;
    [SerializeField] Text posMessage;

    // Start is called before the first frame update
    void Start()
    {
        clientManager = ClientManager.Instance;
        serverManager = ServerManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        SendCameraPos();
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
        posMessage.text = "This.CameraPos: x=" +androidCamera.transform.position.x+
                          "\ny=" + androidCamera.transform.position.y +
                          "\nz=" + androidCamera.transform.position.z;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidManager : MonoBehaviour
{
    ClientManager clientManager;
    ServerManager serverManager;

    [SerializeField] Camera androidCamera;

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

    private void SendCameraPos()
    {
        clientManager.SendVector(androidCamera.transform.position);
    }
}

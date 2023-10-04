using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : SingletonMonoBehaviour<ObjectManager>
{
    [SerializeField] List<GameObject> inRoomObjects;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount >0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                //‰Ÿ‚µ‚½Žž

            }

            if (touch.phase == TouchPhase.Ended)
            {
                //—£‚µ‚½Žž
                
            }

            
        }
    }

    //objectHighLight -11
    //On == 1 , OFF == 0
    public void ChangeObjectHighLight(int objectIndex, int onOff)
    {
        if(onOff == 1)
        {
            inRoomObjects[objectIndex].layer = 6;
        }
        else
        {
            inRoomObjects[objectIndex].layer = 0;
        }
    }

    public void EmphasisReset()
    {
        foreach(var roomObject in inRoomObjects)
        {
            roomObject.layer = 0;
        }
    }
}

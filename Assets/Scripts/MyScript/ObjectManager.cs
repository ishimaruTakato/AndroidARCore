using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : SingletonMonoBehaviour<ObjectManager>
{
    [SerializeField] List<GameObject> inRoomObjects;
    [SerializeField] GameObject ARrightHand;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.touchCount >0)
        //{
        //    Touch touch = Input.GetTouch(0);

        //    if (touch.phase == TouchPhase.Began)
        //    {
        //        //押した時

        //    }

        //    if (touch.phase == TouchPhase.Ended)
        //    {
        //        //離した時
                
        //    }

            
        //}
    }

    //objectHighLight -11
    //On == 1 , OFF == 0
    public void ChangeObjectHighLight(int objectIndex)
    {
        inRoomObjects[objectIndex].layer = 6;


        //if (onOff == 1)
        //{
        //}
        //else
        //{
        //    inRoomObjects[objectIndex].layer = 0;
        //}
    }

    public void EmphasisReset()
    {
        foreach (var roomObject in inRoomObjects)
        {
            roomObject.layer = 0;
        }

        ARrightHand.layer = 7;

    }

    public void ARHandOpen(Vector3 pos)
    {
        ARrightHand.layer = 0;
        ARrightHand.transform.localPosition = pos;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawManager : SingletonMonoBehaviour<DrawManager>
{
    [SerializeField] GameObject PointObjectPrefab;
    [SerializeField] Transform PaintPosition;
    private GameObject CurrentLineObject = null;
    private List<GameObject> lineObjects;
    [SerializeField] GameObject PaintSphere;

    private bool drawFlag = false;

    private Transform Pointer
    {
        get
        {
            return PaintPosition;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        lineObjects = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
        var pointer = Pointer;
        if (pointer == null)
        {
            Debug.Log("pointer not defiend");
            return;
        }

        if (drawFlag)
        {
            if (CurrentLineObject == null)
            {
                CurrentLineObject = Instantiate(PointObjectPrefab, this.PaintPosition.position, Quaternion.identity);
            }

            LineRenderer render = CurrentLineObject.GetComponent<LineRenderer>();
            int NextPositionIndex = render.positionCount;
            render.positionCount = NextPositionIndex + 1;
            render.SetPosition(NextPositionIndex, pointer.position);

            PaintSphere.transform.position = pointer.position;

        }
        else
        {
            if (CurrentLineObject != null)
            {
                lineObjects.Add(CurrentLineObject);
                CurrentLineObject = null;
            }
        }
    }

    public void DrawFlagSwitch(bool flag)
    {
        drawFlag = flag;
        //if (drawFlag == true) PaintSphere.SetActive(true);
        //else if (drawFlag == false) PaintSphere.SetActive(false);
    }

    public void ReceivePaintPos(Vector3 PaintPos)
    {
        PaintPosition.localPosition = PaintPos;
        PaintSphere.transform.position = Pointer.position;
    }

    public void LineUndo()
    {
        Destroy(lineObjects[lineObjects.Count - 1]);
        lineObjects.RemoveAt(lineObjects.Count - 1);
    }

    public void LineAllDelete()
    {
        foreach (var i in lineObjects)
        {
            Destroy(i);
        }

        lineObjects.Clear();
    }
}

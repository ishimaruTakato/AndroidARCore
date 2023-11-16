using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestUI : MonoBehaviour
{
    [SerializeField] Canvas testCanvas;
    [SerializeField] RectTransform testButton;
    [SerializeField] RectTransform testButton2;
    [SerializeField] Text testText;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!testCanvas.enabled) return;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                //‰Ÿ‚µ‚½
                if(RectTransformUtility.RectangleContainsScreenPoint(testButton,Input.mousePosition))
                {
                    OnDown();
                }

                if (RectTransformUtility.RectangleContainsScreenPoint(testButton2, Input.mousePosition))
                {
                    testText.text = "Down2";
                }
            }

            if (touch.phase == TouchPhase.Ended)
            {
                //—£‚µ‚½
                if (RectTransformUtility.RectangleContainsScreenPoint(testButton, Input.mousePosition))
                {
                    OnEnd();
                }

                if (RectTransformUtility.RectangleContainsScreenPoint(testButton2, Input.mousePosition))
                {
                    testText.text = "End2";
                }
            }

            if(touch.phase == TouchPhase.Stationary)
            {                
                //‰Ÿ‚µ‚Á‚Ï
                if (RectTransformUtility.RectangleContainsScreenPoint(testButton, Input.mousePosition))
                {
                    OnStat();
                }

                if (RectTransformUtility.RectangleContainsScreenPoint(testButton2, Input.mousePosition))
                {
                    testText.text = "Stat2";
                }
            }

            if (touch.phase == TouchPhase.Moved)
            {
                //‰Ÿ‚µ‚Ä“®‚©‚·
                if (RectTransformUtility.RectangleContainsScreenPoint(testButton, Input.mousePosition))
                {
                    OnPushing();
                }
            }
        }
    }

    void OnDown()
    {
        testText.text ="OnDown";
    }

    void OnEnd()
    {
        testText.text = "OnEnd";
    }

    void OnStat()
    {
        testText.text = "OnStat";
    }

    void OnPushing()
    {
        testText.text = "OnPushing";
    }
}

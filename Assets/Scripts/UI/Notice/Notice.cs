using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.UI;

public class Notice : MonoBehaviour
{

    private UIWarpContent uiWarpContent;
    public ToggleGroup toggleGroup;
//    private List<ActivityData> activityDatas;
    public GameObject RightBg;

    // Use this for initialization
    void Start()
    {
        LogicEnginerScript.Instance.GetComponent<GetNoticeRequest>().CallBack = GetNoticeData;
        LogicEnginerScript.Instance.GetComponent<GetNoticeRequest>().OnRequest();

//        uiWarpContent = gameObject.transform.GetComponentInChildren<UIWarpContent>();
//        uiWarpContent.onInitializeItem = onInitializeItem;
    }

    public void GetNoticeData(string result)
    {

    }

    private void onInitializeItem(GameObject go, int dataindex)
    {

    }

    public void OnToggleValueChange(GameObject go, bool isOn, int dataindex)
    {
       
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.UI;

public class Notice : MonoBehaviour
{

    public UIWarpContent uiWarpContent;
    public ToggleGroup toggleGroup;
//    private List<ActivityData> activityDatas;
    public GameObject RightBg;
    private List<NoticeData> noticeDatas = new List<NoticeData>();

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/ActivityAndNoticePanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);
        return obj;
    }

    // Use this for initialization
    void Start()
    {
        OtherData.s_notice = this;
        LogicEnginerScript.Instance.GetComponent<GetNoticeRequest>().CallBack = GetNoticeData;
        LogicEnginerScript.Instance.GetComponent<GetNoticeRequest>().OnRequest();

//        uiWarpContent = gameObject.transform.GetComponentInChildren<UIWarpContent>();
        uiWarpContent.onInitializeItem = onInitializeItem;
    }

    public void GetNoticeData(string result)
    {
        NoticelDataScript.getInstance().initJson(result);

        foreach (var noticeData in NoticelDataScript.getInstance().getNoticeDataList())
        {
            if (noticeData.type == 1)
            {
                noticeDatas.Add(noticeData);
            }
        }

        uiWarpContent.Init(noticeDatas.Count);
    }

    private void onInitializeItem(GameObject go, int dataindex)
    {
        Toggle toggle = go.GetComponent<Toggle>();
        toggle.group = toggleGroup;
        if (dataindex == 0)
        {
            toggle.isOn = true;
            go.transform.GetChild(0).gameObject.SetActive(false);
           
        }
        else
        {
            toggle.isOn = false;
        }

        toggle.onValueChanged.RemoveAllListeners();
        toggle.onValueChanged.AddListener((b) => OnToggleValueChange(go, b, dataindex));
    }

    public void OnToggleValueChange(GameObject go, bool isOn, int dataindex)
    {
        Debug.Log("isOn" + isOn);
        if (isOn)
        {
            go.transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            go.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
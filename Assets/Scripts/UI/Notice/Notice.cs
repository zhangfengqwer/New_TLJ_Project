using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.UI;

public class Notice : MonoBehaviour
{
    public class ActivityData
    {
        public int ActivityId;
        public string Title;
        public string ImageUrl;
    }

    private UIWarpContent uiWarpContent;

    public ToggleGroup toggleGroup;

    private List<ActivityData> activityDatas;
    public GameObject RightBg;

    // Use this for initialization
    void Start()
    {
        LogicEnginerScript.Instance.GetComponent<GetAcitivityRequest>().CallBack = GetActivityData;
        LogicEnginerScript.Instance.GetComponent<GetAcitivityRequest>().OnRequest();

        uiWarpContent = gameObject.transform.GetComponentInChildren<UIWarpContent>();
        uiWarpContent.onInitializeItem = onInitializeItem;
    }

    public void GetActivityData(string result)
    {
        JsonData jsonData = JsonMapper.ToObject(result);
        Debug.Log(jsonData["activityDatas"].ToString());
        activityDatas = JsonMapper.ToObject<List<ActivityData>>(jsonData["activityDatas"].ToString());
        uiWarpContent.Init(activityDatas.Count);
    }

    private void onInitializeItem(GameObject go, int dataindex)
    {
        Toggle toggle = go.GetComponent<Toggle>();
        toggle.group = toggleGroup;
        if (dataindex == 0)
        {
            toggle.isOn = true;
            go.transform.GetChild(0).gameObject.SetActive(false);
            GameObject panel = ActivityManager.getActivityPanel(dataindex + 1);
            if (panel != null)
            {
                panel.transform.SetParent(RightBg.transform);
                panel.transform.localScale = new Vector3(1, 1, 1);
                panel.transform.localPosition = new Vector3(0, 0, 0);
            }
        }
        else
        {
            toggle.isOn = false;
        }

        toggle.onValueChanged.RemoveAllListeners();
        toggle.onValueChanged.AddListener((b) => OnToggleValueChange(go, b, dataindex));

        //设置数据
        ActivityData activityData = activityDatas[dataindex];

        go.transform.GetChild(2).GetComponent<Text>().text = activityData.Title;
    }

    public void OnToggleValueChange(GameObject go, bool isOn, int dataindex)
    {
        Debug.Log("isOn" + isOn);
        if (isOn)
        {
            go.transform.GetChild(0).gameObject.SetActive(false);
            GameObject  panel = ActivityManager.getActivityPanel(dataindex + 1);
            if (panel != null)
            {
                panel.transform.SetParent(RightBg.transform);
                panel.transform.localScale = new Vector3(1, 1, 1);
                panel.transform.localPosition = new Vector3(0, 0, 0);
            }
        }
        else
        {
            go.transform.GetChild(0).gameObject.SetActive(true);
        }
    }


    public void OnToggleValueChange(bool isOn)
    {
        Debug.Log("isOn" + isOn);
        if (isOn)
        {
        }
    }
}
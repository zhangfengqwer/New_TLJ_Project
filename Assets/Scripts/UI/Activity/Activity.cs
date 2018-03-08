using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.UI;

public class Activity : MonoBehaviour
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
    private List<NoticeData> noticeDatas = new List<NoticeData>();
    public GameObject RightBg;
    public Button BtnActivity;
    public Button BtnNotice;
    public Image TabImage;
    private GameObject Content;
    public GameObject ItemNotice;

    // Use this for initialization
    void Start()
    {
        OtherData.s_activity = this;

        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("Activity_hotfix", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.Activity_hotfix", "Start", null, null);
            return;
        }

        LogicEnginerScript.Instance.GetComponent<GetAcitivityRequest>().CallBack = GetActivityData;
        LogicEnginerScript.Instance.GetComponent<GetAcitivityRequest>().OnRequest();

        LogicEnginerScript.Instance.GetComponent<GetNoticeRequest>().CallBack = GetNoticeData;
        LogicEnginerScript.Instance.GetComponent<GetNoticeRequest>().OnRequest();

        BtnActivity.onClick.AddListener(() => { OnClickButton(0); });
        BtnNotice.onClick.AddListener(() => { OnClickButton(1); });

        uiWarpContent = gameObject.transform.GetComponentInChildren<UIWarpContent>();
        Content = uiWarpContent.gameObject.transform.GetChild(0).gameObject;
    }

    private void OnClickButton(int i)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("Activity_hotfix", "OnClickButton"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.Activity_hotfix", "OnClickButton", null, i);
            return;
        }

        DeleteAllItem(Content);

        switch (i)
        {
            case 0:
                TabImage.transform.localPosition = new Vector3(-83, 0, 0);
                uiWarpContent.onInitializeItem = onInitializeItemActivity;
                uiWarpContent.Init(activityDatas.Count);
                break;
            case 1:
                TabImage.transform.localPosition = new Vector3(83, 0, 0);

                DeleteAllItem(RightBg);

                uiWarpContent.onInitializeItem = onInitializeItemNotice;
                uiWarpContent.Init(noticeDatas.Count);
                break;
        }
    }

    private void DeleteAllItem(GameObject go)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("Activity_hotfix", "DeleteAllItem"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.Activity_hotfix", "DeleteAllItem", null, go);
            return;
        }

        for (int j = go.transform.childCount - 1; j >= 0; j--)
        {
            Destroy(go.transform.GetChild(j).gameObject);
        }
    }

    public void GetNoticeData(string result)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("Activity_hotfix", "GetNoticeData"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.Activity_hotfix", "GetNoticeData", null, result);
            return;
        }

        NoticelDataScript.getInstance().initJson(result);

        foreach (var noticeData in NoticelDataScript.getInstance().getNoticeDataList())
        {
            if (noticeData.type == 1)
            {
                noticeDatas.Add(noticeData);
            }
        }
    }

    public void GetActivityData(string result)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("Activity_hotfix", "GetActivityData"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.Activity_hotfix", "GetActivityData", null, result);
            return;
        }

        JsonData jsonData = JsonMapper.ToObject(result);
        Debug.Log(jsonData["activityDatas"].ToString());
        activityDatas = JsonMapper.ToObject<List<ActivityData>>(jsonData["activityDatas"].ToString());
        BtnActivity.onClick.Invoke();
    }

    private void onInitializeItemNotice(GameObject go, int dataindex)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("Activity_hotfix", "onInitializeItemNotice"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.Activity_hotfix", "onInitializeItemNotice", null, go, dataindex);
            return;
        }

        Toggle toggle = go.GetComponent<Toggle>();
        toggle.group = toggleGroup;
        if (dataindex == 0)
        {
            toggle.isOn = true;
           
            if (go.transform.GetChild(0).gameObject.activeSelf)
            {
                go.transform.GetChild(0).gameObject.SetActive(false);
            }

            InitItemNotice(dataindex);

        }
        else
        {
            toggle.isOn = false;
        }

        toggle.onValueChanged.RemoveAllListeners();
        toggle.onValueChanged.AddListener((b) => OnNoticeToggleValueChange(go, b, dataindex));

        //设置数据
        NoticeData noticeData = noticeDatas[dataindex];

        go.transform.GetChild(2).GetComponent<Text>().text = noticeData.title;
    }

    private void InitItemNotice(int dataindex)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("Activity_hotfix", "InitItemNotice"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.Activity_hotfix", "InitItemNotice", null, dataindex);
            return;
        }

        GameObject go = GameObject.Instantiate(ItemNotice, RightBg.transform);
        go.transform.Find("Text_title").GetComponent<Text>().text = noticeDatas[dataindex].title_limian;
        go.transform.Find("Text_content").GetComponent<Text>().text = noticeDatas[dataindex].content.Replace("^","\n");
        go.transform.Find("Text_from").GetComponent<Text>().text = noticeDatas[dataindex].from;
    }

    private void onInitializeItemActivity(GameObject go, int dataindex)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("Activity_hotfix", "onInitializeItemActivity"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.Activity_hotfix", "onInitializeItemActivity", null, go, dataindex);
            return;
        }

        Toggle toggle = go.GetComponent<Toggle>();
        toggle.group = toggleGroup;
        if (dataindex == 0)
        {
            toggle.isOn = true;
            if (go.transform.GetChild(0).gameObject.activeSelf)
            {
                go.transform.GetChild(0).gameObject.SetActive(false);
            }
            GameObject panel = ActivityManager.getActivityPanel(activityDatas[dataindex]);
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
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("Activity_hotfix", "OnToggleValueChange"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.Activity_hotfix", "OnToggleValueChange", null, go, isOn, dataindex);
            return;
        }

        Debug.Log("isOn" + isOn + ",name:" + go.name);
        if (isOn)
        {
            go.transform.GetChild(0).gameObject.SetActive(false);
            GameObject panel = ActivityManager.getActivityPanel(activityDatas[dataindex]);
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

    public void OnNoticeToggleValueChange(GameObject go, bool isOn, int dataindex)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("Activity_hotfix", "OnNoticeToggleValueChange"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.Activity_hotfix", "OnNoticeToggleValueChange", null, go, isOn, dataindex);
            return;
        }

        Debug.Log("isOn" + isOn + ",name:" + go.name);
        if (isOn)
        {
            go.transform.GetChild(0).gameObject.SetActive(false);
            DeleteAllItem(RightBg);
            InitItemNotice(dataindex);
        }
        else
        {
            go.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
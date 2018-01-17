using LitJson;
using System;
using System.Collections.Generic;
using TLJCommon;
using UnityEngine;

public class GetSignRecordRequest : Request
{
    public bool flag = false;
    public string result;

    private void Awake()
    {
        Tag = Consts.Tag_GetSignRecord;
    }

    void Update()
    {
        if (flag)
        {
            if (OtherData.s_mainScript != null)
            {
                OtherData.s_mainScript.checkRedPoint();
            }

            flag = false;
        }
    }

    public override void OnRequest()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("GetSignRecordRequest_hotfix", "OnRequest"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.GetSignRecordRequest_hotfix", "OnRequest", null, null);
            return;
        }

        JsonData jsonData = new JsonData();
        jsonData["tag"] = Tag;
        jsonData["uid"] = UserData.uid;
        string requestData = jsonData.ToJson();
        LogicEnginerScript.Instance.SendMyMessage(requestData);
    }

    public override void OnResponse(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("GetSignRecordRequest_hotfix", "OnResponse"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.GetSignRecordRequest_hotfix", "OnResponse", null, data);
            return;
        }

        JsonData jsonData = JsonMapper.ToObject(data);
        var code = (int) jsonData["code"];
        if (code == (int) Consts.Code.Code_OK)
        {
            LogicEnginerScript.IsSuccessList.Add(true);
            SignData.SignWeekDays = (int) jsonData["signWeekDays"];
            string update = (string) jsonData["updateTime"];
            WeeklySignScript._signItems = JsonMapper.ToObject<List<SignItem>>(jsonData["sign_config"].ToString());

            DateTime updateTime;
            var isSuccess = DateTime.TryParse(update, out updateTime);
            if (isSuccess)
            {
                int updateTimeYear = updateTime.Year;
                int updateTimeMonth = updateTime.Month;
                int updateTimeDay = updateTime.Day;
                int nowYear = DateTime.Now.Year;
                int nowMonth = DateTime.Now.Month;
                int nowDay = DateTime.Now.Day;
                // LogUtil.Log(updateTimeYear + "-" + updateTimeMonth + "-" + updateTimeDay);
                //通过数据库的更新时间和本地时间作对比，判断是否签到过
                if (updateTimeYear == nowYear && updateTimeMonth == nowMonth && updateTimeDay == nowDay &&
                    SignData.SignWeekDays != 0)
                {
                    SignData.IsSign = true;
                }
                else
                {
                    SignData.IsSign = false;
                }
                //                LogUtil.Log("SignData.IsSign:" + SignData.IsSign
                //                      + "\nSignData.SignWeekDays:" + SignData.SignWeekDays);
            }

            result = data;
            flag = true;
        }
        else
        {
            LogUtil.Log("返回签到数据错误:" + code);
        }
    }
}